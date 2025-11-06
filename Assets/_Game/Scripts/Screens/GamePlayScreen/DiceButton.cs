using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DiceButton : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private Image diceImg;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite boostSprite;
    [SerializeField] private Animator animator;

    [SerializeField] private UIButton btn;

    #endregion

    #region Member Variables

    [Inject] private GameController gameController;
    [Inject] private GameScreen gameScreen;

    #endregion

    #region Public Methods

    public void OnDiceButtonClick()
    {
        gameController.RollMainDice();
        gameScreen.HideControlPanel();
    }

    public void SetInteractable(bool interactable = true)
    {
        btn.SetInteractable(interactable);
        animator.Play(interactable ? "Idle" : "Default");
    }

    public void UseBoost(bool useBoost = true)
    {
        diceImg.sprite = useBoost? boostSprite : defaultSprite;
    }

    #endregion
}
