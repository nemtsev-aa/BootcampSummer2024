using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCardsManager : MonoBehaviour, IDisposable {
    public event Action<int> LevelIndexSelected;

    [SerializeField] private List<LevelCard> _levelCards = new List<LevelCard>();

    public void Init() {
        AddListeners();
    }

    public void SetPercentByIndex(int index, float value) {
        LevelCard card = _levelCards.FirstOrDefault(c => c.Index == index);
        card.SetPercent(value);
    }

    private void AddListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Init();
            iCard.Selected += OnSquadCardSelected;
        }
    }

    private void RemoveListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Selected -= OnSquadCardSelected;
        }
    }

    private void OnSquadCardSelected(int index) {
        LevelIndexSelected?.Invoke(index);
    }

    public void Dispose() {
        RemoveListeners();
    }
}
