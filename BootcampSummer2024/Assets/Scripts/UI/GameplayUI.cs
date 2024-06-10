using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour {
    public event Action MainMenuButtonClicked;
    public event Action PauseButtonClicked;

    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private LevelProgressBar _progressBar;

    public LevelProgressBar LevelProgressBar => _progressBar;
    
    private void OnEnable() {
        _mainMenuButton.onClick.AddListener(ClickMainMenuButton);
        _pauseButton.onClick.AddListener(ClickPauseButton);
    }

    private void OnDisable() {
        _mainMenuButton.onClick.RemoveListener(ClickMainMenuButton);
        _pauseButton.onClick.RemoveListener(ClickPauseButton);
    }

    private void ClickPauseButton() => PauseButtonClicked?.Invoke();
    
    private void ClickMainMenuButton() => MainMenuButtonClicked?.Invoke();
}
