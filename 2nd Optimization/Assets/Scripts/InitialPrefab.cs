using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPrefab : CelestialObject
{
    bool hasDataBeenInitialized = false; // This is unique to Initial Prefabs. The reason for this function was given in the main.cs
    // string nameOfObjectAttached;
    // public float mass;
    // GameObject Object;
    // Rigidbody rigidbody;
    // string orbitTargetName; // This is the name of the astronomical object that this object is orbiting.
    

    public override void setStart()
    {
        nameOfObjectAttached = this.name;
        Object = this.gameObject;
        rigidbody = Object.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true; //Basically the same reason as to why I set gameobject inactive in its parent class. Although here gameobject is NOT inactive.
        rigidbody.useGravity = false;
        orbitPlaneVector = new Vector3(0f,1f,0f);
    }
    public void dataHasBeenInitialized()
    {
        hasDataBeenInitialized = true;
    }

    public bool getDataInitializedState()
    {
        return hasDataBeenInitialized;
    }
    // public GameObject getGameObject()
    // {
    //     return Object;
    // }

    // public void setName(string n)
    // {
    //     nameOfObjectAttached = n;
    // }
    // public string getName()
    // {
    //     return nameOfObjectAttached;
    // }

    // public Rigidbody getRigidbody()
    // {
    //     return rigidbody;
    // }

    // public void setOrbitTargetName(string n)
    // {
    //     orbitTargetName = n;
    // }
    // public string getOrbitTargetName()
    // {
    //     return orbitTargetName;
    // }
}
