using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{   

    public AnimationClip StateRun;
    public AnimationClip StateRunSide;
    public AnimationClip StateRunBack;

    public AnimationClip StateIdle;
    public AnimationClip StateIdleSide;
    public AnimationClip StateIdleBack;
    public AnimationClip StateJump;
    
    [SerializeField]
    private float speed;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    [SerializeField]
    private bool flip;
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private float maximumSpeed;
    private PlayerControler playerControler;
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (isPlayer)
            playerControler = GetComponent<PlayerControler>();
            maximumSpeed = playerControler.GetMaximumSpeed();

        

    }
    bool isJumping = false;
    void LateUpdate()
    {
        Vector2 CurrentSpeed = rigidbody2D.velocity;
        if (playerControler.IsJumping() && !isJumping) {
            animator.speed = 1f;
            animator.Play(StateJump.name);
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
            //animator.SetBool("Idle", false);
            animator.speed = CurrentSpeed.magnitude/maximumSpeed;
            if (Mathf.Abs(CurrentSpeed.x) > Mathf.Abs(CurrentSpeed.y)) {
                animator.Play(StateRunSide.name);
                if (CurrentSpeed.x < 0)
                    spriteRenderer.flipX = true&&flip;
                else if (CurrentSpeed.x > 0)
                    spriteRenderer.flipX = false&&flip;
            }
            else if (Mathf.Abs(CurrentSpeed.x) < Mathf.Abs(CurrentSpeed.y)) {
                if (CurrentSpeed.y < 0) {
                    animator.Play(StateRun.name);
                }
                else if (CurrentSpeed.y > 0) {
                    animator.Play(StateRunBack.name);
                }
            }
        }
        else {
            animator.speed = 1f;
            animator.Play(StateIdle.name);
        }
            //
            //animator.SetBool("Idle", true);
    }
}
