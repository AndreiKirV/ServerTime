using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

//создаю МБ по привычке, понравилось схема работы с ECS подобная, по хорошему в таких МБ все поля публичные, gо идее здесь не должно быть логики
public class ArrowMB : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform Rect;
    public ArrowType ArrowType;
    public Image Image;

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

        _dialController.SetTime();
    }

    public void Init(DialController dialController)
    {
        _dialController = dialController;
    }

    public int GetValue()
    {
        float angleZ = 360f - Rect.localEulerAngles.z;

        if (ArrowType == ArrowType.Hour)
            return (int)(angleZ % 360f / (360f / 12f));

        float result = angleZ % 360f / (360f / 60f);

        return (int)result;
    }
}

//лучше вывести в какой ниудь словарик или типа того, но т.к. проектик не большой, решил не морочиться
public enum ArrowType
{
    Hour,
    Minute,
    Second
}