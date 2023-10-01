using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private Button closeButton;

    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(HandlePlayButton);
        exitButton.onClick.AddListener(HandleExitButton);
        settingsButton.onClick.AddListener(HandleSettingsButton);
        closeButton.onClick.AddListener(HandleCloseButton);
    }
    void Start()
    {
        settingsPanel.SetActive(false);
    }

    private void HandleCloseButton()
    {
        settingsPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(()=> 
        settingsPanel.SetActive(true));
    }

    private void HandleSettingsButton()
    {
        settingsPanel.transform.localScale = Vector3.zero;
        settingsPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        settingsPanel.SetActive(true);
    }

    private void HandleExitButton()
    {
        Application.Quit();
    }

    private void HandlePlayButton()
    {
        SceneManagement.Instance.LoadSceneAsync(1); //gamescene
    }
}
