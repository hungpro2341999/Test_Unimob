using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUp : Panel
{
    public Image imgRs;
    public RippleButton btnLevelUp;
    public TextMeshProUGUI txtTittle;
    public TextMeshProUGUI txtNameProduct;
    public TextMeshProUGUI txtPrice;
    public TextMeshProUGUI txIncome;
    public TextMeshProUGUI txtSpeed;
    public Slider sliderProcess;
    public Transform contentUpgrade;
    public Transform contentMax;
    public Tree tree;

    void Start()
    {
        transform.FindChildByName("Close").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            ViewUI.Instance.Hide<UILevelUp>();
        });
        EventBus.Subscribe<Tree.EventUpgradeTree>((ID) =>
        {
            if (ID.IDTree == tree.Upgrade_Tree.dataLevelUpTree.ID)
            {
                ShowInfor(tree);
            }
        });
    }

    public void ShowInfor(Tree tree)
    {
        this.tree = tree;
        var upgradeTree = tree.Upgrade_Tree;        
        transform.FindChildByName("Frame").transform.position = Camera.main.WorldToScreenPoint(tree.transform.position + Vector3.up * 1.5f);
        var defineRs = DataConfigUpgrade.Instance.GetResoucesDefine(upgradeTree.dataLevelUpTree.ID);
        btnLevelUp.onClick.RemoveAllListeners();
        sliderProcess.value = (float)upgradeTree.level / (upgradeTree.dataLevelUpTree.levelMax - 1);
        txtTittle.text = "Level " + (upgradeTree.level + 1);
        txtNameProduct.text = defineRs.name;
        txtPrice.text = BigNumberFormatter.Format( upgradeTree.ValuePriceUpgrade);
        txIncome.text = BigNumberFormatter.Format( tree.profit.Value);
        var dividedPrice = tree.profit.Value / new BigNumber(3, 0);
        txtSpeed.text =  (upgradeTree.ValueSpeed.ToString())+"s"+"/"+BigNumberFormatter.Format(dividedPrice);      
        imgRs.sprite = defineRs.image;
        imgRs.SetNativeSize();
        btnLevelUp.onClick.AddListener(() =>
        {
            upgradeTree.LevelUp();
        });
        if (upgradeTree.IsMaxLevel())
        {
            contentUpgrade.gameObject.SetActive(false);
            contentMax.gameObject.SetActive(true);
            btnLevelUp.interactable = false;

        }
        else
        {
            contentUpgrade.gameObject.SetActive(true);
            contentMax.gameObject.SetActive(false);
            btnLevelUp.interactable = true;
        }
    }
}
