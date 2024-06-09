using System;

[Serializable]
public class PlayerScoreData {
    public PlayerScoreData() {
    }

    public PlayerScoreData(int squadIndex, int score) {
        SquadIndex = squadIndex;
        Score = score;
    }

    public int SquadIndex { get; private set; }

    public int Score { get; private set; }

    public void SetSquadIndex(int index) {
        SquadIndex = index;
    }

    public void SetScore(int score) {
        Score = score;
    }
}
