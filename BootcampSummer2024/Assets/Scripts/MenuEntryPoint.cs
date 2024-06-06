using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : MonoBehaviour {
    [SerializeField] private SquadCardsManager _squadCardsManager;
    [SerializeField] private LevelCardsManager _levelCardsManager;
    
    private void Start() {
        _squadCardsManager.Init();
        _levelCardsManager.Init();

        _squadCardsManager.transform.parent.gameObject.SetActive(true);
        _levelCardsManager.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable() {
        _squadCardsManager.SquadIndexSelected += OnSquadIndexSelected;
        _levelCardsManager.LevelIndexSelected += OnLevelIndexSelected;
    }

    private void OnDisable() {
        _squadCardsManager.SquadIndexSelected -= OnSquadIndexSelected;
        _levelCardsManager.LevelIndexSelected -= OnLevelIndexSelected;
    }

    private void OnSquadIndexSelected(int index) {
        _squadCardsManager.transform.parent.gameObject.SetActive(false);
        _levelCardsManager.transform.parent.gameObject.SetActive(true);
    }

    private void OnLevelIndexSelected(int index) {
        SceneManager.LoadScene(index);
    }
}
