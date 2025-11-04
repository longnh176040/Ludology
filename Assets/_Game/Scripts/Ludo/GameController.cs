using UnityEngine;
using Zenject;

public enum TeamColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW
}

public class GameController : MonoBehaviour
{ 

    #region Inspector Variables

    [SerializeField] private Player[] players;
    [SerializeField] private TurnPanel turnPanel;

    #endregion

    #region Member Variables

    [Inject] private DataManager dataManager;

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
    public void Init()
    {
        //Init data for main player
        PlayerInfoData playerInfoData = dataManager.playerInfoData;
        players[(int)playerInfoData.color].Init(playerInfoData.info);
        
        //Set red to be the first team
        currentTurn = TeamColor.RED;
    }

    public void AddPlayer(NewPlayerSignal newPlayerSignal)
    {
        int teamID = (int) newPlayerSignal.TeamColor;
        players[teamID].Init(newPlayerSignal.PlayerInfo);
    }  

    public Color GetColor(TeamColor teamColor, bool light = true)
    {
        return teamColor switch
        {
            TeamColor.RED => light
                ? new Color(1f, 99f / 255f, 99f / 255f)
                : new Color(1f, 47f / 255f, 47f / 255f),

            TeamColor.GREEN => light
                ? new Color(136f / 255f, 1f, 89f / 255f)
                : new Color(55f / 255f, 188f / 255f, 95f / 255f),

            TeamColor.BLUE => light
                ? new Color(71f / 255f, 188f / 255f, 1f)
                : new Color(50f / 255f, 146f / 255f, 210f / 255f),

            TeamColor.YELLOW => light
                ? new Color(1f, 239f / 255f, 68f / 255f)
                : new Color(223f / 255f, 189f / 255f, 25f / 255f),

            _ => Color.white
        };
    }

    #region Signal Functions
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

    #endregion

    #region Private Methods

    private void SwitchTurn()
    {
        if (!isDoubleTurn)
        {
            switch (currentTurn)
            {
                case TeamColor.RED:
                    currentTurn = TeamColor.BLUE;
                    break;
                case TeamColor.BLUE:
                    currentTurn = TeamColor.GREEN;
                    break;
                case TeamColor.GREEN:
                    currentTurn = TeamColor.YELLOW;
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

        var currentPlayer = players[(int)currentTurn];
        turnPanel.SetTurnPanel(currentPlayer.PlayerInfo, currentTurn);
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
