using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DATA_KEY
{
    public static string PLAYER_INFO_DATA_KEY = "playerinfo_data";
    public static string SETTING_DATA_KEY = "setting_data";
}

[Serializable]
public class PlayerInfoData
{
    public TeamColor color;
    public PlayerInfo info;

    public int level;
    public int exp;

    public int energy;
    public int diamond;

    public PlayerInfoData() 
    {
        color = TeamColor.RED;
        info = new("null", "avatar_red_000", "frame_000", "background_000", Guid.NewGuid().ToString("N").Substring(0, 8));
        //info.name = Guid.NewGuid().ToString("N").Substring(0, 8);
        //info.avatarID = "avatar_red_000";
        //info.frameID = "frame_000";
        //info.backgroundID = "background_000";

        level = 1;
        exp = 0;
        energy = 60;
        diamond = 0;
    }
}

public class DataManager : MonoBehaviour
{
    #region Properties

    public PlayerInfoData playerInfoData { get; private set; }
    public SettingData settingData { get; private set; }

    #endregion

    #region Member Variables

    [Inject] private SignalBus signalBus;

    private bool isLoad;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        Load();
    }

    private void Start()
    {
        if (isLoad) 
            signalBus.Fire(new DataLoaded { });
    }

    private void OnApplicationPause(bool pause)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #endregion

    public void Save()
    {
        if (!isLoad) return;

        string _playerInfoData = JsonUtility.ToJson(playerInfoData);
        PlayerPrefs.SetString(DATA_KEY.PLAYER_INFO_DATA_KEY, _playerInfoData);

        string _settingData = JsonUtility.ToJson(settingData);
        PlayerPrefs.SetString(DATA_KEY.SETTING_DATA_KEY, _settingData);

        PlayerPrefs.Save();

    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(DATA_KEY.PLAYER_INFO_DATA_KEY))
        {
            string _data = PlayerPrefs.GetString(DATA_KEY.PLAYER_INFO_DATA_KEY);
            playerInfoData = JsonUtility.FromJson<PlayerInfoData>(_data);
        }
        else
        {
            playerInfoData = new();
        }

        if (PlayerPrefs.HasKey(DATA_KEY.SETTING_DATA_KEY))
        {
            string _data = PlayerPrefs.GetString(DATA_KEY.SETTING_DATA_KEY);
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