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
        restartAction = InputSystem.actions.FindAction("Restart");
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void RestartLevel()
    {
        LoadLevel(levels[currentLevelIndex]);;
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
        if(currentLevelIndex == -1) return;
        
        if (restartAction.WasPressedThisFrame())
        {
            RestartLevel();
        }
    }
}
