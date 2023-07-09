using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private CapsuleCollider2D Collider1;
    [SerializeField]
    private CapsuleCollider2D Collider2;
    [SerializeField]
    private CapsuleCollider2D Trigger1;
    [SerializeField]
    private CapsuleCollider2D Trigger2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CapsuleCollider2D>()!=Collider1)
        Collider1.enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CapsuleCollider2D>()!=Collider1)
        Collider1.enabled = true;
    }

}
