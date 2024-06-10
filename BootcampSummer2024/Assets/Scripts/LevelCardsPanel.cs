using System;
using UnityEngine;

public class LevelCardsPanel : UIPanel {
    public event Action<int> LevelIndexSelected;

    [SerializeField] private LevelCardsManager _levelCardsManager;

    public void Init() {
        _levelCardsManager.Init();
    }

    public override void UpdateContent() {
        base.UpdateContent();

        _levelCardsManager.UpdateCards();
    }

    public override void AddListeners() {
        base.AddListeners();

        _levelCardsManager.LevelIndexSelected += OnLevelIndexSelected;
    }

    public override void RemoveListeners() {
        base.RemoveListeners();

        _levelCardsManager.LevelIndexSelected -= OnLevelIndexSelected;
    }

    private void OnLevelIndexSelected(int levelIndex) => LevelIndexSelected?.Invoke(levelIndex);
}
