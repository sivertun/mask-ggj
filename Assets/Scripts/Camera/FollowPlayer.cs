using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private Vector3 offset;


    private void Update()
    {
        if (!playerTransform) return;
        transform.position = playerTransform.position + offset;
    }
}
