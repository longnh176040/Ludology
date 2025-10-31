using System.Collections;
using UnityEngine;
using Zenject.SpaceFighter;

public class FrameDataManager : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private PlayerFrameDataSO playerFrameData;

    #endregion

    #region Public Methods

    public (int, int, int) RandomPlayerFrame(TeamColor color)
    {
        AvatarItemSO[] avatars = GetAvatarListByColor(color);
        if (avatars == null) Debug.LogError("Avatar data not found!");

        int avatarID = Random.Range(0, avatars.Length);
        int frameID = Random.Range(0, playerFrameData.frameItems.Length);
        int backgroundID = Random.Range(0, playerFrameData.backgroundItems.Length);

        return (avatarID, frameID, backgroundID);
    }

    public AvatarItemSO GetAvatarItemByIndex(TeamColor color, int idx = 0)
    {
        return GetAvatarListByColor(color)[idx];
    }

    public BackgroundItemSO GetBackgroundItemByIndex(int idx = 0)
    {
        return playerFrameData.backgroundItems[idx];
    }

    public FrameItemSO GetFrameItemByIndex(int idx = 0)
    {
        return playerFrameData.frameItems[idx];
    }

    public void RandomAvatarRoutine(PlayerFrame player)
    {
        StartCoroutine(IERandomPlayerFrame(player));
    }

    #endregion

    #region Private Methods

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

    private IEnumerator IERandomPlayerFrame(PlayerFrame player, float minDuration = 0.5f, float maxDuration = 2f)
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
    }

    #endregion


}
