using UnityEngine;


public class DeathBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mask") || other.transform.Find("Mask") != null)
        {
            LevelManager.Instance.RestartLevel();
        }
        else if (other.gameObject.CompareTag("NPC"))
        {
            Destroy(other.gameObject);
        }
    }
}
