using System;
using Unity.VisualScripting;
using UnityEngine;

public class MaskSpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer maskSpriteRenderer;
    [SerializeField] private MaskMovement maskMovement;
    [SerializeField] private Sprite attachedSprite;
    [SerializeField] private Sprite looseSprite;

    private void Start()
    {
        maskMovement.OnAttachmentChanged += HandleAttachmentChanged;
        UpdateSprite(true);
    }

    private void HandleAttachmentChanged(bool isAttached)
    {
        UpdateSprite(isAttached);
    }


    private void UpdateSprite(bool isAttached)
    {
        if (isAttached)
        {
            maskSpriteRenderer.sprite = attachedSprite;
        } else
        {
            maskSpriteRenderer.sprite = looseSprite;
        }
    }
}
