using System;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        Attach();
    }

    private void Update()
    {
        if (!playerTransform)
        {
            Attach();
            if(!playerTransform) return;
        }
        transform.position = playerTransform.position + offset;
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
