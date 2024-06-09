using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameplayMediator : MonoBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private Portal _portal;
    [SerializeField] private GameplayUI _gameplayUI;

    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    [SerializeField] private PlayerSFXManager _playerSFXManager;

    private PauseHandler _pauseHandler;
    private CoinCounter _coinCounter;
    private PlayerProgressManager _playerProgressManager;
    private LevelProgressCounter _levelProgressCounter;

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
        //_playerProgressManager.ResetAllData();
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

    private void OnPauseButtonClicked() {
        bool isPaused = _pauseHandler.IsPaused ? false : true;

        _pauseHandler.SetPause(isPaused);
    }

    private void OnMainMenuButtonClicked() {
        // Получить текущий прогресс
        // Сохранить текущий прогресс
        SceneManager.LoadScene(0);
    }

    private void OnPlayerObstacleCollided() {
        var newCoinCount = _coinCounter.GetPointsFromLevel();
        var newPercent = _levelProgressCounter.CurrentPercent;
        SaveLevelProgress(newCoinCount, newPercent);

        Invoke(nameof(ReloadLevel), 1f);
    }

    private void OnPlayerPortalCollided() {
        var newCoinCount = _coinCounter.GetPointsFromLevel();
        SaveLevelProgress(newCoinCount, 100);

        _gameplayUI.LevelProgressBar.ShowLevelIsOver();
        _levelProgressCounter.Reset();
        _coinCounter.RemoveCoinsFromLevel();

        Invoke(nameof(NextLevel), 1f);
    }

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

    private void ReloadLevel() {
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
