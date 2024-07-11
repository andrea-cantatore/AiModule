using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public enum TimeOfDay
    {
        Day,
        Night
    }

    [SerializeField] private float _dayDuration = 60f;
    [SerializeField] private int _dayStartHour = 4;
    [SerializeField] private int _nightStartHour = 24;

    private float _timer;
    private TimeOfDay _currentDayTime = TimeOfDay.Day;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _timer = (_dayStartHour / 24f) * _dayDuration;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _dayDuration)
        {
            _timer -= _dayDuration;
        }

        UpdateTimeOfDay();
    }

    private void UpdateTimeOfDay()
    {
        float currentHour = CurrentHour();

        if (currentHour >= _dayStartHour && currentHour < _nightStartHour)
        {
            if (_currentDayTime == TimeOfDay.Night)
            {
                _currentDayTime = TimeOfDay.Day;
                UIManager.Instance.UpdateTimeOfDay(true);
                ObjectSpawner.Instance.SpawnObjects();
            }
        }
        else
        {
            if (_currentDayTime == TimeOfDay.Day)
            {
                UIManager.Instance.UpdateTimeOfDay(false);
                _currentDayTime = TimeOfDay.Night;
            }
        }
    }

    public float CurrentHour()
    {
        return _timer / _dayDuration * 24f;
    }
    
    public float CurrentMin()
    {
        float totalHours = _timer / _dayDuration * 24;
        float totalMinutes = totalHours * 60;
        return totalMinutes % 60;
    }

    public bool IsDay()
    {
        return _currentDayTime == TimeOfDay.Day;
    }
}
