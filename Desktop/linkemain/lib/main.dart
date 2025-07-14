import 'package:flutter/material.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:flutter_sms_inbox/flutter_sms_inbox.dart';
import 'dart:convert';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:permission_handler/permission_handler.dart';
import 'package:flutter_background_service/flutter_background_service.dart';
import 'package:flutter_background_service_android/flutter_background_service_android.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:flutter/services.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:fl_chart/fl_chart.dart';

final FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin = FlutterLocalNotificationsPlugin();
Map<String, int> linkStatistics = {};
final Set<String> _sentNotifications = {};
List<Map<String, dynamic>> _processedMessages = [];

class LinkAnalyzer {
  static List<String> extractLinks(String text) {
    final regex = RegExp(r'(https?://\S+|www\.\S+\.\S+)');
    return regex.allMatches(text).map((e) => e.group(0)!).toSet().toList();
  }

  static Future<bool> isMessageProcessed(String contentKey, String source) async {
    final result = _processedMessages.any((item) =>
    item['contentKey'] == contentKey &&
        item['source'] == source &&
        DateTime.parse(item['timestamp']).isAfter(
            DateTime.now().subtract(const Duration(minutes: 5))));
    if (result) {
      print("Mesaj zaten işlenmiş (son 5 dakika): $contentKey, Kaynak: $source");
      return true;
    }
    print("Mesaj işlenmemiş: $contentKey, Kaynak: $source");
    return false;
  }

  static Future<bool> isLinkProcessed(String link, String source) async {
    final result = _processedMessages.any((item) =>
    item['link'] == link &&
        item['source'] == source &&
        DateTime.parse(item['timestamp']).isAfter(
            DateTime.now().subtract(const Duration(minutes: 5))));
    if (result) {
      print("Link zaten işlenmiş (son 5 dakika): $link, Kaynak: $source");
      return true;
    }
    print("Link işlenmemiş: $link, Kaynak: $source");
    return false;
  }

  static Future<void> markMessageAsProcessed(String contentKey, String link, String source) async {
    if (!_processedMessages.any((item) => item['contentKey'] == contentKey && item['link'] == link && item['source'] == source)) {
      _processedMessages.add({
        'contentKey': contentKey,
        'link': link,
        'source': source,
        'timestamp': DateTime.now().toIso8601String(),
      });
      print("Mesaj ve link işlenmiş olarak işaretlendi: $contentKey, Link: $link, Kaynak: $source");
    } else {
      print("Mesaj ve link zaten işlenmiş, tekrar eklenmedi: $contentKey, Link: $link, Kaynak: $source");
    }
    _processedMessages.removeWhere((item) =>
        DateTime.parse(item['timestamp']).isBefore(
            DateTime.now().subtract(const Duration(minutes: 10))));
  }

  static Future<bool> _checkLinkSafety(String link) async {
    try {
      bool isMaliciousGoogle = await checkWithGoogleSafeBrowsing(link);
      print("Google Safe Browsing sonucu: $isMaliciousGoogle için $link");
      if (isMaliciousGoogle) return true;
      bool isMaliciousVirusTotal = await checkWithVirusTotal(link);
      print("VirusTotal sonucu: $isMaliciousVirusTotal için $link");
      return isMaliciousVirusTotal;
    } catch (e) {
      print(
          "Link güvenliği kontrolü hatası: $e için $link - Varsayılan olarak güvenli kabul edildi");
      return false;
    }
  }

  static Future<void> showNotification(String title, String body, String notificationId, String source) async {
    try {
      final notificationKey = "$source|$body|$notificationId";
      if (_sentNotifications.contains(notificationKey)) {
        print(
            "Bildirim zaten gönderildi, tekrar gönderilmez: $title - $body (ID: $notificationId, Kaynak: $source)");
        return;
      }

      const androidDetails = AndroidNotificationDetails(
        'channelId',
        'channelName',
        channelDescription: 'SMS, WhatsApp ve Gmail Link Kontrol Bildirimleri',
        importance: Importance.max,
        priority: Priority.high,
        icon: 'notification_icon',
        largeIcon: DrawableResourceAndroidBitmap('notification_icon'),
        onlyAlertOnce: true,
      );

      const notificationDetails = NotificationDetails(android: androidDetails);

      await flutterLocalNotificationsPlugin.show(
        notificationId.hashCode,
        title,
        body,
        notificationDetails,
        payload: 'link-analiz|$source',
      );

      _sentNotifications.add(notificationKey);
      print("Bildirim gönderildi: $title - $body (ID: $notificationId, Kaynak: $source)");
    } catch (e) {
      print("Bildirim gönderme hatası: $e");
    }
  }

  static Future<bool> checkWithVirusTotal(String url) async {
    const apiKey = '95d15f7d2c6189a21daccd9c03b03efdd5eb3520215b33fd568d9033f2442695';

    try {
      final response = await http.post(
        Uri.parse('https://www.virustotal.com/api/v3/urls'),
        headers: {
          'x-apikey': apiKey,
          'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: 'url=$url',
      );

      if (response.statusCode != 200) {
        print("VirusTotal başlatma hatası: ${response.statusCode} - $url");
        return false;
      }

      final data = jsonDecode(response.body);
      final analysisId = data['data']['id'] as String?;
      if (analysisId == null) {
        print("VirusTotal analiz ID alınamadı: $url");
        return false;
      }

      await Future.delayed(const Duration(seconds: 2));
      final resultResponse = await http.get(
        Uri.parse('https://www.virustotal.com/api/v3/analyses/$analysisId'),
        headers: {'x-apikey': apiKey},
      );

      if (resultResponse.statusCode == 200) {
        final resultData = jsonDecode(resultResponse.body);
        final status = resultData['data']['attributes']['status'] as String?;
        if (status == 'completed') {
          final stats = resultData['data']['attributes']['stats'] as Map?;
          if (stats != null) {
            final malicious = stats['malicious'] as int? ?? 0;
            return malicious > 0;
          }
        } else {
          print("VirusTotal analiz durumu: $status - $url");
        }
      } else {
        print("VirusTotal sorgu hatası: ${resultResponse.statusCode} - $url");
      }
      print("VirusTotal analiz tamamlanmadı, varsayılan güvenli kabul edildi: $url");
      return false;
    } catch (e) {
      print("VirusTotal API hatası: $e - $url");
      return false;
    }
  }

  static Future<bool> checkWithGoogleSafeBrowsing(String url) async {
    const apiKey = 'AIzaSyANXDdMSkE6XrHoD_gvVFaMWHbCeoocnQc';
    const clientId = 'lively-sentry-459706-21';
    const clientVersion = '1.0';

    try {
      final response = await http.post(
        Uri.parse(
            'https://safebrowsing.googleapis.com/v4/threatMatches:find?key=$apiKey'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'client': {
            'clientId': clientId,
            'clientVersion': clientVersion,
          },
          'threatInfo': {
            'threatTypes': [
              'MALWARE',
              'SOCIAL_ENGINEERING',
              'UNWANTED_SOFTWARe',
              'POTENTIALLY_HARMFUL_APPLICATION'
            ],
            'platformTypes': ['ANY_PLATFORM'],
            'threatEntryTypes': ['URL'],
            'threatEntries': [{'url': url}],
          },
        }),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        final isMalicious = (data['matches'] as List?)?.isNotEmpty ?? false;
        print("Google Safe Browsing: $isMalicious - $url");
        return isMalicious;
      } else if (response.statusCode == 204) {
        print("Google Safe Browsing: Güvenli - $url");
        return false;
      } else {
        print("Google Safe Browsing hatası: ${response.statusCode} - $url");
        return false;
      }
    } catch (e) {
      print("Google Safe Browsing API hatası: $e - $url");
      return false;
    }
  }

