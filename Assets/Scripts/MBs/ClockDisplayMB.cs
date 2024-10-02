using System;
using UnityEngine;
using UnityEngine.UI;

//создаю МБ по привычке, понравилось схема работы с ECS подобная, по хорошему в атких МБ все поля публичные, но здесь в рамках ООП закрыл. По идее здесь не должно быть логики установки текста
public class ClockDisplayMB : MonoBehaviour
{
    [SerializeField] private Text _time;

    public void SetTime(DateTimeOffset time)
    {
        _time.text = $"{time.TimeOfDay.Hours.ToString()} ч. {time.TimeOfDay.Minutes.ToString()} м. {time.TimeOfDay.Seconds.ToString()} с.";
    }
}