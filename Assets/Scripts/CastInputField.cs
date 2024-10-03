using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CastInputField : InputField
{
    public Action OnPointerDownA;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnPointerDownA?.Invoke();
    }
}