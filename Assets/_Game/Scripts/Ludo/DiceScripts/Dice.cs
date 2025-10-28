using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class Dice : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private UIButton diceBtn;
    [SerializeField] private Animation zoomAnimation;
    [SerializeField] private Animation rollAnimation;

    [SerializeField] private GameObject[] diceNumbers;

    #endregion

    #region Property 

    public int DiceResult { get; private set; }

    #endregion

    #region Member Variables

    [Inject] private AudioManager audioManager;
    [Inject] private GameController gameController;
    
    #endregion

    #region Public Methods 

    public void SetActiveDiceButton(bool active = true)
    {
        diceBtn.enabled = active;

        if (active) zoomAnimation.Play();
        else
        {
            zoomAnimation.Stop();
            diceBtn.transform.localScale = Vector3.one;
        }
    }

    public void OnDiceClick()
    {
        audioManager.PlaySound("Dice");

        diceBtn.enabled = false; 
        zoomAnimation.Stop();
        diceBtn.transform.localScale = Vector3.one;
        rollAnimation.Play();
        DiceResult = Random.Range(1, 7);
    }

    public void SetDiceResult(int diceResult)
    {
        DiceResult = diceResult;
    }

    public void OnDiceEnd()
    {
        for (int i = 0; i < diceNumbers.Length; i++)
            diceNumbers[i].SetActive(i == DiceResult - 1);

        if (DiceResult == 6)
        {
            audioManager.PlaySound("Dice 6");
        }

        gameController.IsDoubleTurn = (DiceResult == 6);
        EventManager.SendSimpleEvent(Events.FINISH_DICE);
    }

    #endregion
}
