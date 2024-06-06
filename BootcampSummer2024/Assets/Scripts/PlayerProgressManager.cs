using Cysharp.Threading.Tasks;
using GamePush;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProgressManager {
    private const string PlayerData = "PlayerData";
    private const string DefaultProgress = "DefaultPlayerProgress";

    private readonly Logger _logger;
    private readonly PlayerProgressLoader _progressLoader;
    private PlayerProgressData _currentProgress;

    public PlayerProgressManager(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _progressLoader = new PlayerProgressLoader(logger, savesManager);
    }

    private IReadOnlyList<LevelProgressData> LevelProgress => _currentProgress.LevelProgressDatas;

    public async UniTask LoadProgress() {
        _logger.Log("PlayerProgress Loading...");

        _currentProgress = await _progressLoader.LoadPlayerProgress();

        if (_currentProgress.LevelProgressDatas != null)
            _logger.Log("PlayerProgress loaded success");
        else
            _logger.Log("PlayerProgress loaded fialed");
    }

    public void UpdateProgressByLevel(LevelProgressData levelProgressData) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == levelProgressData.Index);

        data.SetPercent(levelProgressData.Percent);
        data.SetCoinsCount(levelProgressData.CoinsCount);

        _progressLoader.SavePlayerProgress(_currentProgress);
    }

    public int GetCoinsCountByLevelIndex(int levelIndex) {
        LevelProgressData data = _currentProgress.LevelProgressDatas.First(data => data.Index == levelIndex);
        return data.CoinsCount;
    }

    public async void ResetLocalPlayerProgress() {
        var defaultPlayerProgress = await _progressLoader.LoadDefaultProgress();

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        PlayerPrefs.SetString(DefaultProgress, defaultPlayerProgressToString);
    }

    public async void ResetCloudPlayerProgress() {
        var defaultPlayerProgress = await _progressLoader.LoadDefaultProgress();

        ResetProgress(defaultPlayerProgress);

        string defaultPlayerProgressToString = JsonConvert.SerializeObject(defaultPlayerProgress);
        GP_Player.Set(PlayerData, defaultPlayerProgressToString);
        GP_Player.Sync();
    }

    public IReadOnlyList<LevelProgressData> GetLevelProgress() {
        return _currentProgress.LevelProgressDatas;
    }

    private void ResetProgress(PlayerProgressData data) {
        _currentProgress = data;

        foreach (var iLevelProgress in _currentProgress.LevelProgressDatas) {
            iLevelProgress.SetPercent(0);
            iLevelProgress.SetCoinsCount(0);
        }

        _currentProgress.LevelProgressDatas[0].SetPercent(0);
        _currentProgress.LevelProgressDatas[0].SetCoinsCount(0);

        _progressLoader.SavePlayerProgress(_currentProgress);
    }

    private void UpdateProgress() {
        foreach (var iLevelProgress in LevelProgress) {
            LevelProgressData data = LevelProgress.First(data => data.Index == iLevelProgress.Index);

            data.SetPercent(iLevelProgress.Percent);
            data.SetCoinsCount(iLevelProgress.CoinsCount);
        }
    }
}
