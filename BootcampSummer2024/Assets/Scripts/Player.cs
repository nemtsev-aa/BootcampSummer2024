using UnityEngine;
using Zenject;

public class Player : MonoBehaviour {
    [SerializeField] private PlayerInteraction _interaction;
    [SerializeField] private PlayerMove _move;

    [SerializeField] private GameObject _playerImage;
    [SerializeField] private GameObject _trail;

    [SerializeField] private GameObject _loseEffect;
    [SerializeField] private GameObject _winEffect;

    private CoinCounter _coinCounter;
    private PauseHandler _pauseHandler;

    public PlayerInteraction Interaction => _interaction;
    public PlayerMove Move => _move;

    [Inject]
    public void Construct(PauseHandler pauseHandler, CoinCounter counter) {
        _pauseHandler = pauseHandler;
        _coinCounter = counter;

        _interaction.Init(_pauseHandler);
        _move.Init(_pauseHandler);

        Reset();
    }

    public void Reset() {
        _loseEffect.SetActive(false);
        _winEffect.SetActive(false);
    }

    private void OnEnable() {
        _interaction.ObstacleCollided += OnObstacleCollided;
        _interaction.PortalCollided += OnPortalCollided;
        _interaction.CoinCollided += OnCoinCollided;
    }

    private void OnDisable() {
        _interaction.ObstacleCollided -= OnObstacleCollided;
        _interaction.PortalCollided -= OnPortalCollided;
        _interaction.CoinCollided -= OnCoinCollided;
    }

    private void OnCoinCollided(Coin coin) {
        _coinCounter.AddCoinInLevelList(coin);
    }

    private void OnPortalCollided() {
        _winEffect.SetActive(true);
        _coinCounter.AddCoinInGeneralList();

        Hide();
    }

    private void OnObstacleCollided() {
        _loseEffect.SetActive(true);
        _coinCounter.RemoveCoinsFromLevel();

        Hide();
    }

    private void Hide() {
        _playerImage.SetActive(false);
        _trail.SetActive(false);
        _move.enabled = false;

        Invoke(nameof(Reset), 1f);
    }
}
