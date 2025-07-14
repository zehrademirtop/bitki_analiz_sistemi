package com.example.ulayzer

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.telephony.SmsMessage
import io.flutter.embedding.engine.FlutterEngineCache
import io.flutter.plugin.common.MethodChannel

class SmsReceiver : BroadcastReceiver() {
    private val CHANNEL = "com.example.ulayzer/sms_receiver"

    override fun onReceive(context: Context, intent: Intent) {
        if (intent.action == "android.provider.Telephony.SMS_RECEIVED") {
            val bundle: Bundle? = intent.extras
            if (bundle != null) {
                val pdus = bundle.get("pdus") as Array<*>?
                if (pdus != null) {
                    for (pdu in pdus) {
                        val sms = SmsMessage.createFromPdu(pdu as ByteArray, bundle.getString("format"))
                        val sender = sms.originatingAddress ?: "Bilinmeyen"
                        val body = sms.messageBody ?: ""
                        val date = sms.timestampMillis
                        val id = sms.indexOnIcc

                        val flutterEngine = FlutterEngineCache.getInstance().get("ulayzer_engine")
                        if (flutterEngine != null) {
                            val channel = MethodChannel(flutterEngine.dartExecutor.binaryMessenger, CHANNEL)
                            channel.invokeMethod("onSmsReceived", mapOf(
                                "id" to id,
                                "address" to sender,
                                "body" to body,
                                "date" to date
                            ))
                        }
                    }
                }
            }
        }
    }
}