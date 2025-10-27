using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject selectVFX_Obj;
    [SerializeField] private UIButton selectBtn;
    [SerializeField] private RectTransform rectTrans;

    [Header("TEAM")]
    [SerializeField] private Player player;

    public int Id { get; private set; }
    public RectTransform RectTrans => rectTrans;
    public TeamColor TeamColor => player.TeamColor;
    public bool IsInCorner { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsActive { get; private set; }

    public BoardPoint CurrentPoint => currentPoint;

    private BoardPoint currentPoint;
    private BoardPoint[] path;

    private Action moveAction;

    private float inCornerScaleOffset = 1.5f;
    private float inBoardScaleOffset = 1f;
    private float inSharedBoardScaleOffset = 0.75f;


    public void Init(int id)
    {
        Id = id;
        IsInCorner = true;
        IsFinished = false;
        rectTrans.localScale = Vector3.one * inCornerScaleOffset;
        ActiveSelection(false, false);
        currentPoint = null;
    }

    public void ActiveSelection(bool active = true, bool interactable = true)
    {
        IsActive = active;
        selectVFX_Obj.SetActive(interactable);
        selectBtn.interactable = interactable;
    }

    public void OnPieceClick()
    {
        player.ActivePieceSelection(false, false);
        //player.SetLastMovedPiece(this);

        if (path != null)
        {
            Move(path);
        }
    }

    public void SetMoveAction(BoardPoint[] path)
    {
        this.path = path;

        /*foreach (BoardPoint p in path) { 
            Debug.Log(p.name) ;
        }*/
    }


    private void Move(BoardPoint[] path)
    {
        if (IsInCorner) IsInCorner = false;
        rectTrans.localScale = Vector3.one * inBoardScaleOffset;
        rectTrans.anchorMin = rectTrans.anchorMax = new Vector2(.5f, .5f);

        // Remove from current point
        if (currentPoint != null)
        {
            currentPoint.RemovePiece(this);
            if (currentPoint.NumPieceInside == 1)
            {
                currentPoint.Pieces[0].RectTrans.localScale = Vector3.one * inBoardScaleOffset;
                MovePoint curMovePoint = (MovePoint)currentPoint;
                curMovePoint.IsBlocked = false;
            }
        }

        // Start step-by-step movement coroutine
        StartCoroutine(MoveStepByStep(path));
    }

    private IEnumerator MoveStepByStep(BoardPoint[] path)
    {
        float moveDuration = 0.2f; // Time to move between points (adjust as needed)
        float scaleOffset = 1.25f;
        Vector2 endPos = new Vector2 (20f, -20f);

        for (int i = 0; i < path.Length; i++)
        {
            Transform targetParent = path[i].RectTrans;        
        
            // Snap to final position
            rectTrans.SetParent(targetParent);
            AudioManager.Instance.PlaySound("Step");

            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                if (elapsedTime < moveDuration / 2f) 
                {
                    rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, Vector3.one * scaleOffset, elapsedTime / moveDuration);
                }
                else
                {
                    rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, Vector3.one, elapsedTime / moveDuration);
                }

                rectTrans.anchoredPosition = Vector3.Lerp(rectTrans.anchoredPosition, endPos, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTrans.anchoredPosition = endPos;

            // Handle arrival logic at final point
            if (i == path.Length - 1)
            {
                currentPoint = path[i];

                if (currentPoint is InHousePoint) // Go in house
                {
                    HandleInHousePoint(path[i]);
                }
                else // Still on board
                {
                    HandleMovePoint(path[i]);
                }
            }
        
            yield return null; // Small delay between steps
        }
    
        path = null;
        EventManager.SendSimpleEvent(Events.END_TURN);
    }

    private void HandleInHousePoint(BoardPoint point)
    {
        point.AddPiece(this);
        if (point.NumPieceInside > 1)
        {
            ScalePieces(point.Pieces, inSharedBoardScaleOffset);
        }

        InHousePoint ihp = (InHousePoint)currentPoint;
        if (ihp.IsFinal)
        {
            IsFinished = true;
            player.CheckWin();
        }
    }

    private void HandleMovePoint(BoardPoint point)
    {
        MovePoint curMovePoint = (MovePoint)currentPoint;
    
        if (curMovePoint.IsStartPoint)
        {
            point.AddPiece(this);
            if (point.NumPieceInside > 1)
            {
                ScalePieces(point.Pieces, inSharedBoardScaleOffset);
            }
        }
        else
        {
            if (point.NumPieceInside > 0)
            {
                HandlePieceCollisions(point);
            }
            else
            {
                point.AddPiece(this);
            }
        }
    }

    private void HandlePieceCollisions(BoardPoint point)
    {
        foreach (Piece piece in point.Pieces.ToList())
        {
            if (piece.TeamColor != TeamColor)
            {
                // Kick enemy
                MovePoint curEnemyPoint = (MovePoint)piece.currentPoint;
                MovePoint targetEnemyPoint = (MovePoint)piece.player.StartPoint;
                MovePoint[] returnPath = BoardManager.Instance.FindPathBetweenMovePoints(curEnemyPoint.ID, targetEnemyPoint.ID);
            
                if (returnPath != null)
                {
                    piece.GetKicked(returnPath);
                    GameController.Instance.IsDoubleTurn = true;
                }
                else
                {
                    Debug.LogError("Cannot find return path");
                }
            }
            else
            {
                piece.RectTrans.localScale = Vector3.one * inSharedBoardScaleOffset;
            }
        }
    
        point.AddPiece(this);
        if (point.NumPieceInside > 1)
        {
            rectTrans.localScale = Vector3.one * inSharedBoardScaleOffset;
            ((MovePoint)currentPoint).IsBlocked = true;
        }
    }

    private void ScalePieces(List<Piece> pieces, float scale)
    {
        foreach (Piece piece in pieces)
        {
            piece.RectTrans.localScale = Vector3.one * scale;
        }
    }

    private void GetKicked(BoardPoint[] path)
    {
        currentPoint.RemovePiece(this);
        if (currentPoint.NumPieceInside == 1)
        {
            currentPoint.Pieces[0].RectTrans.localScale = Vector3.one * inBoardScaleOffset;
        }
        currentPoint = null;

        for (int i = 0; i < path.Length; i++)
        {
            rectTrans.SetParent(path[i].RectTrans);
            rectTrans.anchoredPosition = Vector3.zero;
        }

        AudioManager.Instance.PlaySound("Kick");

        IsInCorner = true;
        rectTrans.localScale = Vector3.one * inCornerScaleOffset;
        rectTrans.SetParent(player.GetPieceCornerTrans(Id));
        rectTrans.anchorMin = rectTrans.anchorMax = new Vector2(.5f, .5f);
        rectTrans.anchoredPosition = Vector3.zero;
        path = null;
        player.CheckAreAllPiecesInCorner();
    }
}
