using System;

public class LevelSelectionDialog : Dialog {
    public static event Action<int> LevelStarted;

    private LevelCardsPanel _levelCardsPanel;

    public override void InitializationPanels() {
        _levelCardsPanel = GetPanelByType<LevelCardsPanel>();
        _levelCardsPanel.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _levelCardsPanel.LevelIndexSelected += OnLevelIndexSelected;
    }


    public override void RemoveListeners() {
        base.RemoveListeners();

        _levelCardsPanel.LevelIndexSelected -= OnLevelIndexSelected;
    }

    private void OnLevelIndexSelected(int index) => LevelStarted?.Invoke(index);
}
