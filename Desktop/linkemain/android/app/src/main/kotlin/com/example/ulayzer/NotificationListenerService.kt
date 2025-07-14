package com.example.ulayzer

import android.app.Notification
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.IBinder
import android.service.notification.NotificationListenerService
import android.service.notification.StatusBarNotification
import androidx.core.app.NotificationCompat
import com.google.firebase.FirebaseApp
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.auth.FirebaseAuth
import kotlinx.coroutines.*
import okhttp3.*
import org.json.JSONObject
import java.util.regex.Pattern
import kotlin.coroutines.CoroutineContext

class NotificationListenerService : NotificationListenerService(), CoroutineScope {

    private val CHANNEL_ID = "WhatsAppLinkChannel"
    private val NOTIFICATION_ID = 1001
    private val job = Job()
    override val coroutineContext: CoroutineContext
        get() = Dispatchers.IO + job

    override fun onCreate() {
        super.onCreate()
        FirebaseApp.initializeApp(this) // Firebase'i başlat
        println("NotificationListenerService onCreate çağrıldı")
    }

    override fun onBind(intent: Intent?): IBinder? {
        println("NotificationListenerService bağlandı.")
        createNotificationChannel()
        return super.onBind(intent)
    }

    override fun onListenerConnected() {
        super.onListenerConnected()
        println("Bildirim dinleyicisi bağlandı. Erişim izni doğrulandı.")
    }

    private fun createNotificationChannel() {
        val name = "WhatsApp ve Gmail Link Kontrol"
        val descriptionText = "WhatsApp ve Gmail bildirimlerinden link kontrolü"
        val importance = NotificationManager.IMPORTANCE_HIGH
        val channel = NotificationChannel(CHANNEL_ID, name, importance).apply {
            description = descriptionText
        }
        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        notificationManager.createNotificationChannel(channel)
    }

    override fun onNotificationPosted(sbn: StatusBarNotification) {
        println("onNotificationPosted çağrıldı: ${sbn.packageName}")
        val packageName = sbn.packageName
        if (packageName == "com.example.ulayzer") return // Kendi bildirimlerini filtrele

        val notification: Notification = sbn.notification
        val extras = notification.extras
        val title = extras.getCharSequence(Notification.EXTRA_TITLE)?.toString() ?: "No Title"
        val text = extras.getCharSequence(Notification.EXTRA_TEXT)?.toString() ?: ""
        val bigText = extras.getCharSequence(Notification.EXTRA_BIG_TEXT)?.toString() ?: ""
        val summaryText = extras.getCharSequence(Notification.EXTRA_SUMMARY_TEXT)?.toString() ?: ""
        val timestamp = sbn.postTime

        println("Bildirim alındı - Paket: $packageName, Başlık: $title, Metin: $text, Big Text: $bigText, Summary: $summaryText, Zaman: $timestamp")

        when (packageName) {
            "com.whatsapp", "com.whatsapp.w4b", "com.google.android.gm", "com.google.android.gm.lite" -> {
                val combinedText = "$title $text $bigText $summaryText"
                val links = extractLinks(combinedText)
                if (links.isNotEmpty()) {
                    val latestLink = links[0]
                    launch {
                        val isMalicious = checkWithVirusTotal(latestLink)
                        val notificationTitle = when (packageName) {
                            "com.whatsapp", "com.whatsapp.w4b" -> if (isMalicious) "⚠️ WhatsApp - Zararlı Link!" else "✅ WhatsApp - Güvenli Link"
                            "com.google.android.gm", "com.google.android.gm.lite" -> if (isMalicious) "⚠️ Gmail - Zararlı Link!" else "✅ Gmail - Güvenli Link"
                            else -> "Link Durumu"
                        }
                        sendNotification(notificationTitle, latestLink)

                        // Göndericiyi çıkar
                        val sender = extractSenderFromText(combinedText, packageName)
                        val data = mapOf(
                            "source" to when (packageName) {
                                "com.whatsapp", "com.whatsapp.w4b" -> "WhatsApp"
                                "com.google.android.gm", "com.google.android.gm.lite" -> "Gmail"
                                else -> "Unknown"
                            },
                            "link" to latestLink,
                            "isMalicious" to if (isMalicious) 1 else 0,
                            "timestamp" to java.text.SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS").format(java.util.Date(timestamp)),
                            "sender" to sender
                        )

                        try {
                            // UID ile dinamik koleksiyon yolu
                            val userId = FirebaseAuth.getInstance().currentUser?.uid
                            if (userId != null) {
                                println("Veri yazılmaya çalışılıyor, Koleksiyon Yolu: users/$userId/notifications")
                                FirebaseFirestore.getInstance()
                                    .collection("users")
                                    .document(userId)
                                    .collection("notifications")
                                    .add(data)
                                    .addOnSuccessListener {
                                        println("Firestore'a veri yazıldı: $data, Doküman ID: ${it.id}")
                                    }
                                    .addOnFailureListener { e ->
                                        println("Firestore'a yazma hatası: $e")
                                    }
                            } else {
                                println("Kullanıcı oturumu açık değil! UID: null")
                            }
                        } catch (e: Exception) {
                            println("Firestore'a yazma hatası: $e")
                        }
                    }
                } else {
                    println("$packageName bildiriminde link bulunamadı: Text=$text, BigText=$bigText, Summary=$summaryText")
                }
            }
            else -> {
                println("Bu bildirim WhatsApp veya Gmail'den değil, atlanıyor: $packageName")
            }
        }
    }

