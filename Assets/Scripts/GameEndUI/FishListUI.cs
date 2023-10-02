using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishListUI : MonoBehaviour
{
    [SerializeField] private List<FishListElement> fishListElements;

    [SerializeField] private Sprite moneySprite;

    [SerializeField] private TMP_Text totalEarnedText;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Image lineImage;

    private int totalMoneyAmount;

    private void OnEnable()
    {
        InitPanel();
    }

    private void InitPanel()
    {
        var fishes = TestAquarium.Instance.GetFishes;
        totalMoneyAmount = 0;
        StartCoroutine(Sequence());
        IEnumerator Sequence()
        {
            for (int i = 0; i < fishListElements.Count; i++)
            {
                FishListElement fishElement = fishListElements[i];
                var typeFishes = fishes.FindAll(x => (int)x.FishType == i);

                if(typeFishes.Count == 0)
                {
                    fishElement.gameObject.SetActive(false);
                    continue;
                }
                
                var heaviest = typeFishes.OrderByDescending(x => x.FishGrowthController.CurrentGrowthAmount).First();
                int moneyAmount = 0;
                foreach (var item in typeFishes)
                {
                    if(item.FishType == FishType.Whale)
                    {
                        moneyAmount += item.GetComponent<MiniWhaleGrowthController>().GetRevenueAmount;
                    }
                    else
                        moneyAmount += item.FishGrowthController.GetRevenueAmount;
                }
                totalMoneyAmount += moneyAmount;
                
                fishElement.gameObject.SetActive(true);
                fishElement.Init(typeFishes[0].FishInfoUI, typeFishes.Count, moneySprite, heaviest.FishGrowthController.CurrentGrowthAmount, moneyAmount);

                yield return new WaitForSeconds(2f);
            }

            int startMoney = 0;
            totalEarnedText.text = 0.ToString();
            
            lineImage.DOFade(1f, 0.4f).OnComplete(()=>
            DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1f, 0.65f).OnComplete(()=>
            DOTween.To(()=> startMoney, x => startMoney = x, totalMoneyAmount, 1.15f).SetEase(Ease.OutCirc).OnUpdate(()=> 
            totalEarnedText.text = startMoney.ToString()))
            );

        }

    }
}
