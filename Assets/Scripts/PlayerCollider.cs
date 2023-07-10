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
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("New");
        if (other.tag == "Terrain")
            zLevel++;
    }
    private void OnTriggerExit2D(Collider2D other)
    {   
        Debug.Log("Old");
        if (other.tag == "Terrain")
            zLevel--;
    }

}
