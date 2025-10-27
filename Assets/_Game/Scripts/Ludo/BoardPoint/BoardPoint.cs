using System.Collections.Generic;
using UnityEngine;

public class BoardPoint : MonoBehaviour
{
    protected RectTransform rectTrans;
    public RectTransform RectTrans => rectTrans;

    public List<Piece> Pieces => pieceInThis;
    public int NumPieceInside => pieceInThis.Count;

    protected List<Piece> pieceInThis = new(); //List of piece stay inside this board point

    protected void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    public void AddPiece(Piece piece)
    {
        if (!pieceInThis.Contains(piece))
        {
            pieceInThis.Add(piece);
        }
    }

    public void RemovePiece(Piece piece)
    {
        pieceInThis.Remove(piece);
    }
}
