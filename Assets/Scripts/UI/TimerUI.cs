using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;


    private void Update()
    {
        if (!LevelManager.Instance) return;
        UpdateTimer(LevelManager.Instance.GetTimer());
    }
    
    private void UpdateTimer(float newTime)
    {
        timerText.text = GetTimerText(newTime); 
    }
    
    
    private string GetTimerText(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int centiseconds = (int)((time - (int)time) * 100);

        return $"{minutes:00}:{seconds:00}:{centiseconds:00}";
    }
}
