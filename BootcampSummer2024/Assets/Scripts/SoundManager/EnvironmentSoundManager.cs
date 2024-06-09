using System;
using UnityEngine;
using Zenject;

public enum MusicType {
    UI,
    Gameplay,
    Gameplay2
}

public class EnvironmentSoundManager : SoundManager {
    public EnvironmentSoundConfig SoundConfig { get; private set; }

    [Inject]
    public void Construct(EnvironmentSoundConfig config) {
        SoundConfig = config;
    }

    public void Init() {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.volume = Volume.Volume - 0.1f;

        AddListener();
        IsInit = true;
    }

    public void PlaySound(MusicType type) {
        if (IsInit == false)
            return;

        AudioSource.Stop();

        var clip = GetAudioClipByType(type);

        if (clip != null) {
            AudioSource.clip = clip;
            AudioSource.Play();
        } 
    }

    public override void AddListener() {
        Volume.VolumeChanged += OnVolumeChanged;
    }

    public override void RemoveLisener() {
        Volume.VolumeChanged -= OnVolumeChanged;
    }

    private void OnVolumeChanged(float value) => AudioSource.volume = Volume.Volume;

    private AudioClip GetAudioClipByType(MusicType type) {
        switch (type) {
            case MusicType.UI:
                return SoundConfig.UI;
    
            case MusicType.Gameplay:
                return SoundConfig.Gameplay;

            case MusicType.Gameplay2:
                return SoundConfig.Gameplay2;

            default:
                throw new ArgumentException($"Invalid MusicType: {type}");
        }
    }
}
