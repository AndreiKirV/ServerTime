using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowMB : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform Rect;
    public ArrowType arrowType;

    public bool IsPoint => _isPoint;

    private DialController _dialController;

    private bool _isPoint = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isPoint = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(Rect.position);
        Vector2 direction = (mousePosition - objectPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Rect.DORotate(new Vector3(0, 0, angle), 1f).SetEase(Ease.OutElastic);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isPoint = false;
        //float angleZ = 360f - Rect.localEulerAngles.z;
        //(int)(angleZ % 360f / (360f / 60f));

        _dialController.SetTime();

    }

    public void Init(DialController dialController)
    {
        _dialController = dialController;
    }

    public int GetValue()
    {
        float angleZ = 360f - Rect.localEulerAngles.z;

        if (arrowType == ArrowType.Hour)
            return (int)(angleZ % 360f / (360f / 12f));

        float result = angleZ % 360f / (360f / 60f);

        return (int)result;
    }
}

public enum ArrowType
{
    Hour,
    Minute,
    Second
}