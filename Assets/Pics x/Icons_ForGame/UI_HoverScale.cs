using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UI_HoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 _hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    private float _duration = 0.3f;
    private Vector3 _originalScale;
    private Tween _currentTween;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_hoverScale, _duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale, _duration).SetEase(Ease.OutQuad);
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
        transform.localScale = _originalScale;
    }
}
