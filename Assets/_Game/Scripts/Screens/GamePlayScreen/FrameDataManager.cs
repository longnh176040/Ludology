using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDataManager : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private PlayerFrameDataSO playerFrameData;

    #endregion

    #region Member Variables

    private Dictionary<string, AvatarItemSO> avatarDict;
    private Dictionary<string, FrameItemSO> frameDict;
    private Dictionary<string, BackgroundItemSO> backgroundDict;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        BuildItemDictionary();
    }

    #endregion

    #region Public Methods

    public PlayerInfo RandomPlayerInfo(TeamColor color)
    {
        AvatarItemSO[] avatars = GetAvatarListByColor(color);
        if (avatars == null) Debug.LogError("Avatar data not found!");

        int avatarID = UnityEngine.Random.Range(0, avatars.Length);
        int frameID = UnityEngine.Random.Range(0, playerFrameData.frameItems.Length);
        int backgroundID = UnityEngine.Random.Range(0, playerFrameData.backgroundItems.Length);

        var playerInfo = new PlayerInfo("null", 
            avatars[avatarID].id, 
            playerFrameData.frameItems[frameID].id, 
            playerFrameData.backgroundItems[backgroundID].id, 
            Utilities.RandomName());

        return playerInfo;
    }

    public AvatarItemSO GetAvatarItemByIndex(TeamColor color, int idx = 0)
    {
        return GetAvatarListByColor(color)[idx];
    }

    public AvatarItemSO GetAvatarItemByID(string id)
    {
        return avatarDict[id];
    }

    public BackgroundItemSO GetBackgroundItemByIndex(int idx = 0)
    {
        return playerFrameData.backgroundItems[idx];
    }

    public BackgroundItemSO GetBackgroundItemByID(string id)
    {
        return backgroundDict[id];
    }

    public FrameItemSO GetFrameItemByIndex(int idx = 0)
    {
        return playerFrameData.frameItems[idx];
    }

    public FrameItemSO GetFrameItemByID(string id)
    {
        return frameDict[id];
    }

    #endregion

    #region Private Methods

    private void BuildItemDictionary()
    {
        avatarDict = new();

        List<AvatarItemSO> avatarMerged = new List<AvatarItemSO>(playerFrameData.redAvatars);
        avatarMerged.AddRange(playerFrameData.blueAvatars);
        avatarMerged.AddRange(playerFrameData.greenAvatars);
        avatarMerged.AddRange(playerFrameData.yellowAvatars);

        foreach (var avatar in avatarMerged)
        {
            if (!avatarDict.ContainsKey(avatar.id))
                avatarDict.Add(avatar.id, avatar);
            else
                Debug.LogWarning($"Duplicate avatar ID found: {avatar.id}");
        }

        frameDict = new();
        foreach (var frame in playerFrameData.frameItems)
        {
            if (!frameDict.ContainsKey(frame.id))
                frameDict.Add(frame.id, frame);
            else
                Debug.LogWarning($"Duplicate frame ID found: {frame.id}");
        }

        backgroundDict = new();
        foreach (var bg in playerFrameData.backgroundItems)
        {
            if (!backgroundDict.ContainsKey(bg.id))
                backgroundDict.Add(bg.id, bg);
            else
                Debug.LogWarning($"Duplicate background ID found: {bg.id}");
        }
    }

    private AvatarItemSO[] GetAvatarListByColor(TeamColor color)
    {
        switch (color)
        {
            case TeamColor.RED:
                return playerFrameData.redAvatars;
            case TeamColor.GREEN:
                return playerFrameData.greenAvatars;
            case TeamColor.BLUE:
                return playerFrameData.blueAvatars;
            case TeamColor.YELLOW:
                return playerFrameData.yellowAvatars;
        }
        return null;
    }

/*    private IEnumerator IERandomPlayerFrame(PlayerFrame player, float minDuration = 0.5f, float maxDuration = 2f)
    {
        float duration = Random.Range(minDuration, maxDuration);
        float elapsed = 0f;
        float startInterval = 0.05f; // init spd
        float endInterval = 0.3f;

        // Random avatars
        AvatarItemSO[] avatars = GetAvatarListByColor(player.Color);
        if (avatars == null) Debug.LogError("Avatar data not found!");

        while (elapsed < duration)
        {
            Sprite tempAvatar = avatars[Random.Range(0, avatars.Length)].sprite;
            player.SetAvatar(tempAvatar);

            float t = elapsed / duration;
            float currentInterval = Mathf.Lerp(startInterval, endInterval, t * t);

            elapsed += currentInterval;
            yield return new WaitForSeconds(currentInterval);
        }

        // Chọn avatar và tên cuối cùng
        var finalAvatar = avatars[Random.Range(0, avatars.Length)].sprite;
        player.SetAvatar(finalAvatar);
    }*/

    #endregion


}


[Serializable]
public class PlayerInfo
{
    public string characterID;
    public string avatarID;
    public string frameID;
    public string backgroundID;
    public string name;

    public PlayerInfo(string characterID, string avatarID, string frameID, string backgroundID, string name)
    {
        this.characterID = characterID;
        this.avatarID = avatarID;
        this.frameID = frameID;
        this.backgroundID = backgroundID;
        this.name = name;
    }
}
