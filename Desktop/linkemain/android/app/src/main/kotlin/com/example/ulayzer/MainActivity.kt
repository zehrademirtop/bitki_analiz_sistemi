package com.example.ulayzer

import android.Manifest
import android.content.ComponentName
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import android.util.Log
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.plugins.FlutterPlugin
import io.flutter.embedding.engine.plugins.activity.ActivityAware
import io.flutter.embedding.engine.plugins.activity.ActivityPluginBinding
import io.flutter.plugin.common.MethodCall
import io.flutter.plugin.common.MethodChannel
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class MainActivity : FlutterActivity(), FlutterPlugin, ActivityAware {

    private val CHANNEL = "com.example.ulayzer/permission"
    private val REQUEST_NOTIFICATION_PERMISSION = 101

    override fun configureFlutterEngine(flutterEngine: io.flutter.embedding.engine.FlutterEngine) {
        super.configureFlutterEngine(flutterEngine)
        MethodChannel(flutterEngine.dartExecutor.binaryMessenger, CHANNEL).setMethodCallHandler { call, result ->
            if (call.method == "openSettings") {
                openAppSettingsWithPrompt()
                result.success(true)
            } else {
                result.notImplemented()
            }
        }
    }

    override fun onAttachedToEngine(flutterPluginBinding: FlutterPlugin.FlutterPluginBinding) {
        // FlutterPlugin ile ilgili bağlama işlemleri (opsiyonel)
    }

    override fun onDetachedFromEngine(binding: FlutterPlugin.FlutterPluginBinding) {
        // FlutterPlugin ile ilgili ayrılma işlemleri (opsiyonel)
    }

    override fun onAttachedToActivity(binding: ActivityPluginBinding) {
        // ActivityAware ile ilgili bağlama işlemleri (opsiyonel)
    }

    override fun onDetachedFromActivity() {
        // ActivityAware ile ilgili ayrılma işlemleri (opsiyonel)
    }

    override fun onReattachedToActivityForConfigChanges(binding: ActivityPluginBinding) {
        // Yapılandırma değişiklikleri için (opsiyonel)
    }

    override fun onDetachedFromActivityForConfigChanges() {
        // Yapılandırma değişiklikleri için ayrılma (opsiyonel)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        CoroutineScope(Dispatchers.Main).launch {
            val prefs = getSharedPreferences("app_prefs", MODE_PRIVATE)

            // Otomatik başlatma kontrolü
            if (!prefs.getBoolean("autostart_checked", false)) {
                openAutoStartSettings()
                prefs.edit().putBoolean("autostart_checked", true).apply()
                if (!isNotificationListenerEnabled()) {
                    openNotificationListenerSettingsWithPrompt()
                }
                return@launch
            }

            // Bildirim izni kontrolü (Android 13+ için)
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU && !hasNotificationPermission()) {
                requestNotificationPermissions()
                return@launch
            }

            // Bildirim dinleyici izni kontrolü
            if (!prefs.getBoolean("notification_listener_granted", false)) {
                if (!isNotificationListenerEnabled()) {
                    openNotificationListenerSettingsWithPrompt()
                    return@launch
                } else {
                    prefs.edit().putBoolean("notification_listener_granted", true).apply()
                }
            }

            Log.d("MainActivity", "Tüm izinler tamamlandı")
            // Flutter tarafına izin durumunu bildir (isteğe bağlı)
        }
    }

    private fun hasNotificationPermission(): Boolean {
        return ContextCompat.checkSelfPermission(this, Manifest.permission.POST_NOTIFICATIONS) == PackageManager.PERMISSION_GRANTED
    }

    private fun requestNotificationPermissions() {
        ActivityCompat.requestPermissions(this, arrayOf(Manifest.permission.POST_NOTIFICATIONS), REQUEST_NOTIFICATION_PERMISSION)
    }

    private fun isNotificationListenerEnabled(): Boolean {
        val flat = Settings.Secure.getString(contentResolver, "enabled_notification_listeners")
        Log.d("MainActivity", "Enabled listeners: $flat, Package: $packageName")
        return flat?.contains(packageName) == true
    }

    private fun openNotificationListenerSettingsWithPrompt() {
        try {
            val intent = Intent(Settings.ACTION_NOTIFICATION_LISTENER_SETTINGS).apply {
                addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            }
            if (intent.resolveActivity(packageManager) != null) {
                startActivity(intent)
                Toast.makeText(this, "Bildirim erişimi izni için ayarlara yönlendirildiniz. Lütfen etkinleştirin.", Toast.LENGTH_LONG).show()
            } else {
                when {
                    Build.MANUFACTURER.lowercase().contains("xiaomi") -> {
                        val xiaomiIntent = Intent().apply {
                            component = ComponentName(
                                "com.miui.securitycenter",
                                "com.miui.permcenter.autostart.AutoStartManagementActivity"
                            )
                            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
                        }
                        if (xiaomiIntent.resolveActivity(packageManager) != null) {
                            startActivity(xiaomiIntent)
                            Toast.makeText(this, "Xiaomi ayarlarına yönlendirildiniz, bildirim erişimi izni için onaylayın", Toast.LENGTH_LONG).show()
                        } else {
                            openAppSettingsWithPrompt()
                        }
                    }
                    Build.MANUFACTURER.lowercase().contains("oppo") -> {
                        val oppoIntent = Intent().apply {
                            component = ComponentName(
                                "com.coloros.safecenter",
                                "com.coloros.safecenter.permission.startupmanager.StartupManagerActivity"
                            )
                            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
                        }
                        if (oppoIntent.resolveActivity(packageManager) != null) {
                            startActivity(oppoIntent)
                            Toast.makeText(this, "Oppo ayarlarına yönlendirildiniz, bildirim erişimi izni için onaylayın", Toast.LENGTH_LONG).show()
                        } else {
                            openAppSettingsWithPrompt()
                        }
                    }
                    Build.MANUFACTURER.lowercase().contains("vivo") -> {
                        val vivoIntent = Intent().apply {
                            component = ComponentName(
                                "com.vivo.permissionmanager",
                                "com.vivo.permissionmanager.activity.BgStartUpManagerActivity"
                            )
                            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
                        }
                        if (vivoIntent.resolveActivity(packageManager) != null) {
                            startActivity(vivoIntent)
                            Toast.makeText(this, "Vivo ayarlarına yönlendirildiniz, bildirim erişimi izni için onaylayın", Toast.LENGTH_LONG).show()
                        } else {
                            openAppSettingsWithPrompt()
                        }
                    }
                    else -> openAppSettingsWithPrompt()
                }
            }
        } catch (e: Exception) {
            Log.e("MainActivity", "Bildirim dinleyici ayarı açılamadı: ${e.message}")
            Toast.makeText(this, "Bildirim erişimi ayarlarına erişilemedi, lütfen manuel olarak ayarlara gidin", Toast.LENGTH_LONG).show()
            openAppSettingsWithPrompt()
        }
    }

    private fun openAutoStartSettings() {
        try {
            val manufacturer = Build.MANUFACTURER.lowercase()
            val intent = Intent()

            when {
                manufacturer.contains("xiaomi") -> {
                    intent.component = ComponentName(
                        "com.miui.securitycenter",
                        "com.miui.permcenter.autostart.AutoStartManagementActivity"
                    )
                }
                manufacturer.contains("oppo") -> {
                    intent.component = ComponentName(
                        "com.coloros.safecenter",
                        "com.coloros.safecenter.startupapp.StartupAppListActivity"
                    )
                }
                manufacturer.contains("vivo") -> {
                    intent.component = ComponentName(
                        "com.vivo.permissionmanager",
                        "com.vivo.permissionmanager.activity.BgStartUpManagerActivity"
                    )
                }
                else -> {
                    Toast.makeText(this, "Lütfen cihaz ayarlarından arka plan izni verin", Toast.LENGTH_LONG).show()
                    return
                }
            }

            intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            startActivity(intent)
            Toast.makeText(this, "Arka plan izni için yönlendirildiniz. Onayladıktan sonra bildirim erişimi açılacaktır.", Toast.LENGTH_LONG).show()
        } catch (e: Exception) {
            Log.e("MainActivity", "Autostart sayfası açılamadı: ${e.message}")
            Toast.makeText(this, "Arka plan izni ayarlarına erişilemedi, lütfen manuel olarak ayarlara gidin", Toast.LENGTH_LONG).show()
            openAppSettingsWithPrompt()
        }
    }

    private fun openAppSettingsWithPrompt() {
        val intent = Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS).apply {
            data = Uri.fromParts("package", packageName, null)
            addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
        }
        startActivity(intent)
        Toast.makeText(this, "Lütfen uygulama ayarlarından gerekli izinleri verin", Toast.LENGTH_LONG).show()
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            REQUEST_NOTIFICATION_PERMISSION -> {
                if (grantResults.isNotEmpty() && grantResults.all { it == PackageManager.PERMISSION_GRANTED }) {
                    Log.d("MainActivity", "Bildirim izni verildi")
                } else {
                    Toast.makeText(this, "Bildirim izni gerekli, lütfen izin verin", Toast.LENGTH_LONG).show()
                    requestNotificationPermissions()
                }
            }
        }
    }
}