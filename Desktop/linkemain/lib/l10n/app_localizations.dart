import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:intl/intl.dart' as intl;

import 'app_localizations_en.dart';
import 'app_localizations_tr.dart';

// ignore_for_file: type=lint

/// Callers can lookup localized strings with an instance of AppLocalizations
/// returned by `AppLocalizations.of(context)`.
///
/// Applications need to include `AppLocalizations.delegate()` in their app's
/// `localizationDelegates` list, and the locales they support in the app's
/// `supportedLocales` list. For example:
///
/// ```dart
/// import 'l10n/app_localizations.dart';
///
/// return MaterialApp(
///   localizationsDelegates: AppLocalizations.localizationsDelegates,
///   supportedLocales: AppLocalizations.supportedLocales,
///   home: MyApplicationHome(),
/// );
/// ```
///
/// ## Update pubspec.yaml
///
/// Please make sure to update your pubspec.yaml to include the following
/// packages:
///
/// ```yaml
/// dependencies:
///   # Internationalization support.
///   flutter_localizations:
///     sdk: flutter
///   intl: any # Use the pinned version from flutter_localizations
///
///   # Rest of dependencies
/// ```
///
/// ## iOS Applications
///
/// iOS applications define key application metadata, including supported
/// locales, in an Info.plist file that is built into the application bundle.
/// To configure the locales supported by your app, you’ll need to edit this
/// file.
///
/// First, open your project’s ios/Runner.xcworkspace Xcode workspace file.
/// Then, in the Project Navigator, open the Info.plist file under the Runner
/// project’s Runner folder.
///
/// Next, select the Information Property List item, select Add Item from the
/// Editor menu, then select Localizations from the pop-up menu.
///
/// Select and expand the newly-created Localizations item then, for each
/// locale your application supports, add a new item and select the locale
/// you wish to add from the pop-up menu in the Value field. This list should
/// be consistent with the languages listed in the AppLocalizations.supportedLocales
/// property.
abstract class AppLocalizations {
  AppLocalizations(String locale) : localeName = intl.Intl.canonicalizedLocale(locale.toString());

  final String localeName;

