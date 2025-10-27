using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public enum TeamColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW
}

public class GameController : Singleton<GameController>
{ 

    #region Inspector Variables

    [SerializeField] private Player[] players;
    [SerializeField] private Animation[] turnAnimations;

    #endregion

    #region Member Variables

    private TeamColor currentTurn;

    #endregion

    #region Properties

    public bool IsDoubleTurn { get; set; }

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        EventManager.Connect(Events.FINISH_DICE, SetMoveTurn);
        EventManager.Connect(Events.END_TURN, SwitchTurn);
    }

    private void OnDisable()
    {
        EventManager.Disconnect(Events.FINISH_DICE, SetMoveTurn);
        EventManager.Disconnect(Events.END_TURN, SwitchTurn);
    }

    private void Start()
    {
        Init();
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
        if (!IsDoubleTurn)
        {
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
        IsDoubleTurn = false;
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
