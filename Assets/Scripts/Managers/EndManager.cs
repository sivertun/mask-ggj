using UnityEngine;

public class EndManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("TEST START MESSAGE");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        EndLevel();
        Debug.Log("TEST MESSAGE");
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void EndLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
