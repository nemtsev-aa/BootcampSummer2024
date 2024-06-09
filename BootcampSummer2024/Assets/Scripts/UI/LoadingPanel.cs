using DG.Tweening;
using TMPro;
using UnityEngine;

public class LoadingPanel : UIPanel {
    [SerializeField] private TextMeshProUGUI _label;
    private Tween _fadeTween;

    public override void Show(bool value) {
        base.Show(value);

        StartAnimation(value);
    }

    private void StartAnimation(bool value) {
        if (value)
            _fadeTween = _label.DOColor(Color.clear, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        else
            _fadeTween.Kill();
    }
}
