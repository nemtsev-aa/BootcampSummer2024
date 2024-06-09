using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EnvironmentSoundConfig : SoundConfig {
    private const string MAIN_MENU = "https://s3.eponesh.com/games/files/14828/MainMenu.mp3";
    private const string GAMEPLAY = "https://s3.eponesh.com/games/files/14828/GamePlay.mp3";
    private const string GAMEPLAY2 = "https://s3.eponesh.com/games/files/14828/GamePlay2.mp3";

    private List<string> _clipUrl = new List<string>() { MAIN_MENU, GAMEPLAY, GAMEPLAY2 };

    public IReadOnlyList<string> ClipUrl => _clipUrl;

    [field: SerializeField] public AudioClip UI { get; private set; }
    [field: SerializeField] public AudioClip Gameplay { get; private set; }
    [field: SerializeField] public AudioClip Gameplay2 { get; private set; }

    public void SetAudioClips(AudioClip gameplay, AudioClip gameplay2) {
        Gameplay = gameplay;
        Gameplay2 = gameplay2;
    }

    public void SetUIAudioClip(AudioClip clip) {
        UI = clip;
    }
}
