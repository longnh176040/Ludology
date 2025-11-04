using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerFrame : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] protected TeamColor color;
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image characterImage;
    [SerializeField] protected Image frameImage;
    [SerializeField] protected Image bgImg;
    [SerializeField] protected TextMeshProUGUI nameTxt;

    #endregion

    #region Properties

    public TeamColor Color { get => color; set => color = value; }

    #endregion

    #region Member Variables

    [Inject] protected FrameDataManager frameDataManager;
    [Inject] protected DataManager dataManager;

    #endregion

    #region Public Methods

    public void SetPosition(Vector3 position)
    {
        rectTransform.anchoredPosition = position;
    }

    public virtual void ShowPlayerInfo(PlayerInfo playerInfo = null)
    {
        var avatarID = playerInfo == null? dataManager.playerInfoData.info.avatarID : playerInfo.avatarID;
        var frameID = playerInfo == null ? dataManager.playerInfoData.info.frameID : playerInfo.frameID;
        var bgID = playerInfo == null ? dataManager.playerInfoData.info.backgroundID : playerInfo.backgroundID;
        string nameTxt = playerInfo == null? dataManager.playerInfoData.info.name : playerInfo.name;

        AvatarItemSO avatarItem = frameDataManager.GetAvatarItemByID(avatarID);
        SetAvatar(avatarItem.sprite);
        FrameItemSO frameItem = frameDataManager.GetFrameItemByID(frameID);
        SetFrame(frameItem.sprite);
        BackgroundItemSO backgroundItem = frameDataManager.GetBackgroundItemByID(bgID);
        SetBackground(backgroundItem.sprite);
        SetPlayerName(nameTxt);
    }

    public void SetAvatar(Sprite sprite) 
    {
        characterImage.sprite = sprite;
    }

    public void SetFrame(Sprite sprite)
    {
        frameImage.sprite = sprite;
    }

    public void SetBackground(Sprite sprite)
    {
        bgImg.sprite = sprite;
    }

    public void SetPlayerName(string name)
    {
        nameTxt.text = name;
    }

    #endregion

    #region Private Methods

    protected void ShowInfo(bool show = true)
    {
        nameTxt.gameObject.SetActive(show);
        characterImage.gameObject.SetActive(show);
    }

    #endregion

}

