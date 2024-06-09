using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class PlayerSFXManager : SoundManager {
    public PlayerSoundConfig SoundConfig { get; private set; }

    private PlayerInteraction _interaction;

    [Inject]
    public void Construct(PlayerSoundConfig config) {
        SoundConfig = config;
    }

    public void Init(Player player) {
        _interaction = player.Interaction;
        
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Volume.Volume;

        AddListener();
        PlayerReady();
    }

    public override void AddListener() {
        _interaction.ObstacleCollided += OnObstacleCollided;
        _interaction.CoinCollided += OnCoinCollided;
        _interaction.PortalCollided += OnPortalCollided;
    }

    public override void RemoveLisener() {
        _interaction.ObstacleCollided -= OnObstacleCollided;
        _interaction.CoinCollided -= OnCoinCollided;
        _interaction.PortalCollided -= OnPortalCollided;
    }

    private void PlayerReady() => PlayOneShot(SoundConfig.Start);

    private void OnObstacleCollided() => PlayOneShot(SoundConfig.Fail);

    private void OnCoinCollided(Coin coin) => PlayOneShot(SoundConfig.CoinCollect);

    private void OnPortalCollided() => PlayOneShot(SoundConfig.Complete);

    private void PlayOneShot(AudioClip clip) {

        if (clip != null)
            AudioSource.PlayOneShot(clip);

    }
}
