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
    
    private float timer = 0f;
    
    public event Action onLevelComplete;

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
    }

    public void RestartLevel()
    {
        if (currentLevelIndex == -1) return;
        LoadLevel(levels[currentLevelIndex]);
    }

    public void EndLevel()
    {
        onLevelComplete?.Invoke();
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            currentLevelIndex = -1;
            SceneManager.LoadScene("MainMenu");
        };
        
        string nextLevel = levels[currentLevelIndex];
        LoadLevel(nextLevel);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
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
