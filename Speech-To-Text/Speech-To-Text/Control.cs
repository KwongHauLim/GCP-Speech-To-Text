﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using HaLi.AudioInput;
using HaLi.Tools.Encryption;
using Newtonsoft.Json;
using Speech_To_Text.View.Manual;
using WindowsInput;
using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;

namespace Speech_To_Text
{
    public class Control
    {
        private static Control _ptr;
        public static Control Share => _ptr = _ptr ?? new Control();

        private readonly string path = "Setting.json";
        public Setting Setting { get; set; }
        public Dictionary<string,string> LanguageCodes { get; private set; }

        public ManualUI Manual { get; private set; }

        public InputSimulator Input { get; private set; }
        public static void InputText(string str) => Share.Input.Keyboard.TextEntry(str);

        private Control()
        {
            LanguageCodes = new Dictionary<string, string>
            {
                ["yue-Hant-HK"] = "廣東話 (香港)",
                ["zh"] = "普通话 (中国大陆)",
                ["en"] = "English",

                ["af-ZA"] = "Afrikaans (Suid-Afrika)",
                ["am-ET"] = "አማርኛ (ኢትዮጵያ)",
                ["hy-AM"] = "Հայ (Հայաստան)",
                ["az-AZ"] = "Azərbaycan (Azərbaycan)",
                ["id-ID"] = "Bahasa Indonesia (Indonesia)",
                ["ms-MY"] = "Bahasa Melayu (Malaysia)",
                ["bn-BD"] = "বাংলা (বাংলাদেশ)",
                ["bn-IN"] = "বাংলা (ভারত)",
                ["ca-ES"] = "Català (Espanya)",
                ["cs-CZ"] = "Čeština (Česká republika)",
                ["da-DK"] = "Dansk (Danmark)",
                ["de-DE"] = "Deutsch (Deutschland)",
                ["en-AU"] = "English (Australia)",
                ["en-CA"] = "English (Canada)",
                ["en-GH"] = "English (Ghana)",
                ["en-GB"] = "English (Great Britain)",
                ["en-IN"] = "English (India)",
                ["en-IE"] = "English (Ireland)",
                ["en-KE"] = "English (Kenya)",
                ["en-NZ"] = "English (New Zealand)",
                ["en-NG"] = "English (Nigeria)",
                ["en-PH"] = "English (Philippines)",
                ["en-SG"] = "English (Singapore)",
                ["en-ZA"] = "English (South Africa)",
                ["en-TZ"] = "English (Tanzania)",
                ["en-US"] = "English (United States)",
                ["es-AR"] = "Español (Argentina)",
                ["es-BO"] = "Español (Bolivia)",
                ["es-CL"] = "Español (Chile)",
                ["es-CO"] = "Español (Colombia)",
                ["es-CR"] = "Español (Costa Rica)",
                ["es-EC"] = "Español (Ecuador)",
                ["es-SV"] = "Español (El Salvador)",
                ["es-ES"] = "Español (España)",
                ["es-US"] = "Español (Estados Unidos)",
                ["es-GT"] = "Español (Guatemala)",
                ["es-HN"] = "Español (Honduras)",
                ["es-MX"] = "Español (México)",
                ["es-NI"] = "Español (Nicaragua)",
                ["es-PA"] = "Español (Panamá)",
                ["es-PY"] = "Español (Paraguay)",
                ["es-PE"] = "Español (Perú)",
                ["es-PR"] = "Español (Puerto Rico)",
                ["es-DO"] = "Español (República Dominicana)",
                ["es-UY"] = "Español (Uruguay)",
                ["es-VE"] = "Español (Venezuela)",
                ["eu-ES"] = "Euskara (Espainia)",
                ["fil-PH"] = "Filipino (Pilipinas)",
                ["fr-CA"] = "Français (Canada)",
                ["fr-FR"] = "Français (France)",
                ["gl-ES"] = "Galego (España)",
                ["ka-GE"] = "ქართული (საქართველო)",
                ["gu-IN"] = "ગુજરાતી (ભારત)",
                ["hr-HR"] = "Hrvatski (Hrvatska)",
                ["zu-ZA"] = "IsiZulu (Ningizimu Afrika)",
                ["is-IS"] = "Íslenska (Ísland)",
                ["it-IT"] = "Italiano (Italia)",
                ["jv-ID"] = "Jawa (Indonesia)",
                ["kn-IN"] = "ಕನ್ನಡ (ಭಾರತ)",
                ["km-KH"] = "ភាសាខ្មែរ (កម្ពុជា)",
                ["lo-LA"] = "ລາວ (ລາວ)",
                ["lv-LV"] = "Latviešu (latviešu)",
                ["lt-LT"] = "Lietuvių (Lietuva)",
                ["hu-HU"] = "Magyar (Magyarország)",
                ["ml-IN"] = "മലയാളം (ഇന്ത്യ)",
                ["mr-IN"] = "मराठी (भारत)",
                ["nl-NL"] = "Nederlands (Nederland)",
                ["ne-NP"] = "नेपाली (नेपाल)",
                ["nb-NO"] = "Norsk bokmål (Norge)",
                ["pl-PL"] = "Polski (Polska)",
                ["pt-BR"] = "Português (Brasil)",
                ["pt-PT"] = "Português (Portugal)",
                ["ro-RO"] = "Română (România)",
                ["si-LK"] = "සිංහල (ශ්රී ලංකාව)",
                ["sk-SK"] = "Slovenčina (Slovensko)",
                ["sl-SI"] = "Slovenščina (Slovenija)",
                ["su-ID"] = "Urang (Indonesia)",
                ["sw-TZ"] = "Swahili (Tanzania)",
                ["sw-KE"] = "Swahili (Kenya)",
                ["fi-FI"] = "Suomi (Suomi)",
                ["sv-SE"] = "Svenska (Sverige)",
                ["ta-IN"] = "தமிழ் (இந்தியா)",
                ["ta-SG"] = "தமிழ் (சிங்கப்பூர்)",
                ["ta-LK"] = "தமிழ் (இலங்கை)",
                ["ta-MY"] = "தமிழ் (மலேசியா)",
                ["te-IN"] = "తెలుగు (భారతదేశం)",
                ["vi-VN"] = "Tiếng Việt (Việt Nam)",
                ["tr-TR"] = "Türkçe (Türkiye)",
                ["ur-PK"] = "اردو (پاکستان)",
                ["ur-IN"] = "اردو (بھارت)",
                ["el-GR"] = "Ελληνικά (Ελλάδα)",
                ["bg-BG"] = "Български (България)",
                ["ru-RU"] = "Русский (Россия)",
                ["sr-RS"] = "Српски (Србија)",
                ["uk-UA"] = "Українська (Україна)",
                ["he-IL"] = "עברית (ישראל)",
                ["ar-IL"] = "العربية (إسرائيل)",
                ["ar-JO"] = "العربية (الأردن)",
                ["ar-AE"] = "العربية (الإمارات)",
                ["ar-BH"] = "العربية (البحرين)",
                ["ar-DZ"] = "العربية (الجزائر)",
                ["ar-SA"] = "العربية (السعودية)",
                ["ar-IQ"] = "العربية (العراق)",
                ["ar-KW"] = "العربية (الكويت)",
                ["ar-MA"] = "العربية (المغرب)",
                ["ar-TN"] = "العربية (تونس)",
                ["ar-OM"] = "العربية (عُمان)",
                ["ar-PS"] = "العربية (فلسطين)",
                ["ar-QA"] = "العربية (قطر)",
                ["ar-LB"] = "العربية (لبنان)",
                ["ar-EG"] = "العربية (مصر)",
                ["fa-IR"] = "فارسی (ایران)",
                ["hi-IN"] = "हिन्दी (भारत)",
                ["th-TH"] = "ไทย (ประเทศไทย)",
                ["ko-KR"] = "한국어 (대한민국)",
                ["zh-TW"] = "國語 (台灣)",
                ["ja-JP"] = "日本語（日本）",
                ["zh-HK"] = "普通話 (香港)",
            };

            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    json = Crypto.Decrypt(json);
                    Setting = JsonConvert.DeserializeObject<Setting>(json);
                }
                catch 
                {
                    Setting = null;
                }
            }

            if (Setting == null)
                Setting = new Setting();


            Input = new InputSimulator();
        }

        internal void SaveSetting()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Setting);
                json = Crypto.Encrypt(json);
                File.WriteAllText(path, json);
            }
            catch 
            {
            }
        }

        public void ManualOpen()
        {
            if (Manual == null)
            {
                var desktop = System.Windows.SystemParameters.WorkArea;
                Manual = new ManualUI();
                Manual.Show();
                Manual.Left = desktop.Width - Manual.Width;
                Manual.Top = desktop.Bottom - Manual.Height; 
            }
        }

        public void ManualClose()
        {
            if (Manual != null && Manual.IsVisible)
            {
                Manual.Close();
                Manual = null;
            }
        }
    }
}
