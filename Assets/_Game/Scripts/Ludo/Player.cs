using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private TeamColor teamColor;
    [SerializeField] private GameObject turnBorder_Obj;
    [SerializeField] private RectTransform[] pieceTransList;
    [SerializeField] private Piece[] pieceList;
    [SerializeField] private Dice dice;
    [SerializeField] private BoardPoint startPoint;

    #endregion

    #region Properties

    public TeamColor TeamColor => teamColor;
    public PlayerInfo PlayerInfo => playerInfo;
    public BoardPoint StartPoint => startPoint;
    public int PieceInHouse { get; private set; }

    #endregion

    #region Member Variables

    [Inject] private BoardManager boardManager;
    [Inject] private SignalBus signalBus;

    private PlayerInfo playerInfo;

    private bool allPieceInCorner = true;

    private IPlayerController controller;

    #endregion

    #region Public Methods

    public void Init(PlayerInfo playerInfo, IPlayerController controller)
    {
        this.playerInfo = playerInfo;
        this.controller = controller;
        this.controller.Init(this);

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

        if (active) controller?.OnTurn();
    }

    public void RollDice()
    {
        dice.OnDiceClick();
    }

    public void ActivePieceSelection(bool active = true, bool interactable = true)
    {
        foreach (var piece in pieceList)
        {
            piece.ActiveSelection(active, interactable);
        }
    }

    public void CalculateMovePiece()
    {
        int diceNumber = dice.DiceResult;
        var movablePieces = new List<Piece>();

        if (allPieceInCorner) //No piece started
        {
            if (diceNumber != 6)
            {
                signalBus.Fire(new SwitchTurnSignal { });
                return;
            }
            else
            {
                foreach (var piece in pieceList)
                {
                    piece.ActiveSelection();
                    piece.SetMoveAction(new BoardPoint[] { startPoint });
                    movablePieces.Add(piece);
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
                        movablePieces.Add(piece);
                        isAllPieceCantMove = false;
                    }
                }
                else
                {
                    BoardPoint[] path = boardManager.CreatePath(teamColor, piece.CurrentPoint, diceNumber);
                    if (path != null)
                    {
                        piece.ActiveSelection();
                        piece.SetMoveAction(path);
                        movablePieces.Add(piece);
                        isAllPieceCantMove = false;
                    }
                }
            }
            if (isAllPieceCantMove)
            {
                signalBus.Fire(new SwitchTurnSignal { });
                return;
            }
        }

        controller?.OnMove(movablePieces);
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

        signalBus.Fire(new GameOver { });
        Debug.Log(gameObject.name + " WIN GAME!!!");
    }

    #endregion
}
