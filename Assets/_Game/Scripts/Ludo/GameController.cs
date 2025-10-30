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
                return new Color(1f, 99f/255f, 99f / 255f, 1f);
            case TeamColor.GREEN:
                return new Color(136f / 255f, 1f, 89f / 255f, 1f);
            case TeamColor.BLUE:
                return new Color(71f / 255f, 188f / 255f, 1f, 1f);
            case TeamColor.YELLOW:
                return new Color(1f, 239f / 255f, 68f / 255f, 1f);                
        }
        return Color.white;
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

    public void OnMainGameStart()
    {
        SetDiceTurn();
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
