using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerFrame : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private TeamColor color;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image characterImage;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image bgImg;
    [SerializeField] private TextMeshProUGUI turnTxt;

    #endregion

    #region Properties

    public TeamColor Color { get => color; set => color = value; }

    #endregion

    #region Member Variables

    [Inject] private FakePlayerManager fakePlayerManager;

    #endregion

    #region Public Methods

    public void RandomFakePlayer()
    {
        fakePlayerManager.RandomAvatarRoutine(this);
    }

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

}
