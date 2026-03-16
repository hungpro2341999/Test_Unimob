using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIUpgrade : MonoBehaviour
{
    public Image imgIcon;
    public TextMeshProUGUI txtNameUpgrade;
    public TextMeshProUGUI txtLevelDesUpgrade;
    public RippleButton btnLevelUp;
    public TextMeshProUGUI txtPrice;
    // Start is called before the first frame update
    public void ShowInfor(AbastractUpgrade upgrade)
    {
        //imgIcon.sprite = upgrade.dataCfgUpgrade.iconUpgrade;
        txtNameUpgrade.text = upgrade.dataCfgUpgrade.description;
        txtLevelDesUpgrade.text = upgrade.dataCfgUpgrade.description;
        txtPrice.text = BigNumberFormatter.Format(upgrade.PriceCurrent);
        btnLevelUp.onClick.RemoveAllListeners();
        imgIcon.sprite = upgrade.dataCfgUpgrade.spr;
        //imgIcon.SetNativeSize();
        btnLevelUp.onClick.AddListener(() =>
        {       
           upgrade.LevelUp();
           // UpgradeUI
        });
    }
}
