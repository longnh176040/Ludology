using System.Collections.Generic;

public interface IPlayerController
{
    void Init(Player player);
    void OnTurn();
    void OnMove(List<Piece> movablePieces);
}
