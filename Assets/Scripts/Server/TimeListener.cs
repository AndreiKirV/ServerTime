using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class TimeListener : MonoBehaviour
{
    [SerializeField] private string TimeApiUrl = "https://yandex.com/time/sync.json";
    [SerializeField] private TimeController _timeController;
    [SerializeField] private float _timeUpdateFrequency;
    private Coroutine timeCoroutine;

    private void Start()
    {
        timeCoroutine = StartCoroutine(UpdateTimePeriodically());
    }

    private void OnDisable()
    {
        //можно еще в самой корутине проверять наличие объекта и там ее стопить
        StopCoroutine(timeCoroutine);
    }

    private IEnumerator UpdateTimePeriodically()
    {
        while (true)
        {
            yield return GetTime();
            yield return new WaitForSeconds(_timeUpdateFrequency);
        }
    }

    private IEnumerator GetTime()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(TimeApiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError($"Error: {request.error}");
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Data timeData = JsonUtility.FromJson<Data>(jsonResponse);
                DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeData.time);

                _timeController.SetTime(dateTime);
            }
        }
    }

    public class Data
    {
        public long time;
    }
}