using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ClockDisplayController : MonoBehaviour
{
    [SerializeField] private Text _time;
    [SerializeField] private CastInputField _timeInput;

    private TimeController _timeController;

    public void Init(TimeController timeController)
    {
        _timeController = timeController;
    }

    public void SetTime(DateTimeOffset time)
    {
        _time.text = $"{time.TimeOfDay.Hours} ч. {time.TimeOfDay.Minutes} м. {time.TimeOfDay.Seconds} с.";
    }

    public void ChangeInteractivity(bool target)
    {
        _timeInput.interactable = target;
    }

    private void SelectInput()
    {
        _timeInput.image.enabled = true;
    }

    private void EndInput(string target)
    {
        string result = Regex.Replace(target, @"\D", "");
        _timeInput.image.enabled = false;

        if (_timeController.IsEditable && result.Length == 6)
        {
            int hours = int.Parse(result.Substring(0, 2));
            int minutes = int.Parse(result.Substring(2, 2));
            int seconds = int.Parse(result.Substring(4, 2));

            if (hours < 24 && minutes < 60 && seconds < 60)
            {
                _timeController.SetTime(hours, minutes, seconds, ClockFormat.FullFormat);
            }
        }

        _timeInput.text = "";
    }

    private void Awake()
    {
        _timeInput.OnPointerDownA += SelectInput;
        _timeInput.onEndEdit.AddListener(EndInput);
    }

    private void OnDisable()
    {
        _timeInput.OnPointerDownA -= SelectInput;
        _timeInput.onEndEdit.RemoveListener(EndInput);
    }
}