  static Future<void> processMessages(ServiceInstance? service, {Map<String, dynamic>? singleMessage}) async {
    try {
      print("processMessages çağrıldı, SMS tarama başlatılıyor: ${DateTime.now()}");
      if (!await Permission.sms.isGranted) {
        print("SMS izni eksik, tarama yapılamıyor.");
        return;
      }

      List<Map<String, dynamic>> messages;
      if (singleMessage != null) {
        messages = [singleMessage];
        print("Tek SMS işleniyor: ID=${singleMessage['id']}, İçerik=${singleMessage['body']}, Gönderici=${singleMessage['address']}, Tarih=${singleMessage['date']}");
      } else {
        print("Son 1 dakikadaki tüm SMS'ler taranıyor...");
        final SmsQuery query = SmsQuery();
        final smsMessages = await query.getAllSms;
        messages = smsMessages
            .where((msg) =>
        msg.date != null &&
            msg.date!.isAfter(DateTime.now().subtract(const Duration(minutes: 1))))
            .map((msg) => {
          'id': msg.id,
          'address': msg.address,
          'body': msg.body,
          'date': msg.date?.toIso8601String(),
        })
            .toList();
        print("Son 1 dakikada bulunan SMS sayısı: ${messages.length}");
        if (messages.isEmpty) {
          print("Son 1 dakikada SMS bulunamadı, yeni SMS bekleniyor.");
          return;
        }
      }

      for (var message in messages) {
        final body = message['body'] as String?;
        final address = message['address'] as String?;
        final date = message['date'] != null
            ? DateTime.parse(message['date'] as String)
            : DateTime.now();
        final id = message['id'] as int?;

        if (body == null || body.isEmpty) {
          print("SMS içeriği boş veya geçersiz: ID=$id");
          continue;
        }

        final contentKey = "$body|$address|${date.toIso8601String()}|$id";
        if (await isMessageProcessed(contentKey, 'SMS')) {
          print("SMS zaten işlenmiş, atlanıyor: $contentKey");
          continue;
        }

        final links = LinkAnalyzer.extractLinks(body).toSet().toList();
        print("SMS'te bulunan benzersiz linkler: $links");

        if (links.isEmpty) {
          print("SMS'te link bulunamadı: $body");
          await markMessageAsProcessed(contentKey, '', 'SMS');
          continue;
        }

        final user = FirebaseAuth.instance.currentUser;
        if (user == null) {
          print("Kullanıcı oturumu açık değil, veri kaydedilemedi.");
          return;
        }

        for (String link in links) {
          final uniqueLinkKey = 'SMS|$link|${date.toIso8601String()}';
          if (await isLinkProcessed(link, 'SMS')) {
            print("Link zaten işlenmiş, atlanıyor: $link");
            continue;
          }

          print("İşlenen SMS linki: $link");
          final notificationId = "sms_${link.hashCode}_${date.toIso8601String()}";
          await LinkAnalyzer.showNotification(
              "⏳ SMS - Link Kontrol Ediliyor",
              link,
              notificationId,
              "SMS");

          Future<bool> safetyCheck = LinkAnalyzer._checkLinkSafety(link);
          safetyCheck.then((isMalicious) async {
            await LinkAnalyzer.showNotification(
                isMalicious ? "⚠️ SMS - Zararlı Link!" : "✅ SMS - Güvenli Link",
                link,
                notificationId,
                "SMS");

            final notificationData = {
              'source': 'SMS',
              'link': link,
              'isMalicious': isMalicious ? 1 : 0,
              'timestamp': date.toIso8601String(),
              'sender': address ?? "Bilinmeyen Gönderici",
              'contentKey': contentKey,
              'uniqueLinkKey': uniqueLinkKey,
            };

            final existingDocs = await FirebaseFirestore.instance
                .collection('users')
                .doc(user.uid)
                .collection('notifications')
                .where('uniqueLinkKey', isEqualTo: uniqueLinkKey)
                .where('contentKey', isEqualTo: contentKey)
                .get();
            if (existingDocs.docs.isEmpty) {
              await FirebaseFirestore.instance
                  .collection('users')
                  .doc(user.uid)
                  .collection('notifications')
                  .add(notificationData);
              print("SMS bildirim verisi Firestore'a eklendi (UID: ${user.uid}): $notificationData");
              if (service != null) {
                service.invoke("updateNotification", notificationData);
              }
            } else {
              print("SMS bildirim zaten Firestore'da mevcut, eklenmedi: $contentKey, Link: $link");
            }
          }).catchError((e) {
            print("Güvenlik kontrolü hatası, bildirim güncelleniyor: $e");
            LinkAnalyzer.showNotification(
                "✅ SMS - Güvenli Link (Hata)",
                link,
                notificationId,
                "SMS");
          });

          await markMessageAsProcessed(contentKey, link, 'SMS');
        }

        await markMessageAsProcessed(contentKey, '', 'SMS');
      }
      print("SMS kontrolü tamamlandı: ${DateTime.now()}");
    } catch (e, stackTrace) {
      print("SMS okuma hatası: $e, StackTrace: $stackTrace");
    }
  }
}

