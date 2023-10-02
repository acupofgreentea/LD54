using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishMarketElement : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text moneyAmountText;

    [SerializeField] private FishType fishType;
    [SerializeField] private int moneyAmount;

    private void Awake()
    {
        button.onClick.AddListener(HandleButtonClick);
        moneyAmountText.text = moneyAmount.ToString();

        button.interactable = DataManager.CurrentMoney >= moneyAmount;
        DataManager.CurrencyUpdate += HandleCurrencyUpdate;
    }

    private void HandleCurrencyUpdate()
    {
        button.interactable = DataManager.CurrentMoney >= moneyAmount;
    }

    void OnDestroy()
    {
        DataManager.CurrencyUpdate -= HandleCurrencyUpdate;
    }

    private void HandleButtonClick()
    {
        DataManager.CurrentMoney -= moneyAmount;
        TestAquarium.Instance.SpawnFish(fishType, 1);
    }
}
