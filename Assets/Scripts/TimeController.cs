using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private ClockDisplayController _display;
    [SerializeField] private DialController _dialController;

    //не очень понял, зачем по ТЗ делать это по кнопке.
    [SerializeField] private Button _editButton;
    [SerializeField] private Text _editButtonText;

    public bool IsEditable => _isEditable;

    private DateTimeOffset _currentTime;
    private float _accumulatedTime;
    private bool _isEditable = false;

    public void SetTime(DateTimeOffset time)
    {
        _currentTime = time.ToOffset(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));

        _display.SetTime(_currentTime);
        _dialController.SetTime(_currentTime);
    }

    public void SetTime(int hours, int minutes, int seconds, ClockFormat Format = ClockFormat.SmallFormat)
    {
        if (Format == ClockFormat.SmallFormat)
        {
            if (_currentTime.Hour >= 12 && hours < 12)
                hours += 12;
            else if (_currentTime.Hour >= 12 && hours == 12)
                hours = 0;
        }

        _currentTime = new DateTimeOffset(_currentTime.Year, _currentTime.Month, _currentTime.Day, hours, minutes, seconds, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
    }

    private void Awake()
    {
        _dialController.Init(this);
        _display.Init(this);
        _editButton.onClick.AddListener(ChangedIsEditable);
        _dialController.ChangeInteractivity(_isEditable);
        _display.ChangeInteractivity(_isEditable);
        
    }

    private void OnDisable() 
    {
        _editButton.onClick.RemoveListener(ChangedIsEditable);
    }

    private void ChangedIsEditable()
    {
        _isEditable = !_isEditable;

        _dialController.ChangeInteractivity(_isEditable);
        _display.ChangeInteractivity(_isEditable);

        if(_isEditable)
            _editButtonText.text = "Редактируйте";
        else
            _editButtonText.text = "Редактировать";
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

public enum ClockFormat
{
    FullFormat,
    SmallFormat
}