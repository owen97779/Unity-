using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Gravitation : MonoBehaviour
{
    //public GameObject Planet;
    public float G;
    public float days;
    public float prevdays = 1;
    Planets[] objects;
	public Text outputtingTime;
    //Create a slider that changes the G value based on time
    public void Slider_change_G(float interval)
    {

        G = 0.00088995511377f / (1/(Mathf.Pow(interval, 2)));
        Debug.Log(G);
        days = interval;
        Debug.Log(days);
        SetVelocity();
        prevdays = days;
    }

    public void SetVelocity()
    {   
		//outputtingTime.text = days.ToString();
        for(int i =0; i < objects.Length; i++)
        {
            Planets eachPlanet = objects[i];
            Rigidbody rb = eachPlanet.GetRigidbody();
            Vector3 oldVelocity = rb.velocity;
            Debug.Log(days);
            rb.velocity = days/prevdays * oldVelocity;
			
            // float X = (days) * oldVelocity.x;
            // float Y = (days) * oldVelocity.y;
            // float Z = (days) * oldVelocity.z;
            // rb.velocity = new Vector3 (X,Y,Z);
            //Debug.Log(rb.velocity);
        }
        /* Planets ObjectSubject= objects[i];
        Planets ObjectObject = objects[j];
        Rigidbody rb1 = ObjectSubject.GetRigidbody();
        Rigidbody rb2 = ObjectObject.GetRigidbody();
        Vector3 oldVelocity1 = rb1.velocity;
        Vector3 oldVelocity2 = rb2.velocity;
        rb1.velocity = new Vector3 ((days * oldVelocity1.x), (days*oldVelocity1.y), (days*oldVelocity1.z));
        rb2.velocity = new Vector3 ((days * oldVelocity2.x), (days*oldVelocity2.y), (days*oldVelocity2.z)); */
        //Debug.Log(rb1.velocity.x);
        //Debug.Log(days);
        
    }


    // Start is called before the first frame update lol
    void Start()
    {
        objects = FindObjectsOfType<Planets>();
		Text outputtingTime = GameObject.Find("Canvas - HUD/HUD Parent/TextParent/Panel/Time Text").GetComponent<Text>();
		outputtingTime.text = "Fix this";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		
        if (days > 1)
        {
            //outputtingTime.text = "Test";
            //Debug.Log(objects[4].velocity);
        } 
        for (var i = 0; i < objects.Length-1; i++)
        {
            for (var j = i+1; j < objects.Length; j++)
            {    
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
