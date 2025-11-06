using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour, IPlayerController
{
    #region Member Variables

    private Player player;

    private float minDelayActionTime = 0.5f;
    private float maxDelayActionTime = 2f;

    #endregion

    #region Public Methods

    public void Init(Player player)
    {
        this.player = player;
    }

    public void OnTurn()
    {
        StartCoroutine(IE_AutoAction(() =>
        {
            player.RollDice();
        }));
    }

    public void OnMove(List<Piece> movablePieces)
    {
        if (movablePieces == null || movablePieces.Count == 0) return;

        //TODO: Replace with AI logic
        var chosen = movablePieces[UnityEngine.Random.Range(0, movablePieces.Count)];

        StartCoroutine(IE_AutoAction(() => { 
            chosen.OnPieceClick();
        }));
    }

    #endregion

    #region Private Methods

    private IEnumerator IE_AutoAction(Action action)
    {
        yield return new WaitForSeconds(
            UnityEngine.Random.Range(minDelayActionTime, maxDelayActionTime));
        action?.Invoke();
    }

    #endregion

}
