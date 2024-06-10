using System;
using TMPro;
using UnityEngine;

public class LevelInfoPanel : UIPanel {
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private LevelProgressBar _progressBar;

    public void Init(string levelName) {
        _nameLabel.text = levelName;
        _progressBar.Init();
    }

    internal void Init() {
        throw new NotImplementedException();
    }
}
