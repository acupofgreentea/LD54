using System;
using System.Collections;
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
        TestAquarium.OnAquariumIsEmpty += HandleAquariumEmpty;
    }

    private void HandleAquariumEmpty()
    {
        panel.SetActive(true);
    }

    private void HandleAllEventsCompleted()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(5f);
            panel.SetActive(true);
        }
    }

    private void HandleExitButton()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        AquariumEventsManager.AllEventsCompleted -= HandleAllEventsCompleted;
        TestAquarium.OnAquariumIsEmpty -= HandleAquariumEmpty;
    }

    private void HandlePlayAgainButton()
    {
        DataManager.ResetMoney();
        SceneManagement.Instance.LoadSceneAsync(1); // gamescene
    }
}
