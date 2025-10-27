using UnityEngine;

public class DiceAnimationEvent : MonoBehaviour
{
    [SerializeField] private Dice dice;
    public void OnDiceEnd()
    {
        dice.OnDiceEnd();
    }
}
