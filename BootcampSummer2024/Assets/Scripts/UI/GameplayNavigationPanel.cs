using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayNavigationPanel : UIPanel {
    public event Action MainMenuButtonClicked;
    public event Action<bool> PauseButtonClicked;

    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _pauseButton;

    private bool _isPaused = false;

    public void Init() {
        AddListeners();
    }

    public override void AddListeners() {
        _mainMenuButton.onClick.AddListener(ClickMainMenuButton);
        _pauseButton.onClick.AddListener(ClickPauseButton);
    }

    public override void RemoveListeners() {
        _mainMenuButton.onClick.RemoveListener(ClickMainMenuButton);
        _pauseButton.onClick.RemoveListener(ClickPauseButton);
    }

    private void ClickPauseButton() => PauseButtonClicked?.Invoke(!_isPaused);

    private void ClickMainMenuButton() => MainMenuButtonClicked?.Invoke();
}
