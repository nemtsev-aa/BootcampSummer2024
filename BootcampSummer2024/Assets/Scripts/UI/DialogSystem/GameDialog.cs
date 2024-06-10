using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDialog : Dialog {
    public static event Action MainMenuClicked;
    public static event Action ResetClicked;
    public static event Action NextLevelClicked;

    public static event Action<bool> PauseClicked;
    public static event Action<bool> LearningClicked;
}
