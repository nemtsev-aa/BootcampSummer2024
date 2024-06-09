using System;

public interface IStorageService {
    void Init(string path);
    void SaveAsString(string key, object data, Action<bool> callback = null);
    void SaveAsInt(string key, int data, Action<bool> callback = null);
    void Load<T>(string key, Action<T> callback);
}
