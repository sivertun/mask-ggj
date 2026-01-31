using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;  


public class DeathBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("DeathBox is active now.");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         
        if (other.gameObject.CompareTag("Mask") || other.gameObject.CompareTag("NPC"))
        {
            if (LevelManager.Instance != null)
            {
                Debug.Log("DeathBox: Restarting level due to collision with " + other.gameObject.tag);
                LevelManager.Instance.RestartLevel();
            }
            else
            {
                Debug.Log("DeathBox: No LevelManager found in the scene!");
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
