using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgressData {

    public PlayerProgressData(List<LevelProgressData> levelProgressDatas) {
        LevelProgressDatas = new List<LevelProgressData>();
        LevelProgressDatas.AddRange(levelProgressDatas);
    }

    public List<LevelProgressData> LevelProgressDatas { get; private set; }


    public int GetScore() {
        int currentScore = 0;

        foreach (var iLevelProgressData in LevelProgressDatas) {
            currentScore += iLevelProgressData.Percent + iLevelProgressData.CoinsCount;
        }

        return currentScore;
    }
}
