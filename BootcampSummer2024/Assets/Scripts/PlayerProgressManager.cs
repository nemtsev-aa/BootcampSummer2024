using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class PlayerProgressManager {
    public const int DEFAULTSCORE = 0;
    public const int DEFAULTSQUADINDEX = 0;

    private readonly Logger _logger;
    private readonly PlayerProgressLoader _progressLoader;
    
    private PlayerProgressData _currentProgressData;
    private PlayerScoreData _currentScoreData;

    public PlayerProgressManager(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _progressLoader = new PlayerProgressLoader(logger, savesManager);
    }

    private IReadOnlyList<LevelProgressData> LevelProgress => _currentProgressData.LevelProgressDatas;


    #region GET/SET
    public async UniTask GetCurrentProgress() {
        _logger.Log("PlayerProgress Loading...");

        _currentProgressData = await _progressLoader.LoadPlayerProgress();
        
        _logger.Log($"PlayerProgress loaded is {_currentProgressData != null}");
    }
    
    public async UniTask GetCurrentScore() {
        _logger.Log("PlayerScore Loading...");

        var currentSquadIndex = await _progressLoader.LoadSquadIndex();

        if (currentSquadIndex == 0)
            currentSquadIndex = DEFAULTSQUADINDEX;

        var currentScore = await _progressLoader.LoadPlayerScore();

        if (currentScore == 0)
            currentScore = DEFAULTSCORE;

        _currentScoreData = new PlayerScoreData(currentSquadIndex, currentScore);

        _logger.Log($"PlayerScore loaded is {_currentScoreData != null}");
    }

    public IReadOnlyList<LevelProgressData> GetLevelsProgressData() {
        return _currentProgressData.LevelProgressDatas;
    }

    public IReadOnlyList<SquadData> GetSquadDatas() {
        return null;
    }

    public int GetCoinsCountByLevelIndex(int levelIndex) {
        LevelProgressData data = _currentProgressData.LevelProgressDatas.First(data => data.Index == levelIndex);
        return data.CoinsCount;
    }

    public int GetPercentByLevelIndex(int levelIndex) {
        LevelProgressData data = _currentProgressData.LevelProgressDatas.First(data => data.Index == levelIndex);
        return data.Percent;
    }
    
    public int GetSquadIndex() {
        return _currentScoreData.SquadIndex;
    }

    public void SetSquadIndex(int index) {
        if (_currentScoreData.SquadIndex == 0) {
            _currentScoreData.SetSquadIndex(index);
        }
    }

    #endregion

    #region UPDATE
    public void UpdateProgressByLevel(LevelProgressData levelProgressData) {
        LevelProgressData data = _currentProgressData.LevelProgressDatas.First(data => data.Index == levelProgressData.Index);

        data.SetPercent(levelProgressData.Percent);
        data.SetCoinsCount(levelProgressData.CoinsCount);
    }

    public void UpdateScore() {
        int currentValue = _currentProgressData.GetScore();

        if (currentValue > _currentScoreData.Score)
            _currentScoreData = new PlayerScoreData(_currentScoreData.SquadIndex, currentValue);
    }

    #endregion

    #region SAVE
    public void SaveCurrentProgress() {
        _progressLoader.SaveProgress(_currentProgressData);
    }

    public void SavePlayerScore() {
        _progressLoader.SavePlayerScore(_currentScoreData);
    }

    #endregion

    #region RESET
    public async void ResetAll() {
        await ResetProgress();
        ResetScore();
    }

    private async UniTask ResetProgress() {
        _currentProgressData = await _progressLoader.LoadDefaultProgress();

        foreach (var iLevelProgress in _currentProgressData.LevelProgressDatas) {
            iLevelProgress.SetPercent(0);
            iLevelProgress.SetCoinsCount(0);
        }

        _progressLoader.SaveProgress(_currentProgressData);
    }

    private void ResetScore() {
        _currentScoreData = new PlayerScoreData(_currentScoreData.SquadIndex, DEFAULTSCORE);
        _progressLoader.SavePlayerScore(_currentScoreData);
    }
    #endregion
}
