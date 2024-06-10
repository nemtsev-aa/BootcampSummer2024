using System;

public class GameDialog : Dialog {
    public static event Action MainMenuClicked;
    public static event Action<bool> PauseClicked;
    public static event Action<bool> LearningClicked;

    private GameplayNavigationPanel _gameplayNavigationPanel;
    private LevelInfoPanel _levelInfoPanel;

    public override void InitializationPanels() {
        _gameplayNavigationPanel = GetPanelByType<GameplayNavigationPanel>();
        _gameplayNavigationPanel.Init();

        _levelInfoPanel = GetPanelByType<LevelInfoPanel>();
        _levelInfoPanel.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _gameplayNavigationPanel.MainMenuButtonClicked += OnMainMenuButtonClicked;
        _gameplayNavigationPanel.PauseButtonClicked += OnPauseButtonClicked;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _gameplayNavigationPanel.MainMenuButtonClicked -= OnMainMenuButtonClicked;
        _gameplayNavigationPanel.PauseButtonClicked -= OnPauseButtonClicked;
    }

    private void OnPauseButtonClicked(bool isPaused) => PauseClicked?.Invoke(isPaused);

    private void OnMainMenuButtonClicked() => MainMenuClicked?.Invoke();

}
