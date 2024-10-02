using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour//, IPointerClickHandler
{
    [SerializeField] private ClockDisplayMB _display;
    [SerializeField] private DialController _dialController;

    //не очень поняял, зачем по ТЗ делать это по кнопке.
    [SerializeField] private Button _editButton;

    private DateTimeOffset _currentTime;
    private float _accumulatedTime;

    private void Awake()
    {
        _dialController.Init(this);
        //_editButton.OnPointerClick(new UnityEngine.EventSystems.PointerEventData());//onClick.AddListener(() => Debug.Log("klick"));
    }

    // OnPointerClick

    public void SetTime(DateTimeOffset time)
    {
        _currentTime = time.ToOffset(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));

        _display.SetTime(_currentTime);
        _dialController.SetTime(_currentTime);
    }

    public void SetTime(int hours, int minutes, int seconds)
    {
        if (_currentTime.Hour >= 12 && hours < 12)
            hours += 12;
        else if (_currentTime.Hour < 12 && hours == 12)
            hours = 0;

        _currentTime = new DateTimeOffset(_currentTime.Year, _currentTime.Month, _currentTime.Day, hours, minutes, seconds, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));//new DateTimeOffset(_currentTime.Year, _currentTime.Month, _currentTime.Day, hours, minutes, seconds, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));

        _display.SetTime(_currentTime);
        _dialController.SetTime(_currentTime);
    }

    private void Update()
    {
        AddSeconds();
    }

    //решил буквально добавлять секунады, первое что еще приходит в голову это брать системное текущее время и ровнять по нему
    private void AddSeconds()
    {
        if (_currentTime == DateTimeOffset.MinValue) return;

        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime >= 1)
        {
            _currentTime = _currentTime.AddSeconds(_accumulatedTime);
            SetTime(_currentTime);
            _accumulatedTime = 0;
        }
    }
}