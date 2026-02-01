using System;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private float distanceFromPlayer = 20f;
    [SerializeField] private float followSpeed = 5f;
    
    private Camera mainCamera;

    private void Start()
    {
        Attach();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!playerTransform)
        {
            Attach();
            if(!playerTransform) return;
        }
        
        Vector3 targetPosition =
            playerTransform.position + new Vector3(0, 5, -distanceFromPlayer);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            distanceFromPlayer,
            followSpeed * Time.deltaTime
        );
    }

    private void Attach()
    {
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player)
        {
            playerTransform = player;
        }
    }
}
