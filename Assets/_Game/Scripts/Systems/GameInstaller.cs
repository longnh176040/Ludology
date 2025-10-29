using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AudioManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BoardManager>().FromComponentInHierarchy().AsSingle();

        //====== Install Signal ======
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<ExtendTurnSignal>();
        Container.DeclareSignal<FinishDiceSignal>();
        Container.DeclareSignal<SwitchTurnSignal>();
        Container.DeclareSignal<GameOver>();

        Container.BindSignal<ExtendTurnSignal>().ToMethod<GameController>(x => x.OnExtendTurn).FromResolve();
        Container.BindSignal<FinishDiceSignal>().ToMethod<GameController>(x => x.OnFinishDice).FromResolve();
        Container.BindSignal<SwitchTurnSignal>().ToMethod<GameController>(x => x.OnSwitchTurn).FromResolve();

    }
}