using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Gravitation : MonoBehaviour
{
    //public GameObject Planet;
    public float G;
    Planets[] objects;
    //Create a slider that changes the G value based on time
    public void Slider_change_G(float newValue)
    {

        G = 0.00088995511377f / (Mathf.Pow(1/newValue, 2));
        Debug.Log(G);

    }
    public void SetVelocity(Planets[] objects)
    {
        for(var i = 0; i < objects.Length; i++)
        {
            Rigidbody rb = objects[i].GetRigidbody();
            //oldVelocity = rb.velocity;
            //rb.velocity = new Vector3 (oldVelocity.x)
            


        }

        
    }


    // Start is called before the first frame update lol
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
                if (G != 3){
                    SetVelocity(objects);
                }
                Force(i,j);
            }
        }
        for (var i = 0; i < objects.Length; i++)
        {
            Movement(i);
        }
        //Debug.Log(objects[2].GetRigidbody().velocity);
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
