using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame

    public AnimationClip StateRun;
    public AnimationClip StateRunSide;
    public AnimationClip StateRunBack;

    public AnimationClip StateIdle;
    public AnimationClip StateIdleSide;
    public AnimationClip StateIdleBack;
    

    public float speed;
    public bool Gravity;
    void LateUpdate()
    {
        Animator PlayerAnimator = GetComponent<Animator>();
        Vector2 CurrentSpeed = GetComponent<Rigidbody2D>().velocity;
        if (CurrentSpeed.magnitude > speed){
            PlayerAnimator.SetBool("Idle", false);
            if (Mathf.Abs(CurrentSpeed.x) > Mathf.Abs(CurrentSpeed.y) || Gravity) {
                PlayerAnimator.Play(StateRunSide.name);
                if (CurrentSpeed.x < 0)
                    GetComponent<SpriteRenderer>().flipX = true;
                else if (CurrentSpeed.x > 0)
                    GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (Mathf.Abs(CurrentSpeed.x) < Mathf.Abs(CurrentSpeed.y) && !Gravity) {
                if (CurrentSpeed.y < 0) {
                    PlayerAnimator.Play(StateRun.name);
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (CurrentSpeed.y > 0) {
                    PlayerAnimator.Play(StateRunBack.name);
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        else
            PlayerAnimator.SetBool("Idle", true);
    }
}
