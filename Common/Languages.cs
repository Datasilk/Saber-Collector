using System.Collections.Generic;

namespace Saber.Vendors.Collector
{
    public static class Languages
    {
        public static LanguageDetection.LanguageDetector Detector { get; set; }

        public static Dictionary<string, string> KnownLanguages { get; set; } = new Dictionary<string, string>()
        {
            {"af", "Afrikaans" },
            {"ar", "Arabic" },
            {"bg", "Bulgarian" },
            {"bn", "Bengali" },
            {"cs", "Czech" },
            {"da", "Danish" },
            {"de", "German" },
            {"el", "Greek" },
            {"en", "English" },
            {"es", "Spanish" },
            {"et", "Estonian" },
            {"fa", "Persian" },
            {"fi", "Finnish" },
            {"fr", "French" },
            {"gu", "Gujarati" },
            {"he", "Hebrew" },
            {"hi", "Hindi" },
            {"hr", "Croatian" },
            {"hu", "Hungarian" },
            {"id", "Indonesian" },
            {"it", "Italian" },
            {"ja", "Japanese" },
            {"kn", "Kannada" },
            {"ko", "Korean" },
            {"lt", "Lithuanian" },
            {"lv", "Latvian" },
            {"mk", "Macedonian" },
            {"ml", "Malayalam" },
            {"mr", "Marathi" },
            {"ne", "Nepali" },
            {"nl", "Flemish" },
            {"no", "Norwegian" },
            {"pa", "Panjabi" },
            {"pl", "Polish" },
            {"pt", "Portuguese" },
            {"ro", "Romanian" },
            {"ru", "Russian" },
            {"sk", "Slovak" },
            {"sl", "Slovenian" },
            {"so", "Somali" },
            {"sq", "Albanian" },
            {"sv", "Swedish" },
            {"sw", "Swahili" },
            {"ta", "Tamil" },
            {"te", "Telugu" },
            {"th", "Thai" },
            {"tl", "Tagalog" },
            {"tr", "Turkish" },
            {"uk", "Ukrainian" },
            {"ur", "Urdu" },
            {"vi", "Vietnamese" },
            {"zh", "Chinese" },
            {"zh-cn", "Chinese" },
            {"zh-tw", "Taiwanese" }
        };
    }
}
