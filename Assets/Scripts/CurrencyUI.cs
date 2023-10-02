using DG.Tweening;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        DataManager.CurrencyUpdate += HandleCurrencyUpdate;
        lastAmount = DataManager.CurrentMoney;
        moneyText.text = lastAmount.ToString();
    }

    private int lastAmount;

    private void HandleCurrencyUpdate()
    {
        DOTween.To(()=> lastAmount, x=> lastAmount = x, DataManager.CurrentMoney, 0.65f).
        SetEase(Ease.OutCirc).
        OnUpdate(()=> moneyText.text = lastAmount.ToString());
    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            DataManager.CurrentMoney += 10;
    }
#endif
    void OnDestroy()
    {
        DataManager.CurrencyUpdate -= HandleCurrencyUpdate;
    }
}
