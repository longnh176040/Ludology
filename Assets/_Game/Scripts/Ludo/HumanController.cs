using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HumanController : MonoBehaviour, IPlayerController
{
    private Player player;
    [Inject] private SignalBus signalBus;

    public void Init(Player player)
    {
        this.player = player;
    }


    public void OnTurn()
    {
        signalBus.Fire(new HumanTurnSignal { });
    }
    public void OnMove(List<Piece> movablePieces) { }
}
