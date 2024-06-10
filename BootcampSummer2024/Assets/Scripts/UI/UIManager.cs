using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour, IDisposable {
    [SerializeField] private RectTransform _dialogsParent;

    private DialogFactory _factory;
    private List<Dialog> _dialogs;

    private GameplayMediator _gameplayMediator;

    public DialogSwitcher DialogSwitcher { get; private set; }

    [Inject]
    public void Constuct(DialogFactory factory) {
        _factory = factory;
        _factory.SetDialogsParent(_dialogsParent);

        _dialogs = new List<Dialog>();
    }

    public void Init(GameplayMediator gameplayMediator) {
        _gameplayMediator = gameplayMediator;

        DialogSwitcher = new DialogSwitcher(this);

        AddListener();
    }

    public void ShowMainMenuDialog() => DialogSwitcher.ShowDialog<MainMenuDialog>();

    public void ShowSquadSelectionDialog() => DialogSwitcher.ShowDialog<SquadSelectionDialog>();

    public void ShowLoadingDialog() => DialogSwitcher.ShowDialog<LoadingDialog>();

    #region Creating Dialogs

    public T GetDialogByType<T>() where T : Dialog {
        var dialog = GetDialogFromList<T>();

        if (dialog != null)
            return dialog;
        else
            return CreateNewDialog<T>();
    }



    private T GetDialogFromList<T>() where T : Dialog {
        if (_dialogs.Count == 0)
            return null;

        return (T)_dialogs.FirstOrDefault(dialog => dialog is T);
    }

    private T CreateNewDialog<T>() where T : Dialog {
        var dialog = _factory.GetDialog<T>();
        dialog.Init();

        _dialogs.Add(dialog);

        return dialog;
    }

    #endregion

    #region Dialogs Events
    private void AddListener() {
        MainMenuDialog.SettingsDialogShowed += OnShowSettingsDialog;
        MainMenuDialog.LevelSelectDialogShowed += OnShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed += OnShowAboutDialog;

        SquadSelectionDialog.BackClicked += OnShowMainMenuDialog;
        SquadSelectionDialog.SquadIndexSelected += OnSquadIndexSelected;

        LevelSelectionDialog.BackClicked += OnShowMainMenuDialog;
        LevelSelectionDialog.LevelStarted += OnLevelStarted;

        GameDialog.MainMenuClicked += OnMainMenuClicked;
        GameDialog.PauseClicked += OnPauseClicked;

        SettingsDialog.BackClicked += OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked += OnSettingsDialogResetClicked;

        AboutDialog.BackClicked += OnShowMainMenuDialog;
        
    }

    private void RemoveLisener() {
        MainMenuDialog.SettingsDialogShowed -= OnShowSettingsDialog;
        MainMenuDialog.LevelSelectDialogShowed -= OnShowLevelSelectionDialog;
        MainMenuDialog.AboutDialogShowed -= OnShowAboutDialog;

        SquadSelectionDialog.BackClicked -= OnShowMainMenuDialog;
        SquadSelectionDialog.SquadIndexSelected -= OnSquadIndexSelected;

        LevelSelectionDialog.BackClicked -= OnShowMainMenuDialog;
        LevelSelectionDialog.LevelStarted -= OnLevelStarted;


        GameDialog.MainMenuClicked -= OnMainMenuClicked;
        GameDialog.PauseClicked -= OnPauseClicked;

        SettingsDialog.BackClicked -= OnSettingsDialogBackClicked;
        SettingsDialog.ResetClicked -= OnSettingsDialogResetClicked;

        AboutDialog.BackClicked -= OnShowMainMenuDialog;

    }

    #endregion

    private void OnShowMainMenuDialog() => DialogSwitcher.ShowDialog<MainMenuDialog>();

    private void OnShowSettingsDialog() => DialogSwitcher.ShowDialog<SettingsDialog>();

    private void OnShowAboutDialog() => DialogSwitcher.ShowDialog<AboutDialog>();

    private void OnShowLevelSelectionDialog() => DialogSwitcher.ShowDialog<LevelSelectionDialog>();

    private void OnShowGameplayDialog() => DialogSwitcher.ShowDialog<GameDialog>();

    private void OnSettingsDialogBackClicked() => OnShowMainMenuDialog();

    private void OnSettingsDialogResetClicked() => _gameplayMediator.ResetPlayerProgress();

    private void OnLevelStarted(int levelIndex) {
        _gameplayMediator.LevelPreparation(levelIndex);

        OnShowGameplayDialog();
    }

    private void OnSquadIndexSelected(int squadIndex) {
        
        
    }

    #region GameDialog Events

    private void OnPauseClicked(bool value) => _gameplayMediator.SetPause(value);

    private void OnMainMenuClicked() {
        OnShowMainMenuDialog();

        _gameplayMediator.FinishGameplay(Switchovers.MainMenu);
    }

    #endregion

    public void Dispose() {
        RemoveLisener();
    }
}
