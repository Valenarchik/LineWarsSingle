using System;
using UnityEngine;
using System.Collections;
using System.Net;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.Networking;

namespace Localization
{
    public class LanguageText : MonoBehaviour
    {
        [SerializeField, HideInInspector] private LocalizationSettings settings;
        [SerializeField] public TMP_Text textMPComponent;
        [SerializeField] public TMP_FontAsset uniqueFontTMP;
        
        [Space(10)] 
        [SerializeField] public string text;
        public string ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz, es, pt, ar, id, ja, it, de, hi = "";

        public bool changeOnlyFont;
        public int fontNumber;
        private int baseFontSize;
        
        public LocalizationSettings Settings
        {
            get
            {
                if (settings == null)
                    settings = SettingsLoader.LoadSettings();
                return settings;
            }
        }

        private void Awake()
        {
            if (textMPComponent)
                baseFontSize = Mathf.RoundToInt(textMPComponent.fontSize); 
        }

        [ContextMenu("Reserialize")]
        public void Serialize()
        {
            textMPComponent = GetComponent<TMP_Text>();
        }
        private void OnEnable()
        {
            SwitchLanguage();
            LocalizationManager.SwitchLangEvent += SwitchLanguage;
        }

        private void OnDisable()
        {
            LocalizationManager.SwitchLangEvent -= SwitchLanguage;
        }

        public void ClearAllValues()
        {
            ru = "";
            en = "";
            tr = "";
            az = "";
            be = "";
            he = "";
            hy = "";
            ka = "";
            et = "";
            fr = "";
            kk = "";
            ky = "";
            lt = "";
            lv = "";
            ro = "";
            tg = "";
            tk = "";
            uk = "";
            uz = "";
            es = "";
            pt = "";
            ar = "";
            id = "";
            ja = "";
            it = "";
            de = "";
            hi = "";
        }

        private void SwitchLanguage()
        { 
            if (!changeOnlyFont)
                AssignTranslate(currentLanguage);
            if (textMPComponent)
                ChangeFont();

            FontSizeCorrect();
        }

        private void AssignTranslate(string translation)
        { 
            if (textMPComponent)
                textMPComponent.text = translation;
        }
        
        private void ChangeFont()
        {
            if (uniqueFontTMP)
            {
                textMPComponent.font = uniqueFontTMP;
                return;
            }
            var currentFont = LangMethods.GetCurrentFont(LocalizationManager.LangName, fontNumber, Settings);
            if (currentFont)
            {
                textMPComponent.font = currentFont;
            }
        }

        void FontSizeCorrect()
        { 
            if (textMPComponent)
            {
                var correct = LangMethods.GetCurrentFontSizeCorrect(LocalizationManager.LangName, fontNumber, Settings);
                textMPComponent.fontSize = baseFontSize + correct;
            }
        }

        public string[] languages
        {
            get
            {
                var s = new string[27];

                s[0] = ru;
                s[1] = en;
                s[2] = tr;
                s[3] = az;
                s[4] = be;
                s[5] = he;
                s[6] = hy;
                s[7] = ka;
                s[8] = et;
                s[9] = fr;
                s[10] = kk;
                s[11] = ky;
                s[12] = lt;
                s[13] = lv;
                s[14] = ro;
                s[15] = tg;
                s[16] = tk;
                s[17] = uk;
                s[18] = uz;
                s[19] = es;
                s[20] = pt;
                s[21] = ar;
                s[22] = id;
                s[23] = ja;
                s[24] = it;
                s[25] = de;
                s[26] = hi;

                return s;
            }
            set
            {
                ru = value[0];
                en = value[1];
                tr = value[2];
                az = value[3];
                be = value[4];
                he = value[5];
                hy = value[6];
                ka = value[7];
                et = value[8];
                fr = value[9];
                kk = value[10];
                ky = value[11];
                lt = value[12];
                lv = value[13];
                ro = value[14];
                tg = value[15];
                tk = value[16];
                uk = value[17];
                uz = value[18];
                es = value[19];
                pt = value[20];
                ar = value[21];
                id = value[22];
                ja = value[23];
                it = value[24];
                de = value[25];
                hi = value[26];
            }
        }

        public string currentLanguage => languages[(int) LocalizationManager.LangName];
        
#if UNITY_EDITOR
        [HideInInspector] public float textHeight = 20f;
        [HideInInspector] public string processTranslateLabel;
        [HideInInspector] public bool componentTextField;

        public void SetLang(int index, string text)
        {
            string[] str = languages;
            str[index] = text;

            languages = str;
        }

        public void Translate(int countLangAvailable)
        {
            RunTranslateEmptyFields(countLangAvailable);
        }

        string TranslateGoogle(string translationTo = "en")
        {
            string text;
            if (!componentTextField)
                text = this.text;
            else if (textMPComponent)
                text = textMPComponent.text;
            else
            {
                Debug.LogError("(The text for translation was not found!");
                return null;
            }

            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("(The text for translation was not found!");
                return null;
            }

            var url = String.Format("https://translate.google." + Settings.domainAutoLocalization + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
                "auto", translationTo, WebUtility.UrlEncode(text));
            var www = UnityWebRequest.Get(url);
            www.SendWebRequest();
            while (!www.isDone)
            {

            }
            var response = www.downloadHandler.text;

            try
            {
                var jsonArray = JArray.Parse(response);
                response = jsonArray[0][0][0].ToString();
            }
            catch
            {
                response = "process error";
                StopAllCoroutines();
                processTranslateLabel = processTranslateLabel + " error";

                Debug.LogError("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
            }

            return response;
        }

        [HideInInspector] public int countLang = 0;

        IEnumerator TranslateEmptyFields(int countLangAvailable)
        {
            countLang = 0;
            processTranslateLabel = "processing... 0/" + countLangAvailable;

            for (int i = 0; i < languages.Length; i++)
            {
                if (LangMethods.GetLangArr(Settings)[i] && (languages[i] == null || languages[i] == ""))
                {
                    bool complete = false;
                    string translate = TranslateGoogle(LangMethods.GetLangName(i));

                    if (translate == null)
                        yield return null;

                    SetLang(i, translate);

                    if (processTranslateLabel.Contains("error"))
                    {
                        processTranslateLabel = countLang + "/" + countLangAvailable + " error";
                    }
                    else
                    {
                        complete = true;
                        processTranslateLabel = countLang + "/" + countLangAvailable;
                    }

                    yield return complete == true;
                    countLang++;
                }
            }

            processTranslateLabel = countLang + "/" + countLangAvailable + " completed";
        }

        private void RunTranslateEmptyFields(int countLangAvailable)
        {
            countLang = 0;
            processTranslateLabel = "processing... 0/" + countLangAvailable;

            for (int i = 0; i < languages.Length; i++)
            {
                if (LangMethods.GetLangArr(Settings)[i] && (languages[i] == null || languages[i] == ""))
                {
                    string translate = TranslateGoogle(LangMethods.GetLangName(i));

                    if (translate == null)
                        return;

                    SetLang(i, translate);

                    if (processTranslateLabel.Contains("error"))
                        processTranslateLabel = countLang + "/" + countLangAvailable + " error";
                    else
                    {
                        processTranslateLabel = countLang + "/" + countLangAvailable;
                    }

                    countLang++;
                }
            }

            processTranslateLabel = countLang + "/" + countLangAvailable + " completed";
        }
#endif
    }
}