using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadCard : MonoBehaviour {
    public event Action<int> Selected;

    [field: SerializeField] public int Index;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _percent;

    public void SetPercent(float value) {
        _percent.text = $"{(int)value}%";
    }

    public void Init() {
        _name.text = $"Отряд {Index}";
    }

    private void OnEnable() {
        _button.onClick.AddListener(ClickSquadCard);
    }

    private void OnDisable() {
        _button.onClick.RemoveListener(ClickSquadCard);
    }

    private void ClickSquadCard() {
        Selected?.Invoke(Index);
    }
}
