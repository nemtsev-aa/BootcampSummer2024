using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private GameplayMediator _gameplayMediator;

    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    [SerializeField] private PlayerSFXManager _playerSFXManager;

    private Logger _logger;
    private PlayerProgressManager _playerProgressManager;
    private SoundsLoader _soundsLoader;
    private List<SquadData> _squadsData;

    [Inject]
    public void Construct(Logger logger, PlayerProgressManager playerProgressManager, SoundsLoader soundsLoader) {
        _logger = logger;

        _playerProgressManager = playerProgressManager;
        _soundsLoader = soundsLoader;
    }

    private async void Start() {
        await Init();
    }

    private async UniTask Init() {
        _logger.Log("MenuEntryPoint Init");

        await StartLoading();
        FinishLoading();

        _logger.Log("MenuEntryPoint Complited");
    }

    private async UniTask StartLoading() {
        _uIManager.ShowLoadingDialog();

        await PlayerProgressLoading();
        await SoundsLoading();
    }

    private void FinishLoading() {
        _uIManager.Init(_gameplayMediator);
        _uIManager.ShowMainMenuDialog();
    } 
    
    #region PLAYERDATA LOADING
    private async UniTask PlayerProgressLoading() {
        await _playerProgressManager.GetCurrentProgress();
        await _playerProgressManager.GetCurrentScore();
    }

    #endregion

    #region SOUNDS LOADING
    private async UniTask<bool> SoundsLoading() {
        _logger.Log("Sounds Loading...");

        bool envSound = await TryEnvironmentSoundManagerInit();
        bool sfx = await TryPlayerSFXManagerInit();

        if (envSound == true && sfx == true) {
            _logger.Log("Sounds Loading Complited");
            return true;
        }
        else {
            _logger.Log("Sounds Loading Not Complited");
            return false;
        }
    }

    private async UniTask<bool> TryEnvironmentSoundManagerInit() {
        await _soundsLoader.LoadAsset(_environmentSoundManager.SoundConfig.ClipUrl[0], OnUIAudioClipLoaded);

        List<AudioClip> sounds = await _soundsLoader.LoadAssets(new List<string>() {
            _environmentSoundManager.SoundConfig.ClipUrl[1],
            _environmentSoundManager.SoundConfig.ClipUrl[2],
            }
        );

        if (sounds != null) {
            _environmentSoundManager.SoundConfig.SetAudioClips(sounds[0], sounds[1]);
            _logger.Log($"EnvironmentSoundManagerInit Complited: {sounds.Count}");

            return true;
        }

        _logger.Log($"EnvironmentSoundManagerInit Not Complited");
        return false;
    }

    private void OnUIAudioClipLoaded(AudioClip clip) {
        _environmentSoundManager.SoundConfig.SetUIAudioClip(clip);

        _environmentSoundManager.Init();
        _environmentSoundManager.PlaySound(MusicType.UI);
    }

    private async UniTask<bool> TryPlayerSFXManagerInit() {
        List<AudioClip> sounds = await _soundsLoader.LoadAssets(_playerSFXManager.SoundConfig.ClipUrl);

        if (sounds != null) {
            _playerSFXManager.SoundConfig.SetAudioClips(sounds[0], sounds[1], sounds[2], sounds[3], sounds[4]);
            _logger.Log($"PlayerSFXManagerInit Complited: {sounds.Count}");
            return true;
        }

        _logger.Log($"PlayerSFXManagerInit Not Complited");
        return false;
    }

    #endregion

    private void OnSquadIndexSelected(int index) {
        _playerProgressManager.SetSquadIndex(index);

        //_squadCardsPanel.transform.parent.gameObject.SetActive(false);

        //_levelCardsPanel.UpdateCards(_playerProgressManager.GetLevelsProgressData());
        //_levelCardsPanel.transform.parent.gameObject.SetActive(true);
    }

    private void OnLevelIndexSelected(int index) {
        SceneManager.LoadScene(index);
    }
}
