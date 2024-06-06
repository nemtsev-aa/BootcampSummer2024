using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public event Action ObstacleCollided;
    public event Action PortalCollided;
    public event Action<Coin> CoinCollided;
    
    private PauseHandler _pauseHandler;
    
    public void Init(PauseHandler pauseHandler) {
        if (_pauseHandler == null)
            _pauseHandler = pauseHandler;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_pauseHandler.IsPaused)
            return;

        if (collision.gameObject.GetComponent<Obstacle>()) 
            ObstacleCollided?.Invoke();
        
        if (collision.gameObject.GetComponent<Portal>()) 
            PortalCollided?.Invoke();
        
        if (collision.gameObject.TryGetComponent(out Coin coin)) 
            CoinCollided?.Invoke(coin);
    }
}
