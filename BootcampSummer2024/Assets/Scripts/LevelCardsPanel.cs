using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelCardsPanel : UIPanel {
    public event Action<int> LevelIndexSelected;

    [SerializeField] private List<LevelCard> _levelCards = new List<LevelCard>();

    public void Init() {
        AddListeners();
    }

    public void UpdateCards(IReadOnlyList<LevelProgressData> progressData) {
        for (int i = 0; i < _levelCards.Count; i++) {
            LevelProgressData data = progressData[i];
            SetPercentByIndex(i + 1, data.Percent, data.CoinsCount);
        }
    }

    public override void AddListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Init();
            iCard.Selected += OnSquadCardSelected;
        }
    }

    public override void RemoveListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Selected -= OnSquadCardSelected;
        }
    }

    private void OnSquadCardSelected(int index) {
        LevelIndexSelected?.Invoke(index);
    }

    private void SetPercentByIndex(int index, float percent, int score) {
        LevelCard card = _levelCards.FirstOrDefault(c => c.Index == index);
        card.SetLevelProgress(percent, score);
    }
}
