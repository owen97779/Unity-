using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newton_Gravitation : MonoBehaviour
{  
    //Calling Rigidbody 
    public Rigidbody rb;
    // Update is called once per frame
    void FixedUpdate() 
    {
        Newton_Gravitation[] objects = FindObjectsOfType<Newton_Gravitation>();
        foreach (Newton_Gravitation i in objects)
        {
            if (i != this)
                Attraction(i);
        }

    }
    void Attraction (Newton_Gravitation objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;
        float forceMagnitude = (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;
        rbToAttract.AddForce(force);
    }
}
