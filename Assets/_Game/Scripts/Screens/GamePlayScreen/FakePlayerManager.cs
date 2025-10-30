using System.Collections;
using UnityEngine;

public class FakePlayerManager : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private PlayerFrameDataSO playerFrameData;

    #endregion

    #region Member Variables

    public float minDuration = 0.5f;
    public float maxDuration = 2f;

    #endregion

    #region Public Methods

    public void RandomAvatarRoutine(PlayerFrame player)
    {
        StartCoroutine(IERandomPlayerFrame(player));
    }

    #endregion

    #region Private Methods

    private IEnumerator IERandomPlayerFrame(PlayerFrame player)
    {
        float duration = Random.Range(minDuration, maxDuration);
        float elapsed = 0f;
        float startInterval = 0.05f; // init spd
        float endInterval = 0.3f;

        // Random avatars
        Sprite[] avatars = null;
        switch (player.Color)
        {
            case TeamColor.RED:
                avatars = playerFrameData.redAvatars;
                break;
            case TeamColor.GREEN:
                avatars = playerFrameData.greenAvatars;
                break;
            case TeamColor.BLUE:
                avatars = playerFrameData.blueAvatars;
                break;
            case TeamColor.YELLOW:
                avatars = playerFrameData.yellowAvatars;
                break;
        }
        if (avatars == null) Debug.LogError("Avatar data not found!");

        while (elapsed < duration)
        {
            Sprite tempAvatar = avatars[Random.Range(0, avatars.Length)];
            player.SetAvatar(tempAvatar);

            float t = elapsed / duration;
            float currentInterval = Mathf.Lerp(startInterval, endInterval, t * t);

            elapsed += currentInterval;
            yield return new WaitForSeconds(currentInterval);
        }

        // Chọn avatar và tên cuối cùng
        var finalAvatar = avatars[Random.Range(0, avatars.Length)];
        player.SetAvatar(finalAvatar);
    }

    #endregion


}
