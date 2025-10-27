using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private UIButton diceBtn;
    [SerializeField] private Animation zoomAnimation;
    [SerializeField] private Animation rollAnimation;

    [SerializeField] private GameObject[] diceNumbers;

    public int DiceResult { get; private set; }

    public void SetActiveDiceButton(bool active = true, bool animate = true)
    {
        diceBtn.enabled = active;

        if (animate) zoomAnimation.Play();
        else
        {
            zoomAnimation.Stop();
            diceBtn.transform.localScale = Vector3.one;
        }
    }

    public void OnDiceClick()
    {
        AudioManager.Instance.PlaySound("Dice");

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
            AudioManager.Instance.PlaySound("Dice 6");
        }

        GameController.Instance.IsDoubleTurn = (DiceResult == 6);
        EventManager.SendSimpleEvent(Events.FINISH_DICE);
    }
}
