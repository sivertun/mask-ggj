using UnityEngine;

public class EndManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        EndLevel();
    }
    
    private void EndLevel()
    {
        LevelManager.Instance.EndLevel();
    }
}