  static AppLocalizations? of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations);
  }

  static const LocalizationsDelegate<AppLocalizations> delegate = _AppLocalizationsDelegate();

  /// A list of this localizations delegate along with the default localizations
  /// delegates.
  ///
  /// Returns a list of localizations delegates containing this delegate along with
  /// GlobalMaterialLocalizations.delegate, GlobalCupertinoLocalizations.delegate,
  /// and GlobalWidgetsLocalizations.delegate.
  ///
  /// Additional delegates can be added by appending to this list in
  /// MaterialApp. This list does not have to be used at all if a custom list
  /// of delegates is preferred or required.
  static const List<LocalizationsDelegate<dynamic>> localizationsDelegates = <LocalizationsDelegate<dynamic>>[
    delegate,
    GlobalMaterialLocalizations.delegate,
    GlobalCupertinoLocalizations.delegate,
    GlobalWidgetsLocalizations.delegate,
  ];

  /// A list of this localizations delegate's supported locales.
  static const List<Locale> supportedLocales = <Locale>[
    Locale('en'),
    Locale('tr')
  ];

  /// No description provided for @appName.
  ///
  /// In en, this message translates to:
  /// **'Ulayzer'**
  String get appName;

  /// No description provided for @privacyCenter.
  ///
  /// In en, this message translates to:
  /// **'Privacy Center'**
  String get privacyCenter;

  /// No description provided for @aboutUs.
  ///
  /// In en, this message translates to:
  /// **'About Us'**
  String get aboutUs;

  /// No description provided for @yourPrivacyMatters.
  ///
  /// In en, this message translates to:
  /// **'Your Privacy Matters'**
  String get yourPrivacyMatters;

  /// No description provided for @privacyDescription.
  ///
  /// In en, this message translates to:
  /// **'At Ulayzer, we highly value our users\' privacy. Your data is a trust, and we work to protect it in the best way possible. Our app analyzes links from SMS, WhatsApp, and Gmail using only the necessary permissions and does not share any personal information with third parties.'**
  String get privacyDescription;

  /// No description provided for @dataSecurityCommitment.
  ///
  /// In en, this message translates to:
  /// **'Data Security Commitment'**
  String get dataSecurityCommitment;

  /// No description provided for @dataSecurityDetails.
  ///
  /// In en, this message translates to:
  /// **'• Your data is stored locally on your device and is never sent to our servers.\n• Link analyses are performed only through trusted APIs like Google Safe Browsing and VirusTotal.\n• Our app operates only with the permissions you grant and uses them only as necessary.\n• We use the latest technologies for anonymizing and encrypting user data.'**
  String get dataSecurityDetails;

  /// No description provided for @youAreValuable.
  ///
  /// In en, this message translates to:
  /// **'You Are Valuable'**
  String get youAreValuable;

  /// No description provided for @youAreValuableDescription.
  ///
  /// In en, this message translates to:
  /// **'Your security and satisfaction are the most important to us. Ulayzer is designed to provide a safer digital experience. If you have any questions about privacy, feel free to contact us. We are here for you!'**
  String get youAreValuableDescription;

  /// No description provided for @whatIsUlayzer.
  ///
  /// In en, this message translates to:
  /// **'What is Ulayzer?'**
  String get whatIsUlayzer;

  /// No description provided for @whatIsUlayzerDescription.
  ///
  /// In en, this message translates to:
  /// **'Ulayzer is an app designed to enhance your security in the digital world. It analyzes links from SMS, WhatsApp, and Gmail to protect you from harmful content. Our goal is to offer a safer digital experience and protect users from malicious links.'**
  String get whatIsUlayzerDescription;

  /// No description provided for @whoAreWe.
  ///
  /// In en, this message translates to:
  /// **'Who Are We?'**
  String get whoAreWe;

  /// No description provided for @whoAreWeDescription.
  ///
  /// In en, this message translates to:
  /// **'We are a team that aims to improve people\'s lives and security using technology. With our user-focused approach, we always strive to provide the best experience. Ulayzer emerged as part of this vision and aims to enhance digital security by reaching more users every day.'**
  String get whoAreWeDescription;

  /// No description provided for @ourMission.
  ///
  /// In en, this message translates to:
  /// **'Our Mission'**
  String get ourMission;

  /// No description provided for @ourMissionDescription.
  ///
  /// In en, this message translates to:
  /// **'To prioritize security and privacy in the digital world, providing the best protection for our users. Stay one step ahead with Ulayzer and avoid harmful links!'**
  String get ourMissionDescription;

  /// No description provided for @developers.
  ///
  /// In en, this message translates to:
  /// **'Developers'**
  String get developers;

  /// No description provided for @developersNames.
  ///
  /// In en, this message translates to:
  /// **'Zehra DEMİRTKOP - Aycan SÖKMEN'**
  String get developersNames;

  /// No description provided for @toggleApp.
  ///
  /// In en, this message translates to:
  /// **'Toggle App On/Off'**
  String get toggleApp;

  /// No description provided for @appRunning.
  ///
  /// In en, this message translates to:
  /// **'App is running'**
  String get appRunning;

  /// No description provided for @appStopped.
  ///
  /// In en, this message translates to:
  /// **'App is stopped'**
  String get appStopped;

  /// No description provided for @securityStatus.
  ///
  /// In en, this message translates to:
  /// **'Security Status'**
  String get securityStatus;

  /// No description provided for @unknown.
  ///
  /// In en, this message translates to:
  /// **'Unknown'**
  String get unknown;

  /// No description provided for @safe.
  ///
  /// In en, this message translates to:
  /// **'Safe'**
  String get safe;

  /// No description provided for @caution.
  ///
  /// In en, this message translates to:
  /// **'Caution'**
  String get caution;

  /// No description provided for @risky.
  ///
  /// In en, this message translates to:
  /// **'Risky'**
  String get risky;

  /// No description provided for @recentAnalyses.
  ///
  /// In en, this message translates to:
  /// **'Recent Analyses'**
  String get recentAnalyses;

  /// No description provided for @malicious.
  ///
  /// In en, this message translates to:
  /// **'Malicious'**
  String get malicious;

  /// No description provided for @secure.
  ///
  /// In en, this message translates to:
  /// **'Secure'**
  String get secure;
}

class _AppLocalizationsDelegate extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  Future<AppLocalizations> load(Locale locale) {
    return SynchronousFuture<AppLocalizations>(lookupAppLocalizations(locale));
  }

  @override
  bool isSupported(Locale locale) => <String>['en', 'tr'].contains(locale.languageCode);

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;
}

AppLocalizations lookupAppLocalizations(Locale locale) {


  // Lookup logic when only language code is specified.
  switch (locale.languageCode) {
    case 'en': return AppLocalizationsEn();
    case 'tr': return AppLocalizationsTr();
  }

  throw FlutterError(
    'AppLocalizations.delegate failed to load unsupported locale "$locale". This is likely '
    'an issue with the localizations generation tool. Please file an issue '
    'on GitHub with a reproducible sample app and the gen-l10n configuration '
    'that was used.'
  );
}
