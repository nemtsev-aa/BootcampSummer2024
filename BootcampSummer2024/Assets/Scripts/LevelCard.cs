using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour {
    public event Action<int> Selected;

    [field: SerializeField] public int Index;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _percent;
    [SerializeField] private TextMeshProUGUI _coins;

    public void Init() {
        _name.text = $"{Index}";
    }

    public void SetLevelProgress(float percent, int coins) {
        _percent.text = $"{(int)percent}%";
        _coins.text = $"{coins}";

    }

    private void OnEnable() {
        _button.onClick.AddListener(ClickLevelCard);
    }

    private void OnDisable() {
        _button.onClick.RemoveListener(ClickLevelCard);
    }

    private void ClickLevelCard() {
        Selected?.Invoke(Index);
    }
}
