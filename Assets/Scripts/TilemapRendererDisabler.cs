using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRendererDisabler : MonoBehaviour
{
    [SerializeField]
    private bool showCollisions = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TilemapRenderer>().enabled = showCollisions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
