using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : MonoBehaviour {
    [SerializeField] private SquadCardsManager _squadCardsManager;

    private void Start() {
        _squadCardsManager.Init();
    }

    private void OnEnable() {
        _squadCardsManager.LevelIndexSelected += OnLevelIndexSelected;
    }

    private void OnDisable() {
        _squadCardsManager.LevelIndexSelected -= OnLevelIndexSelected;
    }

    private void OnLevelIndexSelected(int index) {
        SceneManager.LoadScene(index);
    }
}
