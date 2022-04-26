using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObject : MonoBehaviour
{   //The protected keyword is to ensure the variable can be read in its child classes.
    protected string nameOfObjectAttached; //This was origianly defined in a child class, but it is basically the name for this type.
    protected float mass; //need to see if it needs to be changed to public
    protected GameObject Object;
    protected Vector3 orbitPlaneVector; //The vector of the plane that this object is orbiting in.
    protected Vector3 netForce = new Vector3(0.0f, 0.0f, 0.0f);
    protected string orbitTargetName;// This is the name of the astronomical object that this object is orbiting.
    protected CelestialObject orbitTarget;
    protected Rigidbody rigidbody;    
    protected bool setVelocity; //This checks if this object has its velocity set by data in data.txt. If it has than this value will be set to true.
    protected string type;
    protected Vector3 velocityDirection;
    protected float semiMajorAxis;
    protected Vector3 obliquity; //20220330
    protected float rotationalVelocity; //20220330 The angular velocity of the planets on its own axis.

    //The keyword virtual is to enable the feature of override in its child classes
    //I think the names of the functions are self explanatory.
    public virtual void setStart()
    {
        Object = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Object.SetActive(false); //This is to make sure that the physics etc are not premuturally used by the code for any reason. 
                                //I want near absolute control as to when anything happens and in what order they happen. I don't want to fall for unity cutting corners.
        Object.AddComponent<Rigidbody>();
        Object.GetComponent<Rigidbody>().useGravity = false;
        rigidbody = Object.GetComponent<Rigidbody>();
        orbitPlaneVector = new Vector3(0f,1f,0f);
        setVelocity = false;
    }

    public void setGameObject(GameObject go)// There is no need to set gameobjects from data for prefabs and initialprefabs.
    {
        Object = go;
        Object.SetActive(false);
    } 
    public virtual GameObject getGameObject()
    {
        return Object;
    }

    public virtual void setName(string n)
    {
        nameOfObjectAttached = n;
    }
    public virtual string getName()
    {
        return nameOfObjectAttached;
    }

    public virtual void setNetForce(Vector3 f)
    {
        netForce += f;
    }
    public virtual Vector3 getNetForce()
    {
        return netForce;
    }
    public virtual void clearNetForce() //Force is a property of the object at that specific point and not additive from its previous.
    {
        netForce = Vector3.zero;
    }

    public virtual void setOrbitTargetName(string n)
    {
        orbitTargetName = n;
    }
    public virtual string getOrbitTargetName()
    {
        return orbitTargetName;
    }
    public virtual void setOrbitTarget(CelestialObject co)
    {
        orbitTarget = co;
    }
    public virtual CelestialObject getOrbitTarget()
    {
        return orbitTarget;
    }

    public virtual void setOrbitPlaneVector(Vector3 vector)
    {
        orbitPlaneVector = vector;
    }
    public virtual Vector3 getOrbitPlaneVector()
    {
        return orbitPlaneVector;
    }
    public virtual Rigidbody getRigidbody()
    {
        return rigidbody;
    }    
    public virtual void hasSetVelocity()
    {
        setVelocity = true;
    }
    public virtual bool getSetVelocityStatus()
    {
        return setVelocity;
    }
    public virtual void setType(string t)
    {
        type = t;
    }

    public virtual string getType()
    {
        return type;
    }
    public virtual void setVelocityDirection(Vector3 d)
    {
        velocityDirection = d;
    }
    public virtual Vector3 getVelocityDirection()
    {
        return velocityDirection;
    }
    public virtual void setSemiMajorAxis(float axis)
    {
        semiMajorAxis = axis;
    }
    public virtual float getSemiMajorAxis()
    {
        return semiMajorAxis;
    }
    public virtual void setObliquity(Vector3 o) //20220330
    {
        obliquity = o;
    }
    public virtual Vector3 getObliquity()   //20220330
    {
        return obliquity;
    }
    public virtual void setRotationalVelocity(float f) //20220330
    {
        rotationalVelocity=f;
    }
    public virtual float getRotationalVelocity() //202220330
    {
        return rotationalVelocity;
    }
}