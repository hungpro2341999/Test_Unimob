using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewUnlock : Panel
{
    public TextMeshProUGUI txtName;
    public RippleButton btnUnlock;
    public Image imageIcon;
    public TextMeshProUGUI txtPrice;
   override protected void Start()
    {
        base.Start();
        transform.FindChildByName("Close").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Debug.Log("Close View Unlock");
            ViewUI.Instance.Hide<UIViewUnlock>();
        });
    }
    public void ShowInfor(DataResouceUnlock dataResouceUnlock, UnityEngine.Events.UnityAction actionUnlock)
    {
        txtName.text = dataResouceUnlock.des;
        imageIcon.sprite = dataResouceUnlock.icon ;
         txtPrice.text = BigNumberFormatter.Format(dataResouceUnlock.priceUnlock);
        btnUnlock.onClick.RemoveAllListeners();
        btnUnlock.onClick.AddListener(actionUnlock);
    }
    
}
