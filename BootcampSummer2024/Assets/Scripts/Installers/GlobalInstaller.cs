using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    [SerializeField] private SaveManagerConfig _saveConfig;
    private Logger _logger;

    public override void InstallBindings() {
        BindServices();
        BindInput();
    }

    private void BindServices() {
        _logger = new Logger();
        Container.Bind<Logger>().FromInstance(_logger).AsSingle();

        Container.Bind<PauseHandler>().AsSingle();
        Container.Bind<CoinCounter>().AsSingle();
        Container.Bind<SoundsLoader>().AsSingle();

        Container.BindInterfacesAndSelfTo<LevelProgressCounter>().AsSingle();

        BindSaveManager();
        BindSoundManager();
    }

    private void BindInput() {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
        else
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();

        Container.Bind<SwipeHandler>().AsSingle().NonLazy();
    }

    private void BindSaveManager() {
        if (_saveConfig.SavePath == "")
            _saveConfig.SetPath(Application.persistentDataPath);

        Container.BindInstance(_saveConfig).AsSingle();

        Container.Bind<SavesManager>().AsSingle();
        Container.Bind<PlayerProgressLoader>().AsSingle();
        Container.Bind<PlayerProgressManager>().AsSingle();
    }

    private void BindSoundManager() {
        Container.Bind<EnvironmentSoundConfig>().AsSingle();
        Container.Bind<PlayerSoundConfig>().AsSingle();
    }
}