using UnityEngine;

public class DeathBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("DeathBox is active.");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that was hit is the DeathZone
        if (other.CompareTag("Player")) 
        {   
            Debug.Log("Player has died!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
