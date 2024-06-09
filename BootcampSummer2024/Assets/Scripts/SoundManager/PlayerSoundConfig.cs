using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSoundConfig : SoundConfig {
    private const string START = "https://s3.eponesh.com/games/files/14828/StartLevel.mp3";
    private const string MOVE = "https://s3.eponesh.com/games/files/14828/Move.mp3";
    private const string COMPLETE = "https://s3.eponesh.com/games/files/14828/LevelComplete.mp3";
    private const string FAIL = "https://s3.eponesh.com/games/files/14828/Fail.mp3";
    private const string COINCOLLECT = "https://s3.eponesh.com/games/files/14828/CoinCollect.mp3";

    private List<string> _clipUrl = new List<string>() { START, MOVE, COMPLETE, FAIL, COINCOLLECT };

    public IReadOnlyList<string> ClipUrl => _clipUrl;

    [field: SerializeField] public AudioClip Start { get; private set; }
    [field: SerializeField] public AudioClip Move { get; private set; }
    [field: SerializeField] public AudioClip Complete { get; private set; }
    [field: SerializeField] public AudioClip Fail { get; private set; }
    [field: SerializeField] public AudioClip CoinCollect { get; private set; }

    public void SetAudioClips(AudioClip start, AudioClip move, AudioClip complete, AudioClip fail, AudioClip coinCollect) {
        Start = start;
        Move = move;
        Complete = complete;
        Fail = fail;
        CoinCollect = coinCollect;
    }
}
