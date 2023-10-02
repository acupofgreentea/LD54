using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject panel;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(HandlePlayAgainButton);
        exitButton.onClick.AddListener(HandleExitButton);
        
        panel.SetActive(false);
    }
    private void Start()
    {
        AquariumEventsManager.AllEventsCompleted += HandleAllEventsCompleted;
        TestAquarium.OnAquariumIsEmpty += HandleAllEventsCompleted;
    }

    private void HandleAllEventsCompleted()
    {
        panel.SetActive(true);
    }

    private void HandleExitButton()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        AquariumEventsManager.AllEventsCompleted -= HandleAllEventsCompleted;
        TestAquarium.OnAquariumIsEmpty -= HandleAllEventsCompleted;
    }

    private void HandlePlayAgainButton()
    {
        DataManager.ResetMoney();
        SceneManagement.Instance.LoadSceneAsync(1); // gamescene
    }
}
