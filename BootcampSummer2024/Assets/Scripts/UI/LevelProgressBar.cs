using TMPro;
using UnityEngine;

public class LevelProgressBar : Bar {
    [SerializeField] private TextMeshProUGUI _progressLabel;

    private LevelProgressCounter _progressCounter;

    public void Init(LevelProgressCounter progressCounter) {
        _progressCounter = progressCounter;

        AddListeners();
    }

    public override void AddListeners() {
        base.AddListeners();

        if (_progressCounter != null)
            _progressCounter.HasBeenUpdated += OnValueChanged;

    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        if (_progressCounter != null)
            _progressCounter.HasBeenUpdated -= OnValueChanged;
    }

    public override void Reset() {
        Filler.fillAmount = 0f;
        _progressLabel.text = "0%";
    }

    public void ShowLevelIsOver() {
        Filler.fillAmount = 1f;
        _progressLabel.text = "100%";
    }

    protected override void OnValueChanged(float currentValue, float maxValue) {

        float currentPercent = 1 - currentValue / maxValue;

        if (Filler != null)
            Filler.fillAmount = currentPercent;

        if (_progressLabel != null)
            _progressLabel.text = $"{(int)(currentPercent * 100)} %";
    }
}