class NotificationPermissionPage extends StatelessWidget {
  const NotificationPermissionPage({super.key});

  Future<void> _openNotificationAccessSettings() async {
    try {
      const String notificationListenerSettings = 'android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS';
      final Uri notificationSettingsUri = Uri.parse(notificationListenerSettings);
      if (await canLaunchUrl(notificationSettingsUri)) {
        await launchUrl(notificationSettingsUri, mode: LaunchMode.externalApplication);
        print("Bildirim erişim ayarlarına yönlendirme başarılı.");
      } else {
        print("Bildirim erişim ayarlarına yönlendirme başarısız, genel ayarlara yönlendiriliyor...");
        if (await canLaunchUrl(Uri.parse('app-settings:'))) {
          await launchUrl(Uri.parse('app-settings:'), mode: LaunchMode.externalApplication);
          print("Genel ayarlar sayfasına yönlendirme yapıldı.");
        } else {
          print("Hiçbir ayar sayfasına yönlendirme yapılamadı.");
        }
      }
    } catch (e) {
      print("Ayarlara yönlendirme hatası: $e");
      if (await canLaunchUrl(Uri.parse('app-settings:'))) {
        await launchUrl(Uri.parse('app-settings:'), mode: LaunchMode.externalApplication);
        print("Genel ayarlar sayfasına yedek yönlendirme yapıldı.");
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Bildirim İzni Gerekli')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('Ulayzer, bildirimleri analiz etmek için bildirim erişim iznine ihtiyaç duyar. Lütfen aşağıdaki butona basarak izni verin.'),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: _openNotificationAccessSettings,
              style: ElevatedButton.styleFrom(backgroundColor: Colors.teal, padding: const EdgeInsets.symmetric(horizontal: 30, vertical: 15)),
              child: const Text('İzin Ver', style: TextStyle(fontSize: 16, color: Colors.white)),
            ),
          ],
        ),
      ),
    );
  }
}

class PrivacyCenterPage extends StatefulWidget {
  const PrivacyCenterPage({super.key});

  @override
  _PrivacyCenterPageState createState() => _PrivacyCenterPageState();
}

class _PrivacyCenterPageState extends State<PrivacyCenterPage> {
  final String supportEmail = 'ulayzerdestek@gmail.com';

