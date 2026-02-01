using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    private int currentLevelIndex = -1;
    
    [SerializeField] List<string> levels = new List<string>();
    
    private InputAction restartAction;

    private bool timerActive = false;
    private float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timer = 0;
        restartAction = InputSystem.actions.FindAction("Restart");
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        timer = 0f;
        timerActive = true;
    }

    public void RestartLevel()
    {
        if (currentLevelIndex == -1) return;
        LoadLevel(levels[currentLevelIndex]);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count) return;
        
        string nextLevel = levels[currentLevelIndex];
        LoadLevel(nextLevel);
    }

    private void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
        }
        
        if(currentLevelIndex == -1) return;
        
        if (restartAction.WasPressedThisFrame())
        {
            RestartLevel();
        }
    }

    public float GetTimer()
    {
        return timer;
    }
}
