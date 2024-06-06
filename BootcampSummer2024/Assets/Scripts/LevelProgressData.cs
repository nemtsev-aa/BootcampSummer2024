using System;
using UnityEngine;

[Serializable]
public class LevelProgressData {
    public LevelProgressData(int index, int coinsCount, int percent) {
        Percent = percent;
        Index = index;
        CoinsCount = coinsCount;
    }

    [field: SerializeField] public int Index { get; private set; }
    [field: SerializeField] public int CoinsCount { get; private set; }
    [field: SerializeField] public int Percent { get; private set; }

    public void SetPercent(int percent) => Percent = percent;

    public void SetCoinsCount(int count) => CoinsCount = count;
}
