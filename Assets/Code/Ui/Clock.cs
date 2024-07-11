using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;

    [SerializeField] private RectTransform _hour;
    [SerializeField] private RectTransform _minute;
    private float _hourToAngle = 360/12;
    private float _minuteToAngle = 360/60;

    private void Update()
    {
        _hour.rotation = Quaternion.Euler(0, 0, -_timeManager.CurrentHour() * _hourToAngle);
        _minute.rotation = Quaternion.Euler(0, 0, -_timeManager.CurrentMin() * _minuteToAngle);
    }


}
