using UnityEngine;
using Zenject;

public class StartButton : MonoBehaviour, IButtonHandler
{
    [SerializeField] private UIButton button;

    [Inject] private ScreenManager screenManager;
    [Inject] private SignalBus signalBus;

    public void Initialize()
    {
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        screenManager.Show(Constant.GAME_SCREEN_ID);
        signalBus.Fire(new StartMainGameClickSignal { });
    }

    public void Dispose()
    {
        button.onClick.RemoveAllListeners();
    }
}
