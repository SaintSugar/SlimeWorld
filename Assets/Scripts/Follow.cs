using UnityEngine;
using System;
 

public class Follow : MonoBehaviour {

    public Transform Target;
    public float Radius;
    public float Elasticity;
    public bool InfititeElasticity;
    

    // Update is called once per frame
    void LateUpdate () {
        float xp, yp, xc, yc;
        xp = Target.transform.position.x;
        yp = Target.transform.position.y;

        xc = transform.position.x;
        yc = transform.position.y;

        Vector2 pointer = new Vector2((xc-xp), (yc-yp)).normalized;
        
        float xo, yo;   

        xo = pointer.x * Radius + xp;
        yo = pointer.y * Radius + yp;



        float dist;
        dist = Mathf.Sqrt(Mathf.Pow(xp-xc, 2) + Mathf.Pow(yp-yc, 2));   
        Vector2 V = GetComponent<Rigidbody2D>().velocity;
        if (dist > Radius) {
            V = new Vector2((xo-xc), (yo-yc))*Elasticity;
        }
        else {
            V = new Vector2(0, 0);
        }
        if (!InfititeElasticity) {
            GetComponent<Rigidbody2D>().velocity = V;
        }
        else {
            if (dist > Radius) {
                transform.position = new Vector3(xo, yo, 0) + new Vector3(0, 0, -10);
            }
        }
    }
}
