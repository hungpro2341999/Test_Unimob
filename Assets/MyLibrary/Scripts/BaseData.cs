using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
[System.Serializable]
public class GameData
{
    static Dictionary<string, object> ALLDATA = new Dictionary<string, object>();
    public static T GetData<T>() where T : Data<T>
    {
        string key = typeof(T).ToString();
        if (!ALLDATA.ContainsKey(key))
        {
            T t = (T)Activator.CreateInstance(typeof(T));
            t = t.Load();
            ALLDATA.Add(t.Key, t);
            if (!t.call)
            {
                t.Call();
                t.call = true;
            }

        }
        return (T)ALLDATA[key];
    }

    public static string GetDataJson(string name)
    {
        var txt = Resources.Load<TextAsset>("Json/" + name).ToString();
        return txt;
    }
    public static void RemoveKey<T>() where T : Data<T>
    {
        string key = typeof(T).ToString();
        GetData<T>().DeleteKey();
        ALLDATA.Remove(key);
    }
  
}

[System.Serializable]
public abstract class Data<T> where T : class
{
    [NonSerialized]
    public bool call = false;
    public string Key
    {
        get
        {
            return GetType().ToString();
        }
    }
    public virtual void Save()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            Init();
        }
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(this));
        PlayerPrefs.Save();

        // if (!ES3.FileExists(Key + ".txt"))
        // {
        //     Init();
        // }

        // ES3.Save(Key, this, Key + ".txt");
        

        // if (!File.Exists(Application.persistentDataPath + "/" + Key + ".txt"))
        // {
        //     File.Create(Application.persistentDataPath + "/" + Key + ".txt");
        //     Init();
        // }
        // File.WriteAllText(Application.persistentDataPath + "/" + Key + ".txt", JsonUtility.ToJson(this));
    }
    public virtual T Load()
    {

        if (!PlayerPrefs.HasKey(Key))
        {
            Init();
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }
        return  JsonUtility.FromJson<T>(PlayerPrefs.GetString(Key));


        // if (!ES3.FileExists(Key + ".txt"))
        // {
        //     Init();
        //     ES3.Save(Key, this, Key + ".txt");
        // }
        


       // return (T)ES3.Load(Key, Key + ".txt");



        // if (!File.Exists(Application.persistentDataPath + "/" + Key + ".txt"))
        // {
        //     File.Create(Application.persistentDataPath + "/" + Key + ".txt");
        //     Init();
        //     File.WriteAllText(Application.persistentDataPath + "/" + Key + ".txt", JsonUtility.ToJson(this));            
        // }
        // Debug.Log(Application.persistentDataPath + "/" + Key + ".txt");
        // return JsonUtility.FromJson<T>(File.ReadAllText(Application.persistentDataPath + "/" + Key + ".txt"));
    }
    public void DeleteKey()
    {
        // string path = Application.persistentDataPath + "/" + Key + ".txt";
        // Debug.Log(path);
        // ES3.DeleteKey(Key);
        // ES3.DeleteFile(path);
    }
    public virtual void Init()
    {

    }
    public virtual void Call()
    {

    }
}
