using LineWars.Model;
using Localization;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New CompanyData", menuName = "Data/Create CompanyData", order = 50)]
    public class CompanyData: ScriptableObject
    {
        [SerializeField] private LanguageField<string> langCompanyName;
        [SerializeField] [LanguageTextArea] private LanguageField<string> langCompanyDescription;
        [SerializeField] private Sprite companyImage;
        [SerializeField] private NationType companyNation;
        
        public string Name => langCompanyName;
        public string Description => langCompanyDescription;
        public Sprite Image => companyImage;
        public NationType Nation => companyNation;
    }
}