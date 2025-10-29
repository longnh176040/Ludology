using UnityEngine;
using Zenject;

public enum TeamColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}

public class GameController : MonoBehaviour
{ 

    #region Inspector Variables

    [SerializeField] private Player[] players;
    [SerializeField] private Animation[] turnAnimations;

    #endregion

    #region Member Variables

    [Inject] private AudioManager audioManager;

    private TeamColor currentTurn;
    private bool isDoubleTurn;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    #endregion

    #region Public Methods

    public Color GetColor(TeamColor teamColor)
    {
        switch (teamColor) 
        { 
            case TeamColor.RED:
                return Color.red;
            case TeamColor.GREEN:
                return Color.green;
            case TeamColor.BLUE:
                return Color.blue;
            case TeamColor.YELLOW:
                return Color.yellow;
            default: 
                return Color.white;
        }
    }

    public void OnExtendTurn()
    {
        isDoubleTurn = true;
    }

    public void OnFinishDice()
    {
        SetMoveTurn();
    }

    public void OnSwitchTurn()
    {
        SwitchTurn();
    }


    #endregion

    #region Private Methods

    private void Init()
    {
        foreach (var player in players)
        {
            player.Init();
        }

        currentTurn = TeamColor.RED;
        SetDiceTurn();
    }

    private void SwitchTurn()
    {
        if (!isDoubleTurn)
        {
            audioManager.PlaySound("Woosh");

            switch (currentTurn)
            {
                case TeamColor.RED:
                    currentTurn = TeamColor.BLUE;
                    break;
                case TeamColor.GREEN:
                    currentTurn = TeamColor.YELLOW;
                    break;
                case TeamColor.BLUE:
                    currentTurn = TeamColor.GREEN;
                    break;
                case TeamColor.YELLOW:
                    currentTurn = TeamColor.RED;
                    break;
            }
        }

        SetDiceTurn();
        isDoubleTurn = false;
    }

    private void SetDiceTurn()
    {
        foreach (var player in players)
        {
            player.ActiveTurn(player.TeamColor == currentTurn);
        }

        for (int i = 0; i < turnAnimations.Length; i++)
        {
            turnAnimations[i].gameObject.SetActive(i == (int)currentTurn);
        }
    }

    private void SetMoveTurn()
    {
        foreach (var player in players)
        {
            if (player.TeamColor == currentTurn)
            {
                player.CalculateMovePiece();
                return;
            }
        }
    }

    #endregion

}
