
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndUI : UIScreen
{
    private LevelManager levelManager;

    [SerializeField] private TextMeshProUGUI endTimer;
    
    bool levelCompleted = false;
    private void Start()
    {
        Hide();
        levelManager = LevelManager.Instance;
        levelManager.onLevelComplete += OnLevelComplete;
    }

    public void OnLevelComplete()
    {
        Show();
        float timer = levelManager.GetTimer();
        endTimer.text = GetTimerText(timer);
        levelCompleted = true;
    }

    private void Update()
    {
        if(!levelCompleted) return;
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            levelCompleted = false;
            Hide();
            levelManager.LoadNextLevel();
        }
    }
    
    private string GetTimerText(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int centiseconds = (int)((time - (int)time) * 100);

        return $"{minutes:00}:{seconds:00}:{centiseconds:00}";
    }
    
    private void OnDestroy()
    {
        if (levelManager != null)
        {
            levelManager.onLevelComplete -= OnLevelComplete;
        }
    }
    
    
}
