using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadCard : MonoBehaviour {
    public event Action<int> Selected;

    [field: SerializeField] public int Index;
    [SerializeField] private Button _button;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _count;

    public bool IsSelected { get; private set; }

    public void Init() {
        ShowFrame();
        _name.text = $"Отряд {Index}";
    }

    public void SetCount(float value) {
        _count.text = $"Участники: {(int)value}";
    }

    public void SetSelectedStatus(bool status) {
        IsSelected = status;
        ShowFrame();
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

    private void ShowFrame() {
        _frameImage.enabled = IsSelected;
    }
}
