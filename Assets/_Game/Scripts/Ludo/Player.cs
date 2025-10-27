using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TeamColor teamColor;
    [SerializeField] private GameObject turnBorder_Obj;
    [SerializeField] private RectTransform[] pieceTransList;
    [SerializeField] private Piece[] pieceList;
    [SerializeField] private Dice dice;
    [SerializeField] private BoardPoint startPoint;

    public TeamColor TeamColor => teamColor;
    public BoardPoint StartPoint => startPoint;
    public int PieceInHouse { get; private set; }
    private bool allPieceInCorner = true;

    public void Init()
    {
        for (int i = 0; i < pieceList.Length; i++)
        {
            pieceList[i].Init(i);
        }

        allPieceInCorner = true;
        ActiveTurn(false);
    }

    public void ActiveTurn(bool active = true)
    {
        turnBorder_Obj.SetActive(active);
        dice.SetActiveDiceButton(active);
    }

    public void ActivePieceSelection(bool active = true, bool interactable = true)
    {
        foreach (var piece in pieceList)
        {
            piece.ActiveSelection(active);
        }
    }

    public void CalculateMovePiece()
    {
        int diceNumber = dice.DiceResult;

        if (allPieceInCorner) //No piece started
        {
            if (diceNumber != 6)
            {
                EventManager.SendSimpleEvent(Events.END_TURN);
            }
            else
            {
                foreach (var piece in pieceList)
                {
                    piece.ActiveSelection();
                    piece.SetMoveAction(new BoardPoint[] { startPoint });
                }
                allPieceInCorner = false;
            }
        }
        else
        {
            bool isAllPieceCantMove = true;
            foreach (var piece in pieceList)
            {
                if (piece.IsInCorner)
                {
                    if (diceNumber == 6)
                    {
                        piece.ActiveSelection();
                        piece.SetMoveAction(new BoardPoint[] { startPoint });
                        isAllPieceCantMove = false;
                    }
                }
                else
                {
                    BoardPoint[] path = BoardManager.Instance.CreatePath(teamColor, piece.CurrentPoint, diceNumber);
                    if (path != null)
                    {
                        piece.SetMoveAction(path);
                        piece.ActiveSelection();
                        isAllPieceCantMove = false;
                    }
                }
            }
            if (isAllPieceCantMove) EventManager.SendSimpleEvent(Events.END_TURN);
        }
    }

    public RectTransform GetPieceCornerTrans(int id)
    {
        if (id >= pieceTransList.Length) return null;
        return pieceTransList[id];
    }

    public void CheckAreAllPiecesInCorner()
    {
        foreach (var item in pieceList)
        {
            if (!item.IsInCorner)
            {
                allPieceInCorner = false;
                return;
            }
        }
        allPieceInCorner = true;
    }

    public void CheckWin()
    {
        foreach (var item in pieceList)
        {
            if (!item.IsFinished)
            {
                return;
            }
        }

        EventManager.SendSimpleEvent(Events.WIN_GAME);
        Debug.Log(gameObject.name + " WIN GAME!!!");
    }
}
