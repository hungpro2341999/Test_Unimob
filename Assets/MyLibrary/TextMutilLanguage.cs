using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextMutilLanguage : MonoBehaviour
{
    public int ID = -1;
    Text txt;
    void Start()
    {
        txt = GetComponent<Text>();
        ImportText();
    }
    public void SetText(int ID)
    {
        this.ID = ID;
        ImportText();
    }
    void OnEnable()
    {
        ImportText();
     //   GameAction.Instance.RegisterAction(TypeAction.UpdateText,ImportText);
    }
    void OnDisable()
    {
     //   GameAction.Instance.UnRegisterAction(TypeAction.UpdateText,ImportText);
    }
    void ImportText()
    {
        GetComponent<Text>().text = GameText.GetTextMutil(ID);
    }
}