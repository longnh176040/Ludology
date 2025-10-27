using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATA_KEY
{
    public static string GAME_DATA_KEY = "game_data";
}

public class DataManager : Singleton<DataManager>
{

    public SettingData settingData { get; private set; }
    public bool isLoad { get; private set; }

    public void Awake()
    {
        //base.Awake();
        Load();
    }

    private void OnApplicationPause(bool pause)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        if (!isLoad) return;

        string _data = JsonUtility.ToJson(settingData);
        PlayerPrefs.SetString(DATA_KEY.GAME_DATA_KEY, _data);
        PlayerPrefs.Save();

    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(DATA_KEY.GAME_DATA_KEY))
        {
            string _data = PlayerPrefs.GetString(DATA_KEY.GAME_DATA_KEY);
            settingData = JsonUtility.FromJson<SettingData>(_data);
        }
        else
        {
            settingData = new();
        }

        isLoad = true;
    }
}

[Serializable]
public class SettingData
{
    public float sound = 0;
    public float music = 0;

    public SettingData()
    {
        sound = 0;
        music = 0;
    }
}