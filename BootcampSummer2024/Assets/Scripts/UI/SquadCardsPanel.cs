using System;
using UnityEngine;

public class SquadCardsPanel : UIPanel {
    public event Action<int> SquadIndexSelected;

    [SerializeField] private SquadCardsManager _squadCardsManager;

    public void Init() {
        
    }

    public override void UpdateContent() {
        base.UpdateContent();

        _squadCardsManager.UpadeCards();
    }

    public override void AddListeners() {
        base.AddListeners();

        _squadCardsManager.SquadIndexSelected += OnSquadIndexSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _squadCardsManager.SquadIndexSelected -= OnSquadIndexSelected;
    }

    private void OnSquadIndexSelected(int index) => SquadIndexSelected?.Invoke(index);
}
