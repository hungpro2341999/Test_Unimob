using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenGamePlay : Panel
{
    public TextMeshProUGUI txtRuby;
    public TextMeshProUGUI txtMoney;
    public RippleButton btnUpgrade;
    public RippleButton btnPlusMoney;
    public RippleButton btnPlusRuby;

    protected override void Start()
    {
        base.Start();
        EventBus.Subscribe<EventUpdateMoney>(UpdateMoney);
        EventBus.Run<EventUpdateMoney>(new EventUpdateMoney { dataPlayer = GameData.GetData<DataPlayer>() });
        btnUpgrade.onClick.AddListener(() =>
        {
            PopupUpgrade popupUpgrade = null;
            PopupUI.Instance.Show<PopupUpgrade>(out popupUpgrade, null, Menu<PopupUI>.ShowType.NotHide);
            popupUpgrade.Init(SystemUpgrade.Instance.All_Upgrade);
        });

        btnPlusMoney.onClick.AddListener(() =>
        {
            BigNumber bigNumber = new BigNumber(1, 10);
            EventBus.Run(new EventChangeMoney() { changeMoney = bigNumber });
        });
    }
    void UpdateMoney(EventUpdateMoney eventChangeMoney)
    {
        txtMoney.text = BigNumberFormatter.Format(eventChangeMoney.dataPlayer.money);
        txtRuby.text = eventChangeMoney.dataPlayer.ruby.ToString();
    }
}
public struct EventUpdateMoney
{
    public DataPlayer dataPlayer;
}
public struct EventChangeMoney
{
    public BigNumber changeMoney;
}
