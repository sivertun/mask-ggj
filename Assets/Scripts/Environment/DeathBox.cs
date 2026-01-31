using UnityEngine;


public class DeathBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("DeathBox is active now.");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("NPC"))
        {
            Debug.Log(other.gameObject.name + " has died.");
            Destroy(other.gameObject);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
