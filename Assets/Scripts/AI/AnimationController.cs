using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Sprite[] movementSprites;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        GetComponentInParent<EntityMovement>().MoveDirectionChangHandler += MoveDirectionChange;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void MoveDirectionChange(MoveDirections newMoveDirection)
    {
        spriteRenderer.sprite = movementSprites[(int)newMoveDirection];
    }
}
