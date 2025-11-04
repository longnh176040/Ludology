
#region UI Signal 

using UnityEngine.UIElements;
using UnityEngine;

public struct StartMainGameClickSignal { }

#endregion

#region Ingame Signal

public struct StartMainGameSignal { }

public class NewPlayerSignal {
    public TeamColor TeamColor {  get; private set; }
    public PlayerInfo PlayerInfo { get; private set; }

    public NewPlayerSignal(TeamColor teamColor, PlayerInfo playerInfo)
    {
        TeamColor = teamColor;
        PlayerInfo = playerInfo;   
    }
}

public struct InitNewGameSignal { }

public struct ExtendTurnSignal { }

public struct FinishDiceSignal { }

public struct SwitchTurnSignal { }

public struct GameOver { }

#endregion

#region System Signal

public struct DataLoaded { }

#endregion