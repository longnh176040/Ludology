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
    [SerializeField] protected TextMeshProUGUI turnTxt;

    #endregion

    #region Properties

    public TeamColor Color { get => color; set => color = value; }

    #endregion

    #region Member Variables

    [Inject] protected FrameDataManager frameDataManager;

    #endregion

    #region Public Methods

    public void SetPosition(Vector3 position)
    {
        rectTransform.anchoredPosition = position;
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

    public void SetPlayerTurn(bool isPlayer = false)
    {
        //TODO: change character image if the player purchased a new avatar

        turnTxt.text = isPlayer ? "YOU" : "PLAYER";
        //frameImage.color = isPlayer ? Color.red : Color.white;
    }

    #endregion

    #region Private Methods

    protected void ShowInfo(bool show = true)
    {
        turnTxt.gameObject.SetActive(show);
        characterImage.gameObject.SetActive(show);
    }

    #endregion

}
