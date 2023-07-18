using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{   
    [SerializeField]
    private float speed;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    [SerializeField]
    private bool flip;
    private float maximumSpeed;
    private PlayerControler playerControler;
    private EntityProperties.AnimationStates animationStates;
    void Start() {
        playerControler = GetComponent<PlayerControler>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        GameObject entity = Helper.FindChildWithTag(gameObject, "Entity");
        spriteRenderer = entity.GetComponent<SpriteRenderer>();
        animator = entity.GetComponent<Animator>();
        maximumSpeed = entity.GetComponent<EntityProperties>().abilities.maximumSpeed;
        animationStates = entity.GetComponent<EntityProperties>().animationStates;
        

    }
    bool isJumping = false;
    void LateUpdate()
    {
        Vector2 CurrentSpeed = rigidbody2D.velocity;
        if (playerControler.IsJumping() && !isJumping) {
            animator.speed = 1f;
            animator.Play(animationStates.StateJump.name);
            isJumping = true;
            return;
        }
        else if (!playerControler.IsJumping()) {
            isJumping = false;
        }
        else if (isJumping) {
            return;
        }
        if (CurrentSpeed.magnitude > speed){
            animator.speed = CurrentSpeed.magnitude/maximumSpeed;
            if (Mathf.Abs(CurrentSpeed.x) > Mathf.Abs(CurrentSpeed.y)) {
                animator.Play(animationStates.StateRunSide.name);
                if (CurrentSpeed.x < 0)
                    spriteRenderer.flipX = true&&flip;
                else if (CurrentSpeed.x > 0)
                    spriteRenderer.flipX = false&&flip;
            }
            else if (Mathf.Abs(CurrentSpeed.x) < Mathf.Abs(CurrentSpeed.y)) {
                if (CurrentSpeed.y < 0) {
                    animator.Play(animationStates.StateRunForward.name);
                }
                else if (CurrentSpeed.y > 0) {
                    animator.Play(animationStates.StateRunBack.name);
                }
            }
        }
        else {
            animator.speed = 1f;
            animator.Play(animationStates.StateIdle.name);
        }
    }
}
