using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    public void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
