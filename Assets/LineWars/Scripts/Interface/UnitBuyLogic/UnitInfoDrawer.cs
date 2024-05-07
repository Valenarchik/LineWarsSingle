using LineWars.Model;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitDescription;
    
    [SerializeField] private TMP_Text hpAmount;
    [SerializeField] private TMP_Text actionPointsAmount;
    [SerializeField] private TMP_Text damageAmount;
    [SerializeField] private TMP_Text moveAmount;

    [SerializeField] private Image unitImage;

    public void Init(Unit unit)
    {
        if(unitName != null)
            unitName.text = unit.UnitName;
        if(unitDescription != null)
            unitDescription.text = unit.UnitDescription;
        
        if (hpAmount != null)
            hpAmount.text = unit.MaxHp.ToString();
        if (actionPointsAmount != null)
            actionPointsAmount.text = unit.MaxActionPoints.ToString();
        if (damageAmount != null)
            damageAmount.text = unit.InitialPower.ToString();
        if (moveAmount != null)
            moveAmount.text = unit.GetMoveCost().ToString();

        unitImage.gameObject.SetActive(true);
        unitImage.sprite = unit.Sprite;
    }

    public void RestoreDefaults()
    {
        unitName.text = LocalizationManager.LangName switch
        {
            LangName.ru => "Выберите воина",
            LangName.en => "Choose a warrior"
        };
        unitDescription.text = "";
        
        if (hpAmount != null)
            hpAmount.text = "?";
        if (actionPointsAmount != null)
            actionPointsAmount.text = "?";
        if (damageAmount != null)
            damageAmount.text = "?";

        if (damageAmount != null)
        {
            damageAmount.text = "?";
        }
        
        unitImage.gameObject.SetActive(false);
        unitImage.sprite = null;
    }
}