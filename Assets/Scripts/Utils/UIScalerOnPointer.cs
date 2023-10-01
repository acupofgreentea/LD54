using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class UIScalerOnPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float targetEnterScale = 1.25f;
    [SerializeField] private float duration = 0.25f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTransform(targetEnterScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTransform(1f);
    }

    private void ScaleTransform(float targetScale)
    {
        transform.DOComplete();
        transform.DOScale(targetScale, duration);
    }

    private void OnDisable() 
    {
        transform.DOComplete();
    }
}