    private fun extractLinks(text: String): List<String> {
        val links = mutableListOf<String>()
        val regex = Pattern.compile(
            "(https?://(?:[\\w-]+\\.)+[\\w-]+(?:/\\S*)?|www\\.[\\w-]+\\.[\\w-]+(?:/\\S*)?)",
            Pattern.CASE_INSENSITIVE
        )
        val matcher = regex.matcher(text)
        while (matcher.find()) {
            var link = matcher.group(0)!!
            if (link.startsWith("www.")) {
                link = "https://$link"
            }
            links.add(link)
        }
        return links
    }

    private suspend fun checkWithVirusTotal(url: String): Boolean = withContext(Dispatchers.IO) {
        try {
            val apiKey = "95d15f7d2c6189a21daccd9c03b03efdd5eb3520215b33fd568d9033f2442695"
            val client = OkHttpClient()

            val formBody = FormBody.Builder()
                .add("url", url)
                .build()

            val request = Request.Builder()
                .url("https://www.virustotal.com/api/v3/urls")
                .post(formBody)
                .addHeader("x-apikey", apiKey)
                .build()

            val response = client.newCall(request).execute()
            if (!response.isSuccessful) return@withContext false

            val jsonData = response.body?.string() ?: return@withContext false
            val data = JSONObject(jsonData)
            val analysisId = data.getJSONObject("data").getString("id")

            delay(5000) // 5 saniye bekle

            val getRequest = Request.Builder()
                .url("https://www.virustotal.com/api/v3/analyses/$analysisId")
                .get()
                .addHeader("x-apikey", apiKey)
                .build()

            val getResponse = client.newCall(getRequest).execute()
            if (!getResponse.isSuccessful) return@withContext false

            val getJson = getResponse.body?.string() ?: return@withContext false
            val resultData = JSONObject(getJson)
            val stats = resultData.getJSONObject("data").getJSONObject("attributes").getJSONObject("stats")
            val malicious = stats.getInt("malicious")

            return@withContext malicious > 0

        } catch (e: Exception) {
            println("VirusTotal API hatası: $e")
            return@withContext false
        }
    }

    private fun extractSenderFromText(text: String, packageName: String): String {
        if (text.isEmpty()) return "Bilinmeyen Gönderici"
        val lines = text.split("\n")
        if (lines.isNotEmpty()) {
            val firstLine = lines[0].trim()
            when (packageName) {
                "com.whatsapp", "com.whatsapp.w4b" -> {
                    if (firstLine.contains(":") && !firstLine.contains("WhatsApp")) {
                        val colonIndex = firstLine.indexOf(":")
                        return firstLine.substring(0, colonIndex).trim()
                    } else if (!firstLine.contains("http") && !firstLine.contains("https")) {
                        return firstLine
                    }
                }
                "com.google.android.gm", "com.google.android.gm.lite" -> {
                    if (firstLine.contains("<") && firstLine.contains(">")) {
                        val startIndex = firstLine.indexOf("<")
                        if (startIndex > 0) {
                            return firstLine.substring(0, startIndex).trim()
                        }
                    } else if (firstLine.contains("Gmail") && lines.size > 1) {
                        return lines[1].trim().split(":")[0].trim()
                    } else if (!firstLine.contains("http") && !firstLine.contains("https")) {
                        return firstLine
                    }
                }
                else -> {
                    val colonIndex = firstLine.indexOf(":")
                    if (colonIndex != -1) {
                        return firstLine.substring(0, colonIndex).trim()
                    }
                }
            }
        }
        return when (packageName) {
            "com.whatsapp", "com.whatsapp.w4b" -> "Bilinmeyen Gönderici (WhatsApp)"
            "com.google.android.gm", "com.google.android.gm.lite" -> "Bilinmeyen Gönderici (Gmail)"
            else -> "Bilinmeyen Gönderici"
        }
    }

    private fun sendNotification(title: String, body: String) {
        val intent = Intent(Intent.ACTION_VIEW, Uri.parse(body))
        val flags = PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
        val pendingIntent = PendingIntent.getActivity(this, 0, intent, flags)

        val builder = NotificationCompat.Builder(this, CHANNEL_ID)
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(body)
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .setContentIntent(pendingIntent)
            .setAutoCancel(true)

        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        notificationManager.notify(NOTIFICATION_ID, builder.build())
    }

    override fun onDestroy() {
        super.onDestroy()
        job.cancel()
        println("NotificationListenerService onDestroy çağrıldı")
    }
}