using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _thirstSlider;
    [SerializeField] private Slider _hungerSlider;
    
    [SerializeField] private TMP_Text _timeOfDayText;
    [SerializeField] private TMP_Text _timeSpeedText;
    private int _timeSpeed = 1;

    public static UIManager Instance { get; private set; }

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
    }
    
    public void UpdateTimeSpeed(bool higher)
    {
        if((_timeSpeed == 1 && !higher) ||
           (_timeSpeed >= 8 && higher)) 
            return;
        
        Time.timeScale = higher ? Time.timeScale * 2 : Time.timeScale / 2;
        _timeSpeed = higher ? _timeSpeed * 2 : _timeSpeed / 2;
        _timeSpeedText.text = "x" + _timeSpeed;
    }
    
    public void UpdateTimeOfDay(bool isDay)
    {
        _timeOfDayText.text = isDay ? "Is Day" : "Is Night";
    }

    public void UpdateThirstSlider(int value)
    {
        _thirstSlider.value = value;
    }
    
    public void UpdateHungerSlider(int value)
    {
        _hungerSlider.value = value;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }


}
