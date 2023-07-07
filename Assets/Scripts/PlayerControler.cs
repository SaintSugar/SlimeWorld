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
 
    void Start()
    {   
        ForcePull = new float[2];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
        Rig = GetComponent<Rigidbody2D>();
    }
    private Rigidbody2D Rig;
    private NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>(writePerm:NetworkVariableWritePermission.Server);
    private NetworkVariable<Vector2> Velocity = new NetworkVariable<Vector2>(writePerm:NetworkVariableWritePermission.Server);
    private NetworkVariable<Vector2> Control = new NetworkVariable<Vector2>(writePerm:NetworkVariableWritePermission.Owner, readPerm:NetworkVariableReadPermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Position.OnValueChanged += (Vector2 a, Vector2 b) =>
        {    
            if ((new Vector2(transform.position.x, transform.position.y) - Position.Value).magnitude > 3) {
                transform.position = Position.Value;
            }
        };
    }

    void FixedUpdate()
    {   
        if (IsOwner) {
            Vector2 control = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Control.Value = control;
            Move(control);
        }
        if (IsServer) {
            Move(Control.Value);
            Position.Value = transform.position;
        }
        else {
            Rig.velocity = Velocity.Value;
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
}