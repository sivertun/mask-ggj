using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;
    
    private bool hasStarted = false;
    
    private void Update()
    {
        if (hasStarted)
            return;
        
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            hasStarted = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        fadeGroup.alpha = 1f;
        fadeGroup.blocksRaycasts = true;
        
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        
        fadeGroup.alpha = 0f;
        LevelManager.Instance.LoadNextLevel();
    }
}
