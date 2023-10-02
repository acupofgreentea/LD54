using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishListElement : MonoBehaviour
{
    [SerializeField] private Image fishImage;
    [SerializeField] private Image moneyImage;

    [SerializeField] private TMP_Text fishNameText;
    [SerializeField] private TMP_Text heaviestFishText;
    [SerializeField] private TMP_Text moneyAmountText;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(FishInfoUI fishInfoUI, int fishAmount, Sprite moneySprite, float heaviestFishKg, int moneyAmount)
    {
        fishImage.sprite = fishInfoUI.FishImage;
        fishNameText.text = $"{fishInfoUI.FishName} x{fishAmount}";
        moneyImage.sprite = moneySprite;
        heaviestFishText.text = heaviestFishKg.ToString("0.00") + "kg";

        moneyAmountText.text = 0.ToString();
        this.moneyAmount = moneyAmount;
        MoneyAmountSequence();
    }
    private void MoneyAmountSequence()
    {
        canvasGroup.alpha = 0f;
        int startMoneyAmount = 0;
        DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1f, 0.65f).OnComplete(()=>
        DOTween.To(()=> startMoneyAmount, x => startMoneyAmount = x, moneyAmount, 1.15f).SetEase(Ease.OutCirc).OnUpdate(()=> 
        moneyAmountText.text = startMoneyAmount.ToString()));

        
    }

    private int moneyAmount;
}
