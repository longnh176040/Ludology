using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotDifficulty { 
    Easy, 
    Normal, 
    Hard,
    Random
}


public class BotController : MonoBehaviour, IPlayerController
{
    #region Member Variables

    private Player player;

    private float smartLevel;

    private float minDelayActionTime = 0.5f;
    private float maxDelayActionTime = 2f;

    #endregion

    #region Public Methods

    public void Init(Player player)
    {
        this.player = player;
    }

    public void SetDifficulty(BotDifficulty diff = BotDifficulty.Random)
    {
        switch (diff)
        {
            case BotDifficulty.Easy: smartLevel = 0.3f; break;
            case BotDifficulty.Normal: smartLevel = 0.6f; break;
            case BotDifficulty.Hard: smartLevel = 1.0f; break;
            case BotDifficulty.Random: 
                smartLevel = UnityEngine.Random.Range(0, 1f); break;
        }
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

        Piece chosenPiece;
        if (movablePieces.Count > 1)
        {
            List<(Piece piece, int score)> evaluations = new();

            foreach (var piece in movablePieces)
            {
                int score = EvaluatePiece(piece);
                evaluations.Add((piece, score));
            }

            //Sort descending by score
            evaluations.Sort((a, b) => b.score.CompareTo(a.score));

            //Randomized decision based on smartness
            chosenPiece = AIPickPiece(evaluations);
        }
        else chosenPiece = movablePieces[0];

        StartCoroutine(IE_AutoAction(() => {
            chosenPiece.OnPieceClick();
        }));
    }

    #endregion

    #region Private Methods

    private int EvaluatePiece(Piece piece)
    {
        int score = 0;
        var path = piece.Path;
        if (path == null || path.Length == 0)
            return score;

        //Evaluate destination first
        BoardPoint destionation = path[^1];

        var pieces = destionation.Pieces;
        if (pieces.Count > 0)
        {
            foreach (var p in pieces)
                //Can kick this opponent piece or block other opponent pieces
                score += p.TeamColor != piece.TeamColor? 50 : 30; 
        }
        //Get into house
        if (destionation is InHousePoint)
            score += 40;
        else if (destionation is ArrowPoint arr && arr.TeamColor == piece.TeamColor)
            score += 35;
        //Leave corner
        if (destionation is MovePoint moveDest && moveDest.IsStartPoint) 
            score += 30;

        //Evaluate the rest of the path
        if (path.Length > 1)
        {
            for (int i = path.Length - 2; i >= 0; i--) 
            {
                var point = path[i];
                if (point is InHousePoint or ArrowPoint)
                {
                    score += 10;
                    continue;
                }
                
                if (point is MovePoint movePoint)
                {
                    var ps = point.Pieces;
                    if (ps.Count > 0)
                    {
                        //Dangerous area: The nearer the enemies, the more point subtracted
                        foreach (var p in ps)
                            if (p.TeamColor != piece.TeamColor)
                                score -= (10 + (i + 1) * 10);
                    }
                }
            }
        }

        //The further the piece move, the more chances it will be chosen
        int pieceScoreOffset = piece.PieceScore / 2;

        return score + pieceScoreOffset;
    }

    private Piece AIPickPiece(List<(Piece piece, int score)> pieceList)
    {
        if (smartLevel <= 0.05f)
        {
            //No AI -> Totally random
            return pieceList[UnityEngine.Random.Range(0, pieceList.Count)].piece;
        }

        //Weighted randomness — higher smartLevel = higher chance to pick better move
        float totalWeight = 0f;
        List<float> weights = new();

        for (int i = 0; i < pieceList.Count; i++)
        {
            //Exponential bias toward better moves
            float weight = Mathf.Pow(smartLevel, i);
            weights.Add(weight);
            totalWeight += weight;
        }

        float r = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < pieceList.Count; i++)
        {
            cumulative += weights[i];
            if (r <= cumulative)
                return pieceList[i].piece;
        }

        return pieceList[^1].piece; // fallback
    }

    private IEnumerator IE_AutoAction(Action action)
    {
        yield return new WaitForSeconds(
            UnityEngine.Random.Range(minDelayActionTime, maxDelayActionTime));
        action?.Invoke();
    }

    #endregion

}
