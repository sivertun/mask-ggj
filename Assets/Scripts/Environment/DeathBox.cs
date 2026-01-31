using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.LevelManager;

public class DeathBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartPosition();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that was hit is the DeathZone
        if (other.CompareTag("Player")) 
        {   
            other.GetComponent<PlayerController>().resetPosition();
            Debug.Log("Player has died!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
