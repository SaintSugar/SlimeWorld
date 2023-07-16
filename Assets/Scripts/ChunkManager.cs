using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    [SerializeField]
    private List<TileBase> TileList = new List<TileBase>();
    [SerializeField]
    private GameObject chunkLevel;
    [SerializeField]
    private int maxHeight;
    [SerializeField]
    private int chunkSize;
    [SerializeField]
    private List<GameObject> chunkLayers = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        chunkLevel.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        chunkLayers.Add(Instantiate(chunkLevel));
        chunkLayers[0].transform.parent = gameObject.transform;
        Destroy(chunkLayers[0].transform.Find("Rock").gameObject);

        Destroy(chunkLayers[0].transform.Find("TriggerJumpingLevel").gameObject);
        Destroy(chunkLayers[0].transform.Find("TriggerWalkingLevel0").gameObject);
        Destroy(chunkLayers[0].transform.Find("TriggerWalkingLevel1").gameObject);
        Destroy(chunkLayers[0].transform.Find("RockCollisions0").gameObject);

        for (int i = 0; i <= maxHeight; i++) {
            chunkLevel.transform.position = new Vector3(transform.position.x, transform.position.y, i);
            chunkLayers.Add(Instantiate(chunkLevel));
            chunkLayers[i+1].transform.parent = gameObject.transform;
            chunkLayers[i+1].transform.Find("Rock").gameObject.layer = LayerMask.NameToLayer("Collisions" + (i+1)%2);
            chunkLayers[i+1].transform.Find("TriggerWalkingLevel0").gameObject.layer = LayerMask.NameToLayer("Collisions" + (i)%2);
            chunkLayers[i+1].transform.Find("TriggerWalkingLevel1").gameObject.layer = LayerMask.NameToLayer("Collisions" + (i+1)%2);
            chunkLayers[i+1].transform.Find("RockCollisions0").gameObject.layer = LayerMask.NameToLayer("Collisions" + (i)%2);
            //Debug.Log(i + ":" + "RockCollisions0: " + LayerMask.LayerToName(chunkLayers[i+1].transform.Find("RockCollisions0").gameObject.layer));

        }
        
        for (int x=0; x < chunkSize; x++) {
            for (int y=0; y < chunkSize; y++) {
                //SetTileOnLayer(0, "Grass", -1, new Vector3Int(x, y, 0));
                int localHeight = GetHeight(x, y);
                //Debug.Log(localHeight);
                //int localHeight = 1;
                for (int i = -1; i <= localHeight; i++) {
                    if (i!=-1)
                        SetTileOnLayer(2, "Rock", i, new Vector3Int(x, y, 0));
                    SetTileOnLayer((i+1)%2, "Grass", i, new Vector3Int(x, y, 0));

                    SetTileOnLayer(3, "TriggerWalkingLevel0", i, new Vector3Int(x, y, 0));
                    SetTileOnLayer(4, "TriggerWalkingLevel1", i, new Vector3Int(x, y, 0));
                    SetTileOnLayer(4, "TriggerJumpingLevel", i, new Vector3Int(x, y, 0));
                }

                SetTileOnLayer(5, "RockCollisions0", localHeight, new Vector3Int(x, y, 0));
                if (y != 0 && GetHeight(x, y-1) == localHeight) {
                    float r = (Mathf.PerlinNoise((float)(x + 3^x)/2f, (float)(y + 5^y)/2f) + Mathf.PerlinNoise((float)(x+x^3)/2f, (float)(y + y^5)/2f)) * 50;
                    if (r < 20) {
                        SetTileOnLayer(6, "Trees", localHeight, new Vector3Int(x, y, 0));
                        //Debug.Log(x + ", "+y);
                    }
                    r = (Mathf.PerlinNoise((float)(x + 6^x)/2f, (float)(y + 7^y)/2f) + Mathf.PerlinNoise((float)(x+x^6)/2f, (float)(y + y^7)/2f)) * 50;
                    if ( r < 40) {
                        SetTileOnLayer(7, "Deco", localHeight, new Vector3Int(x*2, y*2, 0));
                    }
                }

            }
        }
    }
    int GetHeight(int x, int y) {
        return (int)Math.Round(((Mathf.Pow(Mathf.PerlinNoise((float)x/30, (float)y/30), 4) + 0.00*Mathf.PerlinNoise((float)x/2, (float)y/2))/1) * (maxHeight+1)) - 1;
    }
    void SetTileOnLayer(int tile, string layer, int z, Vector3Int position) {
        Tilemap t = chunkLayers[z+1].transform.Find(layer).GetComponent<Tilemap>();
        if (t == null) return;
        t.SetTile(position ,TileList[tile]);
    }
}
