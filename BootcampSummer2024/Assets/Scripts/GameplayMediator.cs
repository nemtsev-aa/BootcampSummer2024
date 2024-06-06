using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameplayMediator : MonoBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private Portal _portal;
    [SerializeField] private GameplayUI _gameplayUI;

    private PauseHandler _pauseHandler;
    private LevelProgressCounter _levelProgressCounter;

    [Inject]
    public void Construct(PauseHandler pauseHandler, LevelProgressCounter levelProgressCounter) {
        _pauseHandler = pauseHandler;
        
        _levelProgressCounter = levelProgressCounter;
        _levelProgressCounter.SetCompanents(_player.Move.transform, _portal.transform);
        
        _gameplayUI.LevelProgressBar.Init(_levelProgressCounter);
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
        Invoke(nameof(ReloadLevel), 1f);
    }

    private void OnPlayerPortalCollided() {
        _levelProgressCounter.Reset();

        Invoke(nameof(NextLevel), 1f);
    }

    private void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextLevel() {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCount <= nextLevelIndex)
            SceneManager.LoadScene(nextLevelIndex);
        else
            SceneManager.LoadScene(0);
    }
}
