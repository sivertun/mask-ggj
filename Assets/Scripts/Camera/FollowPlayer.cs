using System;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private float distanceFromPlayer = 10f;
    
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
        transform.position = playerTransform.position + new Vector3(0, 0, -distanceFromPlayer);
        mainCamera.orthographicSize = distanceFromPlayer;
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
