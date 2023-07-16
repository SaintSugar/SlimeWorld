using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class MainPlayerSettings : NetworkBehaviour
{
    [SerializeField]
    private new GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner){
            //GetComponent<SpriteRenderer>().sortingLayerName = "MainPlayer";
            camera.GetComponent<Follow>().SetTarget(transform);
            Instantiate(camera);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
