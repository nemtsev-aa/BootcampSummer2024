using System;
using UnityEngine;
using Zenject;

public class LevelProgressCounter : ITickable{
    public event Action LevelIsOver;
    public event Action<float, float> HasBeenUpdated;

    private Transform _playerTransform;
    private Transform _portalTransform;

    private float _maxX;

    public LevelProgressCounter() {
    }

    public void SetCompanents(Transform playerTransform, Transform portalTransform) {
        _playerTransform = playerTransform;
        _portalTransform = portalTransform;

        _maxX = _portalTransform.position.x - _playerTransform.position.x;
    }

    public void Reset() {
        _playerTransform = null;
        _portalTransform = null;

        _maxX = 0f;
    }

    public void Tick() {
        float currentX = _portalTransform.position.x - _playerTransform.position.x;

        if (currentX > 0)
            HasBeenUpdated?.Invoke(currentX, _maxX);
    }
}
