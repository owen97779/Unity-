using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitation : MonoBehaviour
{
    //public GameObject Planet;
    public float G;
    Planets[] objects;

    // Start is called before the first frame update
    void Start()
    {
        objects = FindObjectsOfType<Planets>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (var i = 0; i < objects.Length-1; i++)
        {
            for (var j = i+1; j < objects.Length; j++)
            {
                Force(i,j);
            }
        }
        for (var i = 0; i < objects.Length; i++)
        {Movement(i);}
    }

    void Force(int i, int j)
    {
        Planets ObjectSubject= objects[i];
        Planets ObjectObject = objects[j];
        Rigidbody rb1 = ObjectSubject.GetRigidbody();
        Rigidbody rb2 = ObjectObject.GetRigidbody();
        Vector3 position1 = rb1.position;
        Vector3 position2 = rb2.position;
        Vector3 force = G*rb1.mass*rb2.mass*((position2-position1).normalized)/(Mathf.Pow((position2-position1).magnitude,2));
        ObjectSubject.SetForce(force);
        ObjectObject.SetForce(force * (-1));
    }
    void Movement(int i)
    {
        objects[i].Movement();
        objects[i].ClearForce();
    }
        
    
}
