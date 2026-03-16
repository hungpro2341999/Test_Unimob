using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Language
{
    public List<ElemenetLangauge> elemenetLangauges = new List<ElemenetLangauge>();
}
[System.Serializable]
public class ElemenetLangauge
{
    public int ID;
    public List<string> allLanguage = new List<string>();
}
public class GameText
{
    public static Dictionary<int, List<string>> Language = new Dictionary<int, List<string>>();
    public static void Init()
    {
        Language = new Dictionary<int, List<string>>();
        var txt = GameData.GetDataJson("Language");
        var lan = JsonUtility.FromJson<Language>(txt);        
        foreach (var l in lan.elemenetLangauges)
        {
            //Debug.Log("Add : " + l.ID + " - " + l.allLanguage.Count);
            Language.Add(l.ID, l.allLanguage);
        }
    }
    public static string GetTextMutil(int ID)
    {
        return Language[ID][0];
    }
    public static string GetStringLan(int index)
    {
        switch (index)
        {
            case 0:
                return "Tiếng Việt";
            case 1:
                return "English";
            case 2:
                return "日本語";
            case 3:
                return "한국어";
            case 4:
                return "中文";
            case 5:
                return "Русский";
            case 6:
                return "Español";
            case 7:
                return "Português";
            case 8:
                return "Français";
            case 9:
                return "العربية";
            case 10:
                return "हिन्दी";
            default:
                break;
        }
        return "Null";
    }
}