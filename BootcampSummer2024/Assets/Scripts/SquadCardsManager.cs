using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SquadCardsManager : MonoBehaviour {
    public event Action<int> SquadIndexSelected;

    [SerializeField] private List<SquadCard> _squadCards = new List<SquadCard>();

    private SquadCard _selectedCard;
    private PlayerProgressManager _progressManager;

    [Inject]
    public void Construct(PlayerProgressManager progressManager) {
        _progressManager = progressManager;
    }

    public void Init() {
        AddListeners();
    }

    public void UpadeCards() {
        var squadDatas = _progressManager.GetSquadDatas();

        if (squadDatas == null || squadDatas.Count == 0)
            return;

        foreach (var iData in squadDatas) {
            SquadCard card = _squadCards.FirstOrDefault(c => c.Index == iData.Index);
            card.SetCount(iData.Count);
        }
    }

    public void ShowSelectCard(int index) {
        SquadCard card = _squadCards.FirstOrDefault(c => c.Index == index);

        if (card != null) {
            _selectedCard = card;
            card.SetSelectedStatus(true);
        }
    }

    private void SetPlayerCountBySquadIndex(int index, float value) {
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
        SquadCard card = _squadCards.FirstOrDefault(c => c.Index == index);

        if (card.Equals(_selectedCard))
            SquadIndexSelected?.Invoke(index);
        else
            SwitchSelectedCard(card);
    }

    private void SwitchSelectedCard(SquadCard newSelectedCard) {
        if (_selectedCard != null)
            _selectedCard.SetSelectedStatus(false);

        _selectedCard = newSelectedCard;
        _selectedCard.SetSelectedStatus(true);
    }

    public void Dispose() {
        RemoveListeners();
    }
}
