using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private new string tag = null;

    // Update is called once per frame
    public string GetLayerTag()
    {
        return tag;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("NEW COLLISION!");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        tag = other.tag;
        //Debug.Log(tag);
    }

}
