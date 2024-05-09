using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Localization;
using UnityEngine;

namespace LineWars
{
    public class TestScript : MonoBehaviour
    {
        [LanguageTextArea(70), SerializeField] private LanguageField<string> textArea;
        public List<LanguageField<string>> test;
        public SerializedDictionary<string, LanguageField<List<string>>> test2;
        public Color Color;
    }
}
