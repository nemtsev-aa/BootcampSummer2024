using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelCardsManager : MonoBehaviour, IDisposable {
    public event Action<int> LevelIndexSelected;

    [SerializeField] private List<LevelCard> _levelCards = new List<LevelCard>();
    private PlayerProgressManager _progressManager;

    [Inject]
    public void Construct(PlayerProgressManager progressManager) {
        _progressManager = progressManager;
    }

    public void Init() {
        AddListeners();
        UpdateCards();
    }

    public void UpdateCards() {
        var progressData = _progressManager.GetLevelsProgressData();

        for (int i = 0; i < _levelCards.Count; i++) {
            LevelProgressData data = progressData[i];
            SetPercentByIndex(i + 1, data.Percent, data.CoinsCount);
        }
    }

    private void AddListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Init();
            iCard.Selected += OnLevelIndexSelected;
        }
    }

    private void RemoveListeners() {
        foreach (var iCard in _levelCards) {
            iCard.Selected -= OnLevelIndexSelected;
        }
    }

    private void OnLevelIndexSelected(int index) {
        LevelIndexSelected?.Invoke(index);
    }

    private void SetPercentByIndex(int index, float percent, int score) {
        LevelCard card = _levelCards.FirstOrDefault(c => c.Index == index);
        card.SetLevelProgress(percent, score);
    }

    public void Dispose() {
        RemoveListeners();
    }
}
