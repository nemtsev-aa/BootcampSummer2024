using System;
using UnityEngine;
using Zenject;

public class LevelProgressCounter : ITickable {
    public event Action LevelIsOver;
    public event Action<float, float> HasBeenUpdated;

    private Transform _playerTransform;
    private Transform _portalTransform;

    private float _maxX;
    private float _currentPercent;

    public int CurrentPercent => (int)(_currentPercent * 100);


    public LevelProgressCounter() {
    }

    public void SetCompanents(Transform playerTransform, Transform portalTransform) {
        _playerTransform = playerTransform;
        _portalTransform = portalTransform;

        _maxX = _portalTransform.position.x - _playerTransform.position.x;
    }

    public void Reset() {
        LevelIsOver?.Invoke();

        _playerTransform = null;
        _portalTransform = null;

        _maxX = 0f;

    }

    public void Tick() {
        if (_portalTransform == null || _playerTransform == null)
            return;

        float currentX = _portalTransform.position.x - _playerTransform.position.x;
        _currentPercent = 1 - currentX / _maxX;

        if (currentX > 0)
            HasBeenUpdated?.Invoke(currentX, _maxX);

    }
}
