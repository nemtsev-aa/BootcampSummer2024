using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SquadCardsManager : MonoBehaviour, IDisposable {
    public event Action<int> SquadIndexSelected;

    [SerializeField] private List<SquadCard> _squadCards = new List<SquadCard>();

    public void Init() {
        AddListeners();
    }

    public void SetPercentByIndex(int index, float value) {
        SquadCard card = _squadCards.FirstOrDefault(c => c.Index == index);
        card.SetCount(value);
    }

    private void AddListeners() {
        foreach (var iCard in _squadCards) {
            iCard.Init();
            iCard.Selected += OnSquadCardSelected;
        }
    }

    private void RemoveListeners() {
        foreach (var iCard in _squadCards) {
            iCard.Selected -= OnSquadCardSelected;
        }
    }

    private void OnSquadCardSelected(int index) {
        SquadIndexSelected?.Invoke(index);
    }

    public void Dispose() {
        RemoveListeners();
    }
}
