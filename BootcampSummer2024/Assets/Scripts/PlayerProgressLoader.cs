using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine.Networking;

public class PlayerProgressLoader {
    private const string PROGRESS = "PlayerData";
    private const string SQUADINDEX = "SquadIndex";
    private const string SCORE = "score";
    private const string DefaultProgress = "DefaultPlayerProgress";
    private const string DefaultPlayerProgress = "https://s3.eponesh.com/games/files/14828/DefaultPlayerProgress.json";


    private Logger _logger;
    private SavesManager _savesManager;

    private PlayerProgressData _playerData;
    private int _playerScore;
    private int _playerSquadIndex;

    private bool _isLoadComplete;

    public PlayerProgressLoader(Logger logger, SavesManager savesManager) {
        _logger = logger;
        _savesManager = savesManager;
    }

    private CloudToFileStorageService _saveService => (CloudToFileStorageService)_savesManager.CurrentService;


    #region LOAD PROGRESS

    public async UniTask<PlayerProgressData> LoadPlayerProgress() {
        _isLoadComplete = false;
        _saveService.Load<PlayerProgressData>(PROGRESS, OnLevelProgressLoaded);

        while (_isLoadComplete == false) {
            await UniTask.Yield();
        }

        return _playerData;
    }

    private async void OnLevelProgressLoaded(PlayerProgressData playerData) {
        _isLoadComplete = true;

        if (playerData.LevelProgressDatas != null) {
            _playerData = playerData;

            int complitedLevelCount = _playerData.LevelProgressDatas.Where(level => level.Percent == 100).Count();
            _logger.Log($"CurrentPlayerProgress: {complitedLevelCount} levels complited");

            return;
        }

        _logger.Log($"CurrentPlayerProgress is empty!");
        _isLoadComplete = false;

        var defaultProgressData = await LoadDefaultProgress();

        if (defaultProgressData != null) {
            _playerData = defaultProgressData;

            _logger.Log($"DefaultPlayerProgress loading success");
            _isLoadComplete = true;

            return;
        }
    }

    public async UniTask<PlayerProgressData> LoadDefaultProgress() {

        string defaultProgressToString = await GetTextAsync(UnityWebRequest.Get(DefaultPlayerProgress));

        if (defaultProgressToString != "") {
            _logger.Log($"LoadDefaultProgress succeeded");

            return JsonConvert.DeserializeObject<PlayerProgressData>(defaultProgressToString);
        }
        else {
            _logger.Log($"LoadDefaultProgress falled!");
            return null;
        }
    }

    private async UniTask<string> GetTextAsync(UnityWebRequest req) {
        var op = await req.SendWebRequest();
        return op.downloadHandler.text;
    }

    #endregion

    #region LOAD SCORE/SQUADINDEX
    public async UniTask<int> LoadPlayerScore() {
        _isLoadComplete = false;

        _playerScore = await LoadIntAsync(SCORE);

        return _playerScore;
    }

    public async UniTask<int> LoadSquadIndex() {
        _isLoadComplete = false;

        _playerSquadIndex = await LoadIntAsync(SQUADINDEX);

        return _playerSquadIndex;
    }

    private async UniTask<int> LoadIntAsync(string key) {
        int value = await _saveService.LoadIntAsync(key);

        if (value > 0) 
            _logger.Log($"Current value [{key}]: {value}");
        else
            _logger.Log($"Current value [{key}] is Empty");

        return value;
    }

    #endregion
    
    #region SAVE
    public void SaveProgress(PlayerProgressData playerData) => _savesManager.SaveAsString(PROGRESS, playerData, ShowSavingResult);

    public void SavePlayerScore(PlayerScoreData scoreData) {
        _savesManager.SaveAsInt(SCORE, scoreData.Score, ShowSavingResult);
        _savesManager.SaveAsInt(SQUADINDEX, scoreData.Score, ShowSavingResult);
    } 

    private void ShowSavingResult(bool status) {
        if (status == true)
            _logger.Log("Save complited");
        else
            _logger.Log("Save falled");
    }

    #endregion
}
