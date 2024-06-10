using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public enum Switchovers {
    MainMenu,
    CurrentLevel,
    NextLevel,
}

public class GameplayMediator : MonoBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private Portal _portal;


    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    [SerializeField] private PlayerSFXManager _playerSFXManager;

    private PauseHandler _pauseHandler;
    private CoinCounter _coinCounter;
    private PlayerProgressManager _playerProgressManager;
    private LevelProgressCounter _levelProgressCounter;
    private bool _isPause;
    private Switchovers _switchover;
    private UIManager _uIManager;

    [Inject]
    public void Construct(PauseHandler pauseHandler, LevelProgressCounter levelProgressCounter,
                          CoinCounter coinCounter, PlayerProgressManager playerProgressManager) {

        _pauseHandler = pauseHandler;
        _coinCounter = coinCounter;

        _playerProgressManager = playerProgressManager;

        _levelProgressCounter = levelProgressCounter;
        _levelProgressCounter.SetCompanents(_player.Move.transform, _portal.transform);

        _gameplayUI.LevelProgressBar.Init(_levelProgressCounter);

        _environmentSoundManager.Init();
         _playerSFXManager.Init(_player);
    }
    
    private void Start() {
        _environmentSoundManager.PlaySound(MusicType.Gameplay);
        
    }

    private void OnEnable() {
        _player.Interaction.ObstacleCollided += OnPlayerObstacleCollided;
        _player.Interaction.PortalCollided += OnPlayerPortalCollided;

        _gameplayUI.MainMenuButtonClicked += OnMainMenuButtonClicked;
        _gameplayUI.PauseButtonClicked += OnPauseButtonClicked;
    }

    private void OnDisable() {
        _player.Interaction.ObstacleCollided -= OnPlayerObstacleCollided;
        _player.Interaction.PortalCollided -= OnPlayerPortalCollided;

        _gameplayUI.MainMenuButtonClicked -= OnMainMenuButtonClicked;
        _gameplayUI.PauseButtonClicked -= OnPauseButtonClicked;
    }

    public void ResetPlayerProgress() => _playerProgressManager.ResetAll();

    public void LevelPreparation(int levelIndex) => SceneManager.LoadScene(levelIndex);

    public void Reset() {
        _coinCounter.RemoveCoinsFromLevel();
        _levelProgressCounter.Reset();
    }

    public void SetPause(bool value) {
        _isPause = value;
        _pauseHandler.SetPause(_isPause);
    }
    
    public void FinishGameplay(Switchovers switchover) {
        _switchover = switchover;

        int newPercent = 0;

        if (switchover == Switchovers.NextLevel)
            newPercent = 100;
        else
            newPercent = _levelProgressCounter.CurrentPercent;

        var newCoinCount = _coinCounter.GetPointsFromLevel();
        
        SaveLevelProgress(newCoinCount, newPercent);
        Reset();
        
        MakeTransition();
    }

    private void OnPauseButtonClicked() {
        _isPause = !_isPause;
        SetPause(_isPause);
    } 
    
    private void OnMainMenuButtonClicked() => FinishGameplay(Switchovers.MainMenu);

    private void OnPlayerObstacleCollided() => FinishGameplay(Switchovers.CurrentLevel);

    private void OnPlayerPortalCollided() => FinishGameplay(Switchovers.NextLevel);
 
    private void SaveLevelProgress(int newCoinCount, int newPercent) {
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        var oldCoinCount = _playerProgressManager.GetCoinsCountByLevelIndex(currentLevelIndex);
        var oldPercent = _playerProgressManager.GetPercentByLevelIndex(currentLevelIndex);

        if (newCoinCount > oldCoinCount || newPercent > oldPercent) {
            var saveCoinCount = Mathf.Max(newCoinCount, oldCoinCount);
            var savePercent = Mathf.Max(newPercent, oldPercent);

            var currentLevelProgressData = new LevelProgressData(currentLevelIndex, saveCoinCount, savePercent);
            _playerProgressManager.UpdateProgressByLevel(currentLevelProgressData);
            _playerProgressManager.SaveCurrentProgress();

            _playerProgressManager.UpdateScore();
            _playerProgressManager.SavePlayerScore();
        }
    }

    private void MakeTransition() {

        if (_switchover == Switchovers.MainMenu)
            Invoke(nameof(ShowMainMenu), 1f);

        if (_switchover == Switchovers.CurrentLevel)
            Invoke(nameof(RestartLevel), 1f);

        if (_switchover == Switchovers.NextLevel) {
            _gameplayUI.LevelProgressBar.ShowLevelIsOver();
            _levelProgressCounter.Reset();
            _coinCounter.RemoveCoinsFromLevel();

            Invoke(nameof(NextLevel), 1f);
        }
    }

    private void ShowMainMenu() => _uIManager.ShowMainMenuDialog();

    private async UniTask RestartLevel() {
        await UniTask.Delay(1000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextLevel() {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextLevelIndex);
        else
            SceneManager.LoadScene(0);
    }
}
