using Cysharp.Threading.Tasks;
using GamePush;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class CloudToFileStorageService : IStorageService {

    private const string DefaultPlayerProgress = "https://s3.eponesh.com/games/files/14828/DefaultPlayerProgress.json";
    private string _stringData;
    private Logger _logger;

    public CloudToFileStorageService(Logger logger) {
        _logger = logger;
    }

    public void Init(string path) {

    }

    public async void Load<T>(string key, Action<T> callback) {
        _logger.Log($"CloudToFileStorageService: {key} loading...");

        string currentProgressData = GP_Player.GetString(key);

        if (currentProgressData != null && currentProgressData != "") {
            var data = JsonConvert.DeserializeObject<T>(currentProgressData);

            _logger.Log($"CloudToFileStorageService_CurrentProgress: {data}");
            callback?.Invoke(data);

            return;
        }

        if (TryLocalProgressDataLoad(key, out string localData)) {
            var data = JsonConvert.DeserializeObject<T>(localData);

            _logger.Log($"CloudToFileStorageService_LocalProgress: {data}");
            callback?.Invoke(data);

            return;
        }

        var defaultProgress = await TryDefaultProgressDataLoad();
        if (defaultProgress) {
            var data = JsonConvert.DeserializeObject<T>(_stringData);

            _logger.Log($"CloudToFileStorageService_DefaultProgress: {data}");
            callback?.Invoke(data);
        }

    }

    public async UniTask<int> LoadIntAsync(string key) {
        int value = GP_Player.GetInt(key);
        await UniTask.Delay(10);
        
        return value;
    }

    public async UniTask<float> LoadFloatAsync(string key) {
        float value = GP_Player.GetFloat(key);
        await UniTask.Delay(10);

        return value;
    }

    public void SaveAsString(string key, object data, Action<bool> callback = null) {
        string stringData = JsonConvert.SerializeObject(data);

        SaveInPlayerPrefs(key, stringData);

        GP_Player.Set(key, stringData);
        GP_Player.Sync();

        callback?.Invoke(true);
    }

    public void SaveAsInt(string key, int data, Action<bool> callback = null) {
        int intData = (int)data;

        SaveInPlayerPrefs(key, intData);

        GP_Player.Set(key, intData);
        GP_Player.Sync();

        callback?.Invoke(true);
    }

    public bool TryLocalProgressDataLoad(string key, out string stringData) {
        try {
            stringData = PlayerPrefs.GetString(key);
            return stringData != "";
        }
        catch (Exception ex) {
            throw new Exception($"[TryLocalProgressDataLoad] error {ex.Message}");
        }
    }

    public async UniTask<bool> TryDefaultProgressDataLoad() {
        _stringData = await LoadDefaultProgress();

        return _stringData != "";
    }

    public async UniTask<string> LoadDefaultProgress() {
        var req = UnityWebRequest.Get(DefaultPlayerProgress);
        var op = await req.SendWebRequest();

        return op.downloadHandler.text;
    }

    private void SaveInPlayerPrefs(string key, object value) {
        try {
            if (value is string)
                PlayerPrefs.SetString(key, (string)value);
            else if (value is int)
                PlayerPrefs.SetInt(key, (int)value);
            else if (value is float)
                PlayerPrefs.SetFloat(key, (float)value);
        }
        catch (Exception ex) {
            throw new Exception($"[SaveInPlayerPrefs] error: {ex.Message}");
        }
    }
}
