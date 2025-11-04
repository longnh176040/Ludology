using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class MatchingPlayerFrame : PlayerFrame
{
    #region Inspector Variables

    [SerializeField] protected GameObject searchIconObj;

    #endregion

    #region Member Variables

    [Inject] private SignalBus signalBus;
    [Inject] private AudioManager audioManager;

    #endregion

    #region Public Methods

    public void FindOpponent(float duration)
    {
        StartCoroutine(IE_FindOpponent(duration));
    }

    public override void ShowPlayerInfo(PlayerInfo playerInfo = null)
    {
        base.ShowPlayerInfo(playerInfo);
        searchIconObj.SetActive(false);
        ShowInfo(true);
    }

    #endregion

    #region Private Methods

    private IEnumerator IE_FindOpponent(float duration)
    {
        ShowInfo(false);
        searchIconObj.SetActive(true);

        var opponentInfo = frameDataManager.RandomPlayerInfo(color);

        //Fire New Player Signal to add new player to the match
        signalBus.Fire(new NewPlayerSignal(color, opponentInfo));

        var avatarItem = frameDataManager.GetAvatarItemByID(opponentInfo.avatarID);
        var frameItem = frameDataManager.GetFrameItemByID(opponentInfo.frameID);
        var backgroundItem = frameDataManager.GetBackgroundItemByID(opponentInfo.backgroundID);

        yield return new WaitForSeconds(duration);

        /*searchIconObj.SetActive(false);
        ShowInfo(true);
        SetAvatar(avatarItem.sprite);
        SetFrame(frameItem.sprite);
        SetBackground(backgroundItem.sprite);
        nameTxt.text = opponentInfo.name;*/
        ShowPlayerInfo(opponentInfo);
        audioManager.PlaySound("Pop");
    }

    #endregion
}
