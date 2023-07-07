using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerControler : NetworkBehaviour
{
    public float Speed;
    public float Acceleration;

    private float[] ForcePull;
 
    void Start()
    {   
        ForcePull = new float[2];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
        Rig = GetComponent<Rigidbody2D>();
    }
    private Rigidbody2D Rig;
    private NetworkVariable<Vector2> networkControl = new NetworkVariable<Vector2>(writePerm:NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        networkControl.OnValueChanged += (Vector2 a, Vector2 b) =>
        {
            Debug.Log(message:$"{OwnerClientId}: control: {networkControl.Value}");

            
        };
    }
    void Update()
    {   
        if (IsOwner) {
            networkControl.Value = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            MoveServerRpc();
        }
        
    }
    [ServerRpc]
    private void MoveServerRpc() {
        Vector2 current_velocity = Rig.velocity;
        Vector2 new_velocity = Rig.velocity;
        Vector2 control = networkControl.Value;
        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);
        new_velocity.y = speedCheck(current_velocity.y, control.y, new_velocity.y, 1);
        Rig.velocity = new_velocity;
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