using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody Sphere;
    public Vector3 velocity;
    Vector3 Force;
    void Start()
    {
        Force = Vector3.zero;
        Sphere.velocity = velocity;
    }

    public Rigidbody GetRigidbody()
    {
        return Sphere;
    }

    public void SetForce(Vector3 force)
    {
        Force = force + Force;
    }

    public void ClearForce()
    {
        Force = Vector3.zero;
    }

    public void Movement()
    {
        Sphere.AddForce(Force);
    }

}
