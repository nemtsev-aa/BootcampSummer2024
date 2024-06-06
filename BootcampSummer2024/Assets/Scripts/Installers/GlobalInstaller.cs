using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {

    public override void InstallBindings() {
        BindServices();
        BindInput();
    }

    private void BindServices() {  
        Container.Bind<PauseHandler>().AsSingle();
        Container.Bind<CoinCounter>().AsSingle();

        LevelProgressCounter progressCounter = new LevelProgressCounter();
        Container.BindInstance(progressCounter).AsSingle();
        Container.BindInterfacesAndSelfTo<ITickable>().FromInstance(progressCounter).AsSingle();
    }

    private void BindInput() {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
        else
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();

        Container.Bind<SwipeHandler>().AsSingle().NonLazy();
    }
}