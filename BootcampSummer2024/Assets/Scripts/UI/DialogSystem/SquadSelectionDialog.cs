using System;

public class SquadSelectionDialog : Dialog {
    public static event Action<int> SquadIndexSelected;

    private SquadCardsPanel _squadCardsPanel;

    public override void Show(bool value) {
        base.Show(value);

        if (true)
            _squadCardsPanel.Show(true);
    }

    public override void InitializationPanels() {
        _squadCardsPanel = GetPanelByType<SquadCardsPanel>();
        _squadCardsPanel.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

        _squadCardsPanel.SquadIndexSelected += OnSquadIndexSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _squadCardsPanel.SquadIndexSelected -= OnSquadIndexSelected;
    }

    private void OnSquadIndexSelected(int index) => SquadIndexSelected?.Invoke(index);
}
