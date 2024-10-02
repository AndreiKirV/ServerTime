using System;
using DG.Tweening;
using UnityEngine;

//если правильно именовать ClockDisplayMB с той внутрянкой, то как здесь
public class DialController : MonoBehaviour
{
    [SerializeField] private ArrowMB _hourRect;
    [SerializeField] private ArrowMB _minuteRect;
    [SerializeField] private ArrowMB _secondRect;

    private TimeController _timeController;

    private void Awake()
    {
        _hourRect.Init(this);
        _minuteRect.Init(this);
        _secondRect.Init(this);
    }

    //с использованием углов Эйлера могут быть проблемы при повороте по всем осям, но т.к. поварачиваем одну - почему нет
    public void SetTime(DateTimeOffset time)
    {
        float ang = 360f / 60f / 12f * time.Minute;
        float angleHour = 360f / 12f * time.Hour + ang;
        if (!_hourRect.IsPoint)
            _hourRect.Rect.DOLocalRotate(new Vector3(_hourRect.Rect.localEulerAngles.x, _hourRect.Rect.localEulerAngles.y, -angleHour), 0.5f, RotateMode.Fast).SetEase(Ease.InOutBack);

        float amgleMinute = 360f / 60f * time.Minute;
        if (!_minuteRect.IsPoint)
            _minuteRect.Rect.DOLocalRotate(new Vector3(_minuteRect.Rect.localEulerAngles.x, _minuteRect.Rect.localEulerAngles.y, -amgleMinute), 0.5f, RotateMode.Fast).SetEase(Ease.InOutBack);

        float amgleSecond = 360f / 60f * time.Second;
        if (!_secondRect.IsPoint)
            _secondRect.Rect.DOLocalRotate(new Vector3(_secondRect.Rect.localEulerAngles.x, _secondRect.Rect.localEulerAngles.y, -amgleSecond), 0.5f, RotateMode.Fast).SetEase(Ease.InOutBack);
    }

    public void SetTime()
    {
        _timeController.SetTime(_hourRect.GetValue(), _minuteRect.GetValue(), _secondRect.GetValue());
    }

    public void Init(TimeController timeController)
    {
        _timeController = timeController;
    }
}