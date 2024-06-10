public class LoadingDialog : Dialog {

    private LoadingPanel _loadingPanel;

    public override void Show(bool value) {
        base.Show(value);

        if (value) {
            _loadingPanel.Show(true);
        }
    }

    public override void InitializationPanels() {
        _loadingPanel = GetPanelByType<LoadingPanel>();
        _loadingPanel.Init();
    }

    public override void AddListeners() {
        base.AddListeners();

    }

    public override void RemoveListeners() {
        base.RemoveListeners();

    }
}
