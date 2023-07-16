using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public float GetZLevel() {
        return zLevel;
    }
    [SerializeField]
    float zLevel = 0;
    float triggerAmount = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Terrain")
            triggerAmount++;
        if (triggerAmount % 4 == 0) {
            zLevel = triggerAmount/4;
        }
        Debug.Log("New T: " + triggerAmount+" Z: " + zLevel);
    }
    private void OnTriggerExit2D(Collider2D other)
    {   
        
        if (other.tag == "Terrain")
            triggerAmount--;
        if (triggerAmount % 4 == 0) {
            zLevel = triggerAmount/4;        
        }
        Debug.Log("Old T: " + triggerAmount+" Z: " + zLevel);
    }

}