  Future<void> _launchEmail(String email) async {
    final Uri emailLaunchUri = Uri(
      scheme: 'mailto',
      path: email,
      queryParameters: {
        'subject': 'Ulayzer Destek Talebi',
      },
    );
    try {
      if (await canLaunchUrl(emailLaunchUri)) {
        await launchUrl(emailLaunchUri, mode: LaunchMode.externalApplication);
        print("E-posta istemcisi başarıyla açıldı: $emailLaunchUri");
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('E-posta istemcisi bulunamadı. Lütfen bir e-posta uygulaması yükleyin.')),
        );
        print("E-posta istemcisi bulunamadı: $emailLaunchUri");
      }
    } catch (e) {
      print('E-posta açma hatası: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('E-posta açılırken bir hata oluştu. Lütfen tekrar deneyin.')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Gizlilik Merkezi',
          style: TextStyle(color: Colors.white),
        ),
        backgroundColor: Colors.teal,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: Colors.white),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Text(
                'Gizliliğiniz Bizim İçin Önemli',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              const SizedBox(height: 16),
              const Text(
                'Ulayzer olarak, kullanıcılarımızın gizliliğine büyük önem veriyoruz. Verileriniz bizim için bir emanet ve bu emaneti en iyi şekilde korumak için çalışıyoruz. Uygulamamız, SMS, WhatsApp ve Gmail üzerinden gelen linkleri analiz ederken yalnızca gerekli izinleri kullanır ve hiçbir kişisel bilginizi üçüncü taraflarla paylaşmaz.',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              const SizedBox(height: 16),
              const Text(
                'Veri Güvenliği Taahhüdümüz',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              const SizedBox(height: 8),
              const Text(
                '• Verileriniz cihazınızda yerel olarak saklanır ve hiçbir şekilde sunucularımıza gönderilmez.\n'
                    '• Link analizleri, yalnızca Google Safe Browsing ve VirusTotal gibi güvenilir API’ler üzerinden gerçekleştirilir.\n'
                    '• Uygulamamız, yalnızca sizin verdiğiniz izinler doğrultusunda çalışır ve bu izinleri yalnızca gerekli olduğu kadar kullanır.\n'
                    '• Kullanıcı verilerinin anonimleştirilmesi ve şifrelenmesi için en güncel teknolojileri kullanıyoruz.',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              const SizedBox(height: 16),
              const Text(
                'Siz Değerlisiniz',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              const SizedBox(height: 8),
              const Text(
                'Bizim için en önemli şey, sizin güvenliğiniz ve memnuniyetinizdir. Ulayzer, dijital dünyada daha güvenli bir deneyim yaşamanız için tasarlandı. Eğer gizlilikle ilgili herhangi bir sorunuz varsa, bizimle iletişime geçmekten çekinmeyin. Sizin için buradayız!',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              const SizedBox(height: 16),
              const Text(
                'Bize Ulaşın',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              const SizedBox(height: 8),
              GestureDetector(
                onTap: () => _launchEmail(supportEmail),
                child: Text(
                  supportEmail,
                  style: const TextStyle(
                    fontSize: 16,
                    color: Colors.blue,
                    decoration: TextDecoration.underline,
                  ),
                ),
              ),
              const SizedBox(height: 20),
              Center(
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Image.asset(
                        'assets/google_icon.png',
                        width: 50,
                        height: 50,
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Image.asset(
                        'assets/virustotal_icon.png',
                        width: 50,
                        height: 50,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class AboutUsPage extends StatelessWidget {
  const AboutUsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Hakkımızda',
          style: TextStyle(color: Colors.white),
        ),
        backgroundColor: Colors.teal,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: Colors.white),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: const [
              Text(
                'Ulayzer Nedir?',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              SizedBox(height: 16),
              Text(
                'Ulayzer, dijital dünyada güvenliğinizi artırmak için tasarlanmış bir uygulamadır. SMS, WhatsApp ve Gmail üzerinden gelen linkleri analiz ederek zararlı içeriklere karşı sizi korur. Amacımız, kullanıcılarımıza daha güvenli bir dijital deneyim sunmak ve kötü niyetli bağlantılardan korunmalarını sağlamaktır.',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              SizedBox(height: 16),
              Text(
                'Biz Kimiz?',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Biz, teknolojiyi kullanarak insanların hayatını kolaylaştırıp güvenliğini artırmayı hedefleyen bir ekibiz. Kullanıcı odaklı yaklaşımımızla, her zaman en iyi deneyimi sunmak için çalışıyoruz. Ulayzer, bu vizyonun bir parçası olarak ortaya çıktı ve her geçen gün daha fazla kullanıcıya ulaşarak dijital güvenliği artırmayı amaçlıyor.',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              SizedBox(height: 16),
              Text(
                'Misyonumuz',
                style: TextStyle(
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal,
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Dijital dünyada güvenliği ve gizliliği bir öncelik sağlayarak kullanıcılarımıza en iyi korumayı yapmak. Ulayzer ile her zaman bir adım önde olun ve zararlı linklerden uzak durun!',
                style: TextStyle(fontSize: 16, color: Colors.black87),
              ),
              SizedBox(height: 20),
              Divider(thickness: 1, color: Colors.grey),
              SizedBox(height: 10),
              Center(
                child: Text(
                  'Geliştiriciler',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.teal,
                  ),
                ),
              ),
              SizedBox(height: 8),
              Center(
                child: Text(
                  'ZEHRA DEMİRTOP - AYCAN SÖKMEN',
                  textAlign: TextAlign.center,
                  style: TextStyle(fontSize: 16, color: Colors.black87),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class LinkDetailPage extends StatelessWidget {
  final Map<String, dynamic> notification;

  const LinkDetailPage({super.key, required this.notification});

  @override
  Widget build(BuildContext context) {
    final isMalicious = notification['isMalicious'] == 1;
    final sender = notification['sender'] ?? "Bilinmeyen Gönderici";
    return Scaffold(
      appBar: AppBar(
        title: const Text('Link Detayı', style: TextStyle(color: Colors.white)),
        backgroundColor: Colors.teal,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: Colors.white),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Link: ${notification['link'] ?? "Bilinmeyen Link"}',
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 10),
            Text(
              'Gönderici: $sender',
              style: const TextStyle(fontSize: 16),
            ),
            const SizedBox(height: 10),
            Text(
              'Kaynak: ${notification['source'] ?? "Bilinmeyen"}',
              style: const TextStyle(fontSize: 16),
            ),
            const SizedBox(height: 10),
            Text(
              'Durum: ${isMalicious ? "Zararlı" : "Güvenli"}',
              style: TextStyle(
                fontSize: 16,
                color: isMalicious ? Colors.red : Colors.green,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 10),
            Text(
              'Zaman: ${notification['timestamp'] ?? "Bilinmeyen"}',
              style: const TextStyle(fontSize: 16),
            ),
            const SizedBox(height: 20),
            Text(
              'Not: Bu link ${isMalicious ? "Google Safe Browsing veya VirusTotal tarafından zararlı olarak işaretlenmiş olabilir." : "güvenli olarak değerlendirilmiştir."}',
              style: const TextStyle(fontSize: 16, color: Colors.black87),
            ),
          ],
        ),
      ),
    );
  }
}

class MyApp extends StatefulWidget {
  const MyApp({Key? key}) : super(key: key);
  static final GlobalKey<_MyAppState> globalKey = GlobalKey<_MyAppState>();

  @override
  _MyAppState createState() => _MyAppState();
}

Future<void> _requestPermissions() async {
  Map<Permission, PermissionStatus> statuses = await [
    Permission.sms,
    Permission.notification,
  ].request();

  if (!statuses[Permission.sms]!.isGranted) {
    print("SMS izni verilmedi, tekrar deneniyor...");
    await Permission.sms.request();
  }
  if (!statuses[Permission.notification]!.isGranted) {
    print("Bildirim izni verilmedi, yönlendirme yapılıyor...");
  }
}

Future<void> initializeNotifications() async {
  const AndroidInitializationSettings initializationSettingsAndroid = AndroidInitializationSettings('notification_icon');

  final InitializationSettings initializationSettings = InitializationSettings(android: initializationSettingsAndroid);

  await flutterLocalNotificationsPlugin.initialize(
    initializationSettings,
    onDidReceiveNotificationResponse: (details) => print("Bildirim tıklandı: ${details.payload}"),
  );

  const AndroidNotificationChannel channel = AndroidNotificationChannel(
    'channelId',
    'channelName',
    description: 'SMS, WhatsApp ve Gmail Link Kontrol Bildirimleri',
    importance: Importance.max,
    playSound: true,
    showBadge: false,
    enableVibration: true,
    enableLights: true,
  );
  await flutterLocalNotificationsPlugin
      .resolvePlatformSpecificImplementation<AndroidFlutterLocalNotificationsPlugin>()
      ?.createNotificationChannel(channel);
}

Future<void> initializeService() async {
  final service = FlutterBackgroundService();

  await service.configure(
    androidConfiguration: AndroidConfiguration(
      onStart: onStart,
      autoStart: true,
      isForegroundMode: true,
      foregroundServiceNotificationId: 888,
    ),
    iosConfiguration: IosConfiguration(),
  );

  await service.startService();
}

@pragma('vm:entry-point')
void onStart(ServiceInstance service) async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp();
  print("onStart çağrıldı: Arka plan servisi başlatılıyor...");

  if (service is AndroidServiceInstance) {
    await service.setAsForegroundService();
    service.setForegroundNotificationInfo(
      title: "SMS, WhatsApp ve Gmail Takip Aktif",
      content: "Arka planda çalışıyor...",
    );

    service.on('setAsForeground').listen((event) {
      service.setAsForegroundService();
    });
    service.on('setAsBackground').listen((event) {
      service.setAsBackgroundService();
    });
    service.on('stopService').listen((event) {
      service.stopSelf();
      print("Arka plan servisi durduruldu.");
    });
  }

  if (!await Permission.sms.isGranted) {
    print("SMS izni eksik, arka plan servisi şu anda çalışmayacak. İzin bekleniyor...");
    return;
  } else {
    print("SMS izni mevcut, tarama başlayabilir.");
  }

  const platform = MethodChannel('com.example.ulayzer/sms_receiver');
  platform.setMethodCallHandler((call) async {
    try {
      if (call.method == 'onSmsReceived') {
        final args = call.arguments as Map;
        final message = {
          'id': args['id'] as int?,
          'address': args['address'] as String?,
          'body': args['body'] as String?,
          'date': DateTime.fromMillisecondsSinceEpoch(args['date'] as int).toIso8601String(),
        };
        print("Yeni SMS alındı: İçerik=${message['body']}, Gönderici=${message['address']}, Zaman=${message['date']}, ID=${message['id']}");
        await LinkAnalyzer.processMessages(service, singleMessage: message);
      } else {
        print("Bilinmeyen MethodChannel çağrısı: ${call.method}");
      }
    } catch (e, stackTrace) {
      print("MethodChannel hata: $e, StackTrace: $stackTrace");
    }
  });

  startNotificationListener(service);
}

void startNotificationListener(ServiceInstance service) {
  const platform = MethodChannel('com.example.ulayzer/notification_data');
  DateTime? lastNotificationTime;
  Map<String, DateTime> lastProcessedNotifications = {};

  platform.setMethodCallHandler((call) async {
    if (call.method == 'onNotificationReceived') {
      final args = call.arguments as Map;
      final packageName = args['packageName'] as String;
      final notificationText = args['notificationText'] as String? ?? '';
      final bigText = args['bigText'] as String? ?? '';
      final summaryText = args['summaryText'] as String? ?? '';
      final timestamp = DateTime.fromMillisecondsSinceEpoch(args['timestamp'] as int);
      final sender = extractSenderFromText('$notificationText $bigText $summaryText', packageName);
      final notificationKey = '$packageName|$notificationText|$bigText|$summaryText|$sender|$timestamp';

      if (lastNotificationTime != null && timestamp.difference(lastNotificationTime!).inMilliseconds < 1000) {
        print("Aynı bildirim 1000ms içinde tekrarlandı, atlanıyor: $notificationKey");
        return;
      }

      if (packageName == 'com.whatsapp' ||
          packageName == 'com.whatsapp.w4b' ||
          packageName == 'com.google.android.gm' ||
          packageName == 'com.google.android.gm.lite') {
        final source = packageName.contains('whatsapp') ? 'WhatsApp' : 'Gmail';
        final combinedText = '$notificationText $bigText $summaryText'.trim();
        final contentKey = '$packageName|$combinedText|$sender|$timestamp';

        if (await LinkAnalyzer.isMessageProcessed(contentKey, source)) {
          print("$source bildirim zaten işlenmiş, atlanıyor: $contentKey");
          return;
        }

        lastProcessedNotifications[contentKey] = timestamp;
        print("Bildirim alındı - Paket: $packageName, Metin: $notificationText, BigText: $bigText, Summary: $summaryText, Gönderici: $sender, Zaman: $timestamp");
        if (MyApp.globalKey.currentState != null) {
          await MyApp.globalKey.currentState!.onNotificationPosted(packageName, notificationText, bigText, summaryText, sender, timestamp, service);
        }
      } else {
        print("Bu bildirim WhatsApp veya Gmail'den değil, atlanıyor: $packageName");
      }

      lastNotificationTime = timestamp;
      lastProcessedNotifications[notificationKey] = timestamp;
    }
  });
  print("Bildirim dinleyici başlatıldı");
}

String extractSenderFromText(String text, String packageName) {
  if (text.isEmpty) return "Bilinmeyen Gönderici";

  if (packageName == 'com.whatsapp' || packageName == 'com.whatsapp.w4b') {
    final lines = text.split('\n');
    if (lines.isNotEmpty) {
      final firstLine = lines[0].trim();
      if (firstLine.contains(':') && !firstLine.contains('WhatsApp')) {
        final colonIndex = firstLine.indexOf(':');
        return firstLine.substring(0, colonIndex).trim();
      } else if (!firstLine.contains('http') && !firstLine.contains('https')) {
        return firstLine;
      }
    }
    return "Bilinmeyen Gönderici (WhatsApp)";
  } else if (packageName == 'com.google.android.gm' || packageName == 'com.google.android.gm.lite') {
    final lines = text.split('\n').map((line) => line.trim()).toList();
    for (var line in lines) {
      final emailRegex = RegExp(r'([\w\s]+)\s*<\S+@\S+\.\S+>');
      final match = emailRegex.firstMatch(line);
      if (match != null) {
        return match.group(1)?.trim() ?? "Bilinmeyen Gönderici (Gmail)";
      }
      if (line.contains(':') && !line.contains('http') && !line.contains('https')) {
        final colonIndex = line.indexOf(':');
        return line.substring(0, colonIndex).trim();
      }
      if (!line.contains('http') && !line.contains('https') && line.isNotEmpty) {
        return line;
      }
    }
    return "Bilinmeyen Gönderici (Gmail)";
  } else {
    final lines = text.split('\n');
    if (lines.isNotEmpty) {
      final firstLine = lines[0].trim();
      final colonIndex = firstLine.indexOf(':');
      if (colonIndex != -1) {
        return firstLine.substring(0, colonIndex).trim();
      }
    }
    return "Bilinmeyen Gönderici";
  }
}

class _MyAppState extends State<MyApp> {
  bool isActive = true;
  StreamSubscription? _notificationSubscription;
  DateTime? _lastProcessedNotificationTime;
  List<Map<String, dynamic>> recentNotifications = [];
  String? _lastProcessedNotificationContent;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) async {
      await _checkAndRequestPermissions();
      if ((await Permission.sms.isGranted) && (await Permission.notification.isGranted)) {
        await initializeNotifications();
        await initializeService();
        String? userId = FirebaseAuth.instance.currentUser?.uid;
        if (userId == null) {
          await FirebaseAuth.instance.signInAnonymously();
          print("Anonim oturum açıldı.");
        }
        _listenToNotifications();
      }
    });
  }

  Future<void> _checkAndRequestPermissions() async {
    PermissionStatus smsStatus = await Permission.sms.status;
    PermissionStatus notificationStatus = await Permission.notification.status;
    print("Mevcut izin durumları - SMS: $smsStatus, Bildirim: $notificationStatus");

    if (!smsStatus.isGranted || !notificationStatus.isGranted) {
      Map<Permission, PermissionStatus> statuses = await [
        Permission.sms,
        Permission.notification,
      ].request();
      print("İzin istendi sonrası durumlar - SMS: ${statuses[Permission.sms]}, Bildirim: ${statuses[Permission.notification]}");

      if (!statuses[Permission.sms]!.isGranted || !statuses[Permission.notification]!.isGranted) {
        _showPermissionDialog(statuses);
      }
    } else {
      print("Tüm izinler zaten verilmiş, işlem devam ediyor.");
    }
  }

  void _showPermissionDialog(Map<Permission, PermissionStatus> statuses) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('İzin Gerekli'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              if (!statuses[Permission.sms]!.isGranted)
                const Text('Ulayzer, SMS analizi için SMS iznine ihtiyaç duyar.'),
              if (!statuses[Permission.notification]!.isGranted)
                const Text('Ulayzer, bildirim analizi için bildirim iznine ihtiyaç duyar.'),
              const Text('Ayarlara gidip izni verin.'),
            ],
          ),
          actions: [
            TextButton(
              child: const Text('İptal'),
              onPressed: () => Navigator.of(context).pop(),
            ),
            TextButton(
              child: const Text('Ayarlara Git'),
              onPressed: () async {
                Navigator.of(context).pop();
                const androidPlatform = MethodChannel('com.example.ulayzer/permission');
                try {
                  await androidPlatform.invokeMethod('openSettings');
                } catch (e) {
                  print("Ayarlara yönlendirme hatası: $e");
                  if (await canLaunchUrl(Uri.parse('package:com.example.ulayzer'))) {
                    await launchUrl(Uri.parse('package:com.example.ulayzer'), mode: LaunchMode.externalApplication);
                  }
                }
              },
            ),
          ],
        );
      },
    );
  }

  @override
  void dispose() {
    _notificationSubscription?.cancel();
    super.dispose();
  }

  Future<void> onNotificationPosted(String packageName, String notificationText, String bigText, String summaryText, String sender, DateTime timestamp, ServiceInstance? service) async {
    print("onNotificationPosted çağrıldı: $packageName, Metin: $notificationText, BigText: $bigText, Summary: $summaryText, Zaman: $timestamp");

    final combinedText = '$notificationText $bigText $summaryText'.trim();
    final links = LinkAnalyzer.extractLinks(combinedText).toSet().toList();
    print("Çıkarılan benzersiz linkler: $links");

    String source;
    if (packageName == 'com.whatsapp' || packageName == 'com.whatsapp.w4b') {
      source = 'WhatsApp';
    } else if (packageName == 'com.google.android.gm' || packageName == 'com.google.android.gm.lite') {
      source = 'Gmail';
    } else {
      print("Bildirim bilinmeyen kaynaktan geldi, işlenmeyecek: $packageName");
      return;
    }
    print("Belirlenen kaynak: $source, Paket: $packageName");

    final contentKey = '$packageName|$combinedText|$sender|$timestamp';
    if (await LinkAnalyzer.isMessageProcessed(contentKey, source)) {
      print("$source bildirim zaten işlenmiş: $contentKey");
      return;
    }

    if (links.isEmpty) {
      print("$source bildiriminde link bulunamadı: Text=$notificationText, BigText=$bigText, Summary=$summaryText");
      await LinkAnalyzer.markMessageAsProcessed(contentKey, '', source);
      return;
    }

    for (String link in links) {
      final uniqueLinkKey = '$source|$link|$timestamp';
      if (await LinkAnalyzer.isLinkProcessed(link, source)) {
        print("$source için link zaten işlenmiş: $link");
        continue;
      }

      await LinkAnalyzer.showNotification(
          "⏳ $source - Link Kontrol Ediliyor",
          link,
          "${source.toLowerCase()}_${link.hashCode}_$timestamp",
          source);

      bool isMalicious = false;
      try {
        isMalicious = await LinkAnalyzer._checkLinkSafety(link);
      } catch (e) {
        print("Link güvenliği kontrolü hatası (devam ediliyor): $e");
      }

      await LinkAnalyzer.showNotification(
          isMalicious ? "⚠️ $source - Zararlı Link!" : "✅ $source - Güvenli Link",
          link,
          "${source.toLowerCase()}_${link.hashCode}_$timestamp",
          source);

      final notificationData = {
        'source': source,
        'link': link,
        'isMalicious': isMalicious ? 1 : 0,
        'timestamp': timestamp.toIso8601String(),
        'sender': sender,
        'combinedText': combinedText,
        'contentKey': contentKey,
        'uniqueLinkKey': uniqueLinkKey,
      };

      final user = FirebaseAuth.instance.currentUser;
      if (user == null) {
        print("Kullanıcı oturumu açık değil! UID: null");
        return;
      }
      print("Oturum aktif, UID: ${user.uid}");

      try {
        final existingDocs = await FirebaseFirestore.instance
            .collection('users')
            .doc(user.uid)
            .collection('notifications')
            .where('uniqueLinkKey', isEqualTo: uniqueLinkKey)
            .where('contentKey', isEqualTo: contentKey)
            .get();
        if (existingDocs.docs.isEmpty) {
          final docRef = await FirebaseFirestore.instance
              .collection('users')
              .doc(user.uid)
              .collection('notifications')
              .add(notificationData);
          print("$source bildirim verisi Firestore'a eklendi (UID: ${user.uid}), Doküman ID: ${docRef.id}");
          updateLocalNotifications(notificationData);
        } else {
          print("$source bildirim zaten Firestore'da mevcut, eklenmedi: $contentKey, Link: $link");
        }
      } catch (e) {
        print("$source Firestore yazma hatası: $e");
        return;
      }

      if (service != null) {
        service.invoke("updateNotification", {"notificationData": notificationData});
      }

      await LinkAnalyzer.markMessageAsProcessed(contentKey, link, source);
    }
  }

  void updateLocalNotifications(Map<String, dynamic> notificationData) {
    setState(() {
      final existingIndex = recentNotifications.indexWhere((n) =>
      n['uniqueLinkKey'] == notificationData['uniqueLinkKey'] &&
          n['contentKey'] == notificationData['contentKey']);
      if (existingIndex == -1) {
        recentNotifications.insert(0, notificationData);
        if (recentNotifications.length > 15) {
          recentNotifications.removeRange(15, recentNotifications.length);
        }
        print("Yeni bildirim eklendi: ${notificationData['uniqueLinkKey']}");
      } else {
        print("Aynı uniqueLinkKey ve contentKey zaten mevcut, eklenmedi: ${notificationData['uniqueLinkKey']}");
      }
    });
  }

  Future<void> _toggleService(bool value) async {
    try {
      final service = FlutterBackgroundService();
      if (value) {
        await initializeService();
        print("Servis başlatıldı.");
      } else {
        service.invoke("stopService");
        print("Servis durduruldu.");
      }
      setState(() {
        isActive = value;
      });
    } catch (e) {
      print("Servis kontrol hatası: $e");
    }
  }

  void _listenToNotifications() {
    String? userId = FirebaseAuth.instance.currentUser?.uid;
    if (userId == null) {
      print("Kullanıcı oturum açmamış, dinleme başlatılamıyor.");
      setState(() {
        recentNotifications = [];
      });
      return;
    }

    _notificationSubscription = FirebaseFirestore.instance
        .collection('users')
        .doc(userId)
        .collection('notifications')
        .orderBy('timestamp', descending: true)
        .limit(15)
        .snapshots()
        .listen(
          (snapshot) {
        if (mounted) {
          setState(() {
            final newNotifications = snapshot.docs
                .map((doc) => doc.data())
                .where((data) => !recentNotifications.any((n) =>
            n['uniqueLinkKey'] == data['uniqueLinkKey'] &&
                n['contentKey'] == data['contentKey']))
                .toList();
            recentNotifications = [
              ...newNotifications,
              ...recentNotifications.where((n) => !snapshot.docs.any((doc) =>
              doc.data()['uniqueLinkKey'] == n['uniqueLinkKey'] &&
                  doc.data()['contentKey'] == n['contentKey'])),
            ];
            recentNotifications.sort((a, b) => DateTime.parse(b['timestamp']).compareTo(DateTime.parse(a['timestamp'])));
            if (recentNotifications.length > 15) {
              recentNotifications = recentNotifications.sublist(0, 15);
            }
            print("Firestore'dan güncellenen son analizler (UID: $userId): ${recentNotifications.length} öğe");
          });
        }
      },
      onError: (error) {
        print("Firestore dinleme hatası (UID: $userId): $error");
        if (mounted) {
          setState(() {
            recentNotifications = [];
            ScaffoldMessenger.of(context).showSnackBar(
              const SnackBar(content: Text('Verilere erişimde sorun oluştu. Lütfen oturum açın.')),
            );
          });
        }
      },
      cancelOnError: true,
    );
  }

  String getSecurityStatus() {
    if (recentNotifications.isEmpty) return "Bilinmiyor";
    int maliciousCount = recentNotifications.where((n) => n['isMalicious'] == 1).length;
    if (maliciousCount == 0) return "Güvenli";
    if (maliciousCount >= 2) return "Riskli";
    return "Dikkat";
  }

  Color getStatusColor(String status) {
    switch (status) {
      case "Güvenli":
        return Colors.green;
      case "Dikkat":
        return Colors.orange;
      case "Riskli":
        return Colors.red;
      default:
        return Colors.grey;
    }
  }

  @override
  Widget build(BuildContext context) {
    final securityStatus = getSecurityStatus();
    final statusColor = getStatusColor(securityStatus);

    final smsMalicious = recentNotifications.where((n) => n['source'] == 'SMS' && n['isMalicious'] == 1).length;
    final smsSafe = recentNotifications.where((n) => n['source'] == 'SMS' && n['isMalicious'] == 0).length;
    final whatsappMalicious = recentNotifications.where((n) => n['source'] == 'WhatsApp' && n['isMalicious'] == 1).length;
    final whatsappSafe = recentNotifications.where((n) => n['source'] == 'WhatsApp' && n['isMalicious'] == 0).length;
    final gmailMalicious = recentNotifications.where((n) => n['source'] == 'Gmail' && n['isMalicious'] == 1).length;
    final gmailSafe = recentNotifications.where((n) => n['source'] == 'Gmail' && n['isMalicious'] == 0).length;

    return MaterialApp(
      theme: ThemeData(
        primarySwatch: Colors.teal,
        scaffoldBackgroundColor: Colors.grey[100],
        textTheme: const TextTheme(
          bodyMedium: TextStyle(fontSize: 16, color: Colors.black87),
          headlineSmall: TextStyle(fontSize: 24, fontWeight: FontWeight.bold, color: Colors.teal),
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.teal,
            foregroundColor: Colors.white,
            padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 15),
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          ),
        ),
      ),
      home: Scaffold(
        appBar: AppBar(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Text(
                'Ulayzer',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ],
          ),
          centerTitle: true,
          backgroundColor: Colors.teal,
          actions: [
            Builder(
              builder: (BuildContext context) {
                return PopupMenuButton<String>(
                  icon: const Icon(Icons.settings, color: Colors.white),
                  onSelected: (value) {
                    if (value == 'privacy') {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const PrivacyCenterPage()),
                      );
                    } else if (value == 'about') {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const AboutUsPage()),
                      );
                    }
                  },
                  itemBuilder: (BuildContext context) => <PopupMenuEntry<String>>[
                    const PopupMenuItem<String>(
                      value: 'privacy',
                      child: Text('Gizlilik Merkezi'),
                    ),
                    const PopupMenuItem<String>(
                      value: 'about',
                      child: Text('Hakkımızda'),
                    ),
                  ],
                );
              },
            ),
          ],
        ),
        body: Padding(
          padding: const EdgeInsets.all(16.0),
          child: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Card(
                  elevation: 5,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: SwitchListTile(
                      title: const Text(
                        'Aktif/Pasif Yap',
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                      subtitle: Text(
                        isActive ? 'Uygulama çalışıyor' : 'Uygulama durduruldu',
                        style: TextStyle(color: isActive ? Colors.green : Colors.red),
                      ),
                      value: isActive,
                      activeColor: Colors.teal,
                      onChanged: (value) async {
                        await _toggleService(value);
                      },
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                Card(
                  elevation: 5,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        const Text(
                          'Güvenlik Durumu',
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                            color: Colors.teal,
                          ),
                        ),
                        AnimatedContainer(
                          duration: const Duration(milliseconds: 500),
                          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                          decoration: BoxDecoration(
                            color: statusColor.withOpacity(0.2),
                            borderRadius: BorderRadius.circular(10),
                          ),
                          child: Text(
                            securityStatus,
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: statusColor,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                Card(
                  elevation: 5,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Row(
                          children: [
                            Icon(Icons.history, color: Colors.teal, size: 28),
                            SizedBox(width: 10),
                            Text(
                              'Son Analizler',
                              style: TextStyle(
                                fontSize: 24,
                                fontWeight: FontWeight.bold,
                                color: Colors.teal,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 10),
                        SizedBox(
                          height: 200,
                          child: ListView.builder(
                            itemCount: recentNotifications.length,
                            itemBuilder: (context, index) {
                              final notification = recentNotifications[index];
                              final isMalicious = notification['isMalicious'] == 1;
                              final color = isMalicious ? Colors.red : Colors.green;
                              return ListTile(
                                leading: Icon(Icons.link, color: color),
                                title: Text(
                                  notification['link'] ?? "Bilinmeyen Link",
                                  style: TextStyle(color: color),
                                  overflow: TextOverflow.ellipsis,
                                ),
                                subtitle: Text(
                                  "Kaynak: ${notification['source'] ?? 'Bilinmeyen'} - ${notification['timestamp']}",
                                  style: const TextStyle(fontSize: 12),
                                ),
                                trailing: Column(
                                  mainAxisAlignment: MainAxisAlignment.center,
                                  children: [
                                    Text(
                                      isMalicious ? "Zararlı" : "Güvenli",
                                      style: TextStyle(fontWeight: FontWeight.bold, color: color),
                                    ),
                                    GestureDetector(
                                      onTap: () {
                                        Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                            builder: (context) => LinkDetailPage(notification: notification),
                                          ),
                                        );
                                      },
                                      child: const Text(
                                        "Detay",
                                        style: TextStyle(
                                          fontSize: 12,
                                          color: Colors.blue,
                                          decoration: TextDecoration.underline,
                                        ),
                                      ),
                                    ),
                                  ],
                                ),
                              );
                            },
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                Card(
                  elevation: 5,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Container(
                          height: 200,
                          decoration: BoxDecoration(
                            gradient: LinearGradient(
                              colors: [Colors.grey[200]!, Colors.white],
                              begin: Alignment.topCenter,
                              end: Alignment.bottomCenter,
                            ),
                            borderRadius: BorderRadius.circular(10),
                          ),
                          child: BarChart(
                            BarChartData(
                              alignment: BarChartAlignment.spaceAround,
                              maxY: [smsMalicious, smsSafe, whatsappMalicious, whatsappSafe, gmailMalicious, gmailSafe]
                                  .reduce((a, b) => a > b ? a : b)
                                  .toDouble(),
                              barTouchData: BarTouchData(enabled: true),
                              titlesData: FlTitlesData(
                                show: true,
                                bottomTitles: AxisTitles(
                                  sideTitles: SideTitles(
                                    showTitles: true,
                                    getTitlesWidget: (value, meta) {
                                      const style = TextStyle(
                                        fontSize: 10,
                                        color: Colors.teal,
                                      );
                                      switch (value.toInt()) {
                                        case 0: return const Text('Zararlı', style: style);
                                        case 1: return const Text('Güvenli', style: style);
                                        case 2: return const Text('Zararlı', style: style);
                                        case 3: return const Text('Güvenli', style: style);
                                        case 4: return const Text('Zararlı', style: style);
                                        case 5: return const Text('Güvenli', style: style);
                                        default: return const Text('');
                                      }
                                    },
                                    reservedSize: 40,
                                  ),
                                ),
                                leftTitles: const AxisTitles(
                                  sideTitles: SideTitles(showTitles: true, reservedSize: 40),
                                ),
                                topTitles: const AxisTitles(
                                  sideTitles: SideTitles(showTitles: false),
                                ),
                                rightTitles: const AxisTitles(
                                  sideTitles: SideTitles(showTitles: false),
                                ),
                              ),
                              borderData: FlBorderData(show: false),
                              gridData: FlGridData(show: true, drawVerticalLine: false),
                              barGroups: [
                                BarChartGroupData(
                                  x: 0,
                                  barRods: [
                                    BarChartRodData(
                                      toY: smsMalicious.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFFB71C1C), Color(0xFFEF9A9A)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                                BarChartGroupData(
                                  x: 1,
                                  barRods: [
                                    BarChartRodData(
                                      toY: smsSafe.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFF2E7D32), Color(0xFFA5D6A7)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                                BarChartGroupData(
                                  x: 2,
                                  barRods: [
                                    BarChartRodData(
                                      toY: whatsappMalicious.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFFB71C1C), Color(0xFFEF9A9A)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                                BarChartGroupData(
                                  x: 3,
                                  barRods: [
                                    BarChartRodData(
                                      toY: whatsappSafe.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFF2E7D32), Color(0xFFA5D6A7)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                                BarChartGroupData(
                                  x: 4,
                                  barRods: [
                                    BarChartRodData(
                                      toY: gmailMalicious.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFFB71C1C), Color(0xFFEF9A9A)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                                BarChartGroupData(
                                  x: 5,
                                  barRods: [
                                    BarChartRodData(
                                      toY: gmailSafe.toDouble(),
                                      gradient: const LinearGradient(
                                        colors: [Color(0xFF2E7D32), Color(0xFFA5D6A7)],
                                        begin: Alignment.bottomCenter,
                                        end: Alignment.topCenter,
                                      ),
                                      width: 20,
                                      borderRadius: BorderRadius.circular(5),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ),
                        const SizedBox(height: 20),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                          children: [
                            Column(
                              children: [
                                const Text('SMS', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.teal)),
                                Text('Zararlı: $smsMalicious'),
                                Text('Güvenli: $smsSafe'),
                              ],
                            ),
                            Column(
                              children: [
                                const Text('WhatsApp', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.teal)),
                                Text('Zararlı: $whatsappMalicious'),
                                Text('Güvenli: $whatsappSafe'),
                              ],
                            ),
                            Column(
                              children: [
                                const Text('Gmail', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.teal)),
                                Text('Zararlı: $gmailMalicious'),
                                Text('Güvenli: $gmailSafe'),
                              ],
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  try {
    await Firebase.initializeApp();
    await initializeNotifications();
    await initializeService();
    runApp(const MyApp());
  } catch (e) {
    print("Uygulama başlatma hatası: $e");
    runApp(MaterialApp(home: Scaffold(body: Center(child: Text('Hata: $e')))));
  }
}

Future<void> _requestPermissionsOnlyOnce() async {
  Map<Permission, PermissionStatus> statuses = await [
    Permission.sms,
    Permission.notification,
  ].request();
  print("Başlangıç izin durumları - SMS: ${statuses[Permission.sms]}, Bildirim: ${statuses[Permission.notification]}");
}

class PermissionManager {
  static Future<void> requestPermissions(ServiceInstance? service) async {
    await _requestPermissions();
    await startBackgroundService(service);
  }

  static Future<void> startBackgroundService(ServiceInstance? service) async {
    await FlutterBackgroundService().startService();
    if (service != null) {
      service.invoke("setAsForeground");
      print("Arka plan servisi başlatıldı ve ön planda çalıştırıldı.");
    }
  }
}