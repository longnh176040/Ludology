using System.Collections;
using TMPro;
using UnityEngine;

public class MatchingPlayerFrame : PlayerFrame
{
    #region Inspector Variables

    [SerializeField] protected GameObject searchIconObj;

    #endregion

    #region Public Methods

    public void FindOpponent(float duration)
    {
        StartCoroutine(IE_FindOpponent(duration));
    }

    #endregion

    #region Private Methods

    private IEnumerator IE_FindOpponent(float duration)
    {
        ShowInfo(false);
        searchIconObj.SetActive(true);

        var opponentInfo = frameDataManager.RandomPlayerFrame(color);

        var avatarItem = frameDataManager.GetAvatarItemByIndex(color, opponentInfo.Item1);
        //var frameItem = frameDataManager.GetFrameItemByIndex(opponentInfo.Item2);
        //var backgroundItem = frameDataManager.GetBackgroundItemByIndex(opponentInfo.Item3);

        yield return new WaitForSeconds(duration);

        searchIconObj.SetActive(false);
        ShowInfo(true);
        SetAvatar(avatarItem.sprite);
        turnTxt.text = Utilities.RandomName();
        //SetFrame(frameItem.sprite);
        //SetBackground(backgroundItem.sprite);
    }

    #endregion
}
