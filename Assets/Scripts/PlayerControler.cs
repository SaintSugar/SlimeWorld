using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerControler : NetworkBehaviour
{
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float Acceleration;

    private float[] ForcePull;
    private bool jumpedAlready = false;
    SpriteRenderer emotion;
    void Start()
    {   
        emotion = transform.Find("SlimeEmotion").GetComponent<SpriteRenderer>();
        ForcePull = new float[2];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
        Rig = GetComponent<Rigidbody2D>();
        if (IsServer) {
            //isJumping.Value = true;
        }
        jumpTimer = 0.1f;
        gameObject.layer = LayerMask.NameToLayer("PlayerCollisionJump");
    }
    private Rigidbody2D Rig;
    private NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(writePerm:NetworkVariableWritePermission.Server);
    private NetworkVariable<Vector3> Velocity = new NetworkVariable<Vector3>(writePerm:NetworkVariableWritePermission.Server);
    private NetworkVariable<Vector2> Control = new NetworkVariable<Vector2>(writePerm:NetworkVariableWritePermission.Owner, readPerm:NetworkVariableReadPermission.Owner);
    public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(writePerm:NetworkVariableWritePermission.Server, readPerm:NetworkVariableReadPermission.Everyone);
    private NetworkVariable<bool> jumpControl = new NetworkVariable<bool>(writePerm:NetworkVariableWritePermission.Owner, readPerm:NetworkVariableReadPermission.Owner);

    private NetworkVariable<bool> emotionControl = new NetworkVariable<bool>(writePerm:NetworkVariableWritePermission.Owner, readPerm:NetworkVariableReadPermission.Everyone);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Position.OnValueChanged += (Vector3 a, Vector3 b) =>
        {    
            if ((transform.position - Position.Value).magnitude > 3) {
                transform.position = Position.Value;
            }
                
        };
        isJumping.OnValueChanged += (bool a, bool b) =>
        {    
            
        };
    }

    void FixedUpdate()
    {   
        if (IsOwner)
            emotionControl.Value = Input.GetAxis("Fire2")!=0;
        emotion.enabled = emotionControl.Value;
        float zLevel = GetComponent<PlayerCollider>().GetZLevel();
        if (!(jumpTimer <= 0.75-0.125 && jumpTimer >= 0.75-0.125-0.5)) 
        {}
            transform.position = new Vector3(transform.position.x, transform.position.y, zLevel);
        
        
        if (!(jumpTimer <= 0.75-0.125 && jumpTimer >= 0.75-0.125-0.5)) {
            if (Math.Round(zLevel)%2 == 0)
                gameObject.layer = LayerMask.NameToLayer("PlayerCollision0");
            else
                gameObject.layer = LayerMask.NameToLayer("PlayerCollision1");
        }
        if (IsOwner) {
            Vector2 control = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Control.Value = control;
            Move(control);
            if (Input.GetAxis("Jump")!=0)
                jumpControl.Value = true;
            else jumpControl.Value = false;
        }
        if (IsServer) {
            if (MathF.Abs(Control.Value.x) <= 1 && MathF.Abs(Control.Value.y) <= 1)
                Move(Control.Value);
            if (jumpTimer <= 0)
                if (jumpControl.Value)
                    isJumping.Value = true;
                else isJumping.Value = false;
            Jump();
            Position.Value = transform.position;
        }
        else {
            if (!IsOwner)
                Rig.velocity = Velocity.Value;
            Jump();
            if (transform.position.z != Position.Value.z)
                transform.position = Position.Value;
        }
        
    }
    private void Move(Vector2 control) {
        Vector2 current_velocity = Rig.velocity;
        Vector2 new_velocity = Rig.velocity;
        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);
        new_velocity.y = speedCheck(current_velocity.y, control.y, new_velocity.y, 1);
        Rig.velocity = new_velocity;
        if (IsServer) {
            Velocity.Value = new_velocity;
        }
    }
    private float jumpTimer = -1;
    [SerializeField]
    private float jumpTime = 0.75f;
    private void Jump() {
        
        if (!isJumping.Value) jumpedAlready = false;
        if (isJumping.Value && !jumpedAlready) {
            jumpTimer = jumpTime;
            jumpedAlready = true;
            //Helper.FindChildWithTag(gameObject, "Entity").transform.position = new Vector3(Helper.FindChildWithTag(gameObject, "Entity").transform.position.x, Helper.FindChildWithTag(gameObject, "Entity").transform.position.y, transform.position.z + 1);
        }
        if (jumpTimer > 0) {
            float H = 6;
            float Vo = 4*H/jumpTime;
            float G = 2 * Vo /jumpTime;
            float V = Vo - G * (jumpTime - jumpTimer);

            Helper.FindChildWithTag(gameObject, "Entity").transform.position += new Vector3(0, V, 0) * Time.deltaTime;
            jumpTimer -= Time.deltaTime;
            
            if (jumpTimer <= 0.75-0.125 && jumpTimer >= 0.75-0.125-0.5) {
                gameObject.layer = LayerMask.NameToLayer("PlayerCollisionJump");
            }
            else if (jumpTimer < 0.75-0.125-0.5) {
                
            }
            if (jumpTimer <= 0) {
                Helper.FindChildWithTag(gameObject, "Entity").transform.position = transform.position;
            }
            
            
        }
    }

    float speedCheck(float current_velocity, float control, float new_velocity, int axis) {
        if (ForcePull[axis] == 0 && (Mathf.Abs(current_velocity) > Speed)) {
                    ForcePull[axis] = current_velocity / Mathf.Abs(current_velocity);
                }
        else if (ForcePull[axis] * (current_velocity - control * Speed) <= 0 && ForcePull[axis] != 0) {
                    ForcePull[axis] = 0;
                    new_velocity = control * Speed;
                }

        if ((Mathf.Abs(current_velocity) <= Mathf.Abs(control) * Speed || control * current_velocity < 0) && ForcePull[axis] == 0) {
            new_velocity = control * Speed;
        }
        else if (((ForcePull[axis] * (current_velocity - control * Speed) > 0 && ForcePull[axis] * control < 0)) && ForcePull[axis] != 0) 
            Rig.AddForce(new Vector2(Acceleration * control * (Mathf.Abs(axis - 1)), Acceleration * control * axis));
                
                
        return new_velocity;
    }
    public float GetMaximumSpeed() {
        return Speed;
    }
    public bool IsJumping() {
        return (jumpTimer > 0);
    }
}