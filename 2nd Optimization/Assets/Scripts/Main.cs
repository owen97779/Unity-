using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;
using System.Linq;

public class Main : MonoBehaviour
{
    List<CelestialObject> CelestialObjects = new List<CelestialObject>();   //Create a List that will hold all celestial objects (Basically objects in space)
    List<CelestialObject> duplicatedCelestialObjects = new List<CelestialObject>();
    InitialPrefab[] InitialPrefabs;  //Create an array that will be populated with the InitialPrefabs types in the unity scene ==> Refer to the Initial Prefab class for propper meaning.
    public float G; //Gravitational constant
    public Text simulationSpeed;    //==>self explanatory
    float currentSimulationSpeed=1; //==>self explanatory, used in conjunction with simulationSpeed.
    bool mainSceneisPaused = false;
    // Start is called before the first frame update
    float updateFixedUpdateCountPerSecond;
    public Transform target;
    public float newG = 0.0008807525f;
    float maxVelocity = 0f;
    public float speed;

    void Start()
    {
        //Debug.Log("Main Scene");
        InitialPrefabs = FindObjectsOfType<InitialPrefab>();  //Gets all objects in the scene with the script InitialPrefab attached.
        initializePrefabs();//This step is such that the names and gameobjects can be correctly assigned to the relavent variables.
                            //It also creates the corresponding rigidbodies.
        foreach(InitialPrefab ip in InitialPrefabs)
        {CelestialObjects.Add(ip);} //This adds all InitialPrefabs into the big "catch all" tent of CelestialObjects list. It is possible because InitialObjects is a child class of CelestialObjects.
        string[] dataOfEachLineFromFile = System.IO.File.ReadAllLines(@"Assets/Data/data.txt"); //Reads the data in the file
        readFileData(dataOfEachLineFromFile, 0, 0); //The function that uses the data.
        setPositionToPerihelion();
        linkOrbitObjectNameToOrbitObjectandSetInitialVelocity(); // This creates a memory address with the type CelestialObject in each CelestialPbject
                                                                //such that it knows which object it is orbiting to. It also calculates the velocity of each object given its data.
        
        //SceneManager.LoadScene("TrajectoryScene", LoadSceneMode.Additive);
        //changeSimulationSpeed(50);
        
    }

    public void initializePrefabs()
    {
            foreach(InitialPrefab ip in InitialPrefabs)
            {
                ip.setStart(); // This is literally a way to get around the limitations of a constructor.
            }
    }

    void readFileData(string[] dataOfFile, int start, int end)  //start means the starting index (which line in the txt file) end means when to end reading (although it is not really used...I think)...
    {
        int startIndex = start; // I don't know how to comment these two lines ...
        int endIndex = end;
        bool isInitialPrefab = false; //this is used to make sure that the gameobject is not created if it has already been hardcoded.
        bool endtxt=false; //Checking if the end of the document has been reached.

        for (int i = start; i < dataOfFile.Length; i++)  //Not sure how to explain it. But it gets the start and end indicies of a desired block of data.
        {
            if (dataOfFile[i].Equals("ENDTXT"))
            {endtxt = true;}
            if (dataOfFile[i].Equals("START"))
            {
                startIndex = i;
            }
            if (dataOfFile[i].Contains("ISINITIALPREFAB"))
            {isInitialPrefab = true;}
            if (dataOfFile[i].Equals("END"))
            {
                endIndex = i;
                break;
            }
        }


        if(!endtxt && startIndex<endIndex) //This is a check to make sure it doesn't fall into a self recurring loop of fucntion (Which is not desired) == and the names of the variables are self explanitory as to why
        {
            string[] chunk = new string[endIndex-startIndex+1]; //This basically further makes the block of data corresponding to that object indexable (?is it a word)
            for (int j = 0; j<chunk.Length; j++)
            {
                chunk[j] = dataOfFile [j+startIndex];
            }
            implementDataOfFile(chunk,isInitialPrefab); //The names explains itself
        }



            for(int k = endIndex; k < dataOfFile.Length; k++)
            {
                if(endtxt) // A further check to make sure it doesn't fall into an infinite loop.
                {break;}
                // try 
                {
                    readFileData(dataOfFile, endIndex + 1, endIndex);   //recursive function. placing endIndex+1 and endIndex makes sure it doesn't get the same block of data.
                    break; // Don't know how to explain but it makes sure once it reaches the end of the file it will stop looping.
                }
                // catch (System.Exception e)
                // {Debug.Log(e);break;}

            }        
    }

    void implementDataOfFile(string[] blockOfData, bool isInitialPrefab)
    {
        if(isInitialPrefab) //I think the check is self explanatory...
        {
            applyDatatoPrefab(blockOfData);
        }
        if(!isInitialPrefab)
        {
            initializeAndCreateAccordingToData(blockOfData);
        }
    }

    void applyDatatoPrefab(string[] blockofData) // The main purpose of this block of code is to try and link the data with the corresponding ganeobject in the scene by getting its index.
    {
        int index = 0;
        string initPrefabNameAsDefinedInFile;
        bool gotObject = false;
        foreach(string data in blockofData)
        {   
            if (data.Contains("name"))
            {
                initPrefabNameAsDefinedInFile = retrieveVariableDataFromFileLine(data); //purpose of the function is defined in its section.
                foreach(InitialPrefab ip in CelestialObjects.OfType<InitialPrefab>()) //Checking if anything in CelestialObjects that have the type InitialPrefabs has the name corresponding to the name in the data txt.
                {
                    if (ip.getName().Equals(initPrefabNameAsDefinedInFile))
                    {
                        index = CelestialObjects.FindIndex((item => item == ip));
                        gotObject = true;
                        
                    }
                }                
            }
        }

        if (gotObject) //I think this check explains itself.
        {
            assignDatatoPrefabVariable(index, blockofData);
        }
        if (!gotObject)
        {
            initializeAndCreateAccordingToData(blockofData); //Treat is as it doesn't have a prefab
        }
    }

    string retrieveVariableDataFromFileLine(string data) //Bascically reading everything to the right of the first "=" of that line in the data txt. 
    {
        string trimmed = System.String.Concat(data.Where(c => !System.Char.IsWhiteSpace(c))); // This line of code that deletes all spaces was found online...
        int equalIndex = trimmed.IndexOf("=");
        return trimmed.Substring(equalIndex + 1);        
    }

    void assignDatatoPrefabVariable(int indexOfPrefabInInitialPrefabs, string[] blockofData)
    {
        InitialPrefab prefab = (InitialPrefab)CelestialObjects[indexOfPrefabInInitialPrefabs]; //This gets the corresponding gameobject stored in CelestialObjects at that index...
        foreach(string data in blockofData) //The rest of this section of code performs "what is should" to the given InitialPrefab.
        {
            string trimmedData = retrieveVariableDataFromFileLine(data); //Uses the function again...
            Rigidbody prefabRigidbody = prefab.getRigidbody(); //Rigidbodies are useful things in unity...
            switch (data)
            {
                case string mass when mass.Contains("mass"):
                    prefabRigidbody.mass=float.Parse(trimmedData);
                    break;
                
                case string orbits when orbits.Contains("orbits"):
                    prefab.setOrbitTargetName(trimmedData);
                    break;
                
                case string orbitPlane when orbitPlane.Contains("orbitPlane"):
                    Vector3 orbitPlaneVector = getInitialVectorData(trimmedData);
                    prefab.setOrbitPlaneVector(orbitPlaneVector);
                    break;

                case string position when position.Contains("position"):
                    Vector3 setPosiion = getInitialVectorData(trimmedData);
                    prefabRigidbody.transform.position = setPosiion;
                    break;

                case string radius when radius.Contains("radius"):
                    float r = float.Parse(trimmedData);
                    Vector3 setRadius = new Vector3(r,r,r);
                    prefab.getGameObject().transform.localScale = setRadius;                    
                    break;
                
                case string type when type.Contains("type"):
                    prefab.setType(trimmedData);
                    break;

                case string vel when vel.Contains("velocity"):
                    Vector3 velocity = getInitialVectorData(trimmedData);
                    prefabRigidbody.velocity = velocity;
                    prefab.hasSetVelocity();
                    break;

                case string perihelion when perihelion.Contains("perihelion"):
                    prefab.setPerihelion(float.Parse(trimmedData));
                    break;
                
                case string aphelion when aphelion.Contains("aphelion"):
                    prefab.setAphelion(float.Parse(trimmedData));
                    break;
                    
            }
        }
        prefab.dataHasBeenInitialized(); //This thing isn't actually used. The original purpose for this was to destroy the gameobject if no corresponding data was given to it in data txt...
    }

    Vector3 getInitialVectorData(string vectorData) //Converts x,y,z strings to vector3s.
    {
        int indexx = vectorData.IndexOf("x");
        int indexy = vectorData.IndexOf("y");
        int indexz = vectorData.IndexOf("z");

        float xValue = 0f, yValue = 0f, zValue = 0f;
        if (indexx != -1 && indexy != -1 && indexz !=-1)
        {
            // Debug.Log(vector);
            // Debug.Log(vector.Substring(indexx + 2, indexy - (indexx + 2)));
            xValue = float.Parse(vectorData.Substring(indexx + 2, indexy - (indexx + 2)));
            yValue = float.Parse(vectorData.Substring(indexy + 2, indexz-(indexy+2)));
            zValue = float.Parse(vectorData.Substring(indexz + 2));
        }
        Vector3 vec = new Vector3(xValue,yValue,zValue);
        return vec;            
    }

    void initializeAndCreateAccordingToData(string[] blockofData) //This is for the most general case where the objects are created purely from data txt (although). There are no InitialPrefabs or others/
    {
        CelestialObject co = new CelestialObject(); //Needs to create the "gameobject" <== strictly speaking it isn't a gameobject, but it is created in the CelestialObject class constructor. Currently it only creates spheres
        co.setStart(); //This is literally a way to get around limitations of the constructor.
        foreach(string data in blockofData) //This is basically the same as before...Tho it couldn't be merged into one function due to several small tweaks. The main difference is there is a function that assigns the name.
        {
            string trimmedData = retrieveVariableDataFromFileLine(data);
            Rigidbody objectRigidbody = co.getRigidbody();
            switch (data)
            {

                case string mass when mass.Contains("mass"):
                    objectRigidbody.mass=float.Parse(trimmedData);
                    break;

                case string name when name.Contains("name"):
                    co.getGameObject().name = trimmedData;
                    break;
                
                case string orbits when orbits.Contains("orbits"):
                    co.setOrbitTargetName(trimmedData);
                    break;
                
                case string orbitPlane when orbitPlane.Contains("orbitPlane"):
                    Vector3 orbitPlaneVector = getInitialVectorData(trimmedData);
                    co.setOrbitPlaneVector(orbitPlaneVector);
                    break;

                case string position when position.Contains("position"):
                    Vector3 setPosiion = getInitialVectorData(trimmedData);
                    objectRigidbody.transform.position = setPosiion;
                    break;

                //What if a gameobject is not a sphere... Guess it would not have a radius anyway if it wasn't.
                case string radius when radius.Contains("radius"):
                    float r = float.Parse(trimmedData);
                    Vector3 setRadius = new Vector3(r,r,r);
                    co.getGameObject().transform.localScale = setRadius;                    
                    break;
                
                case string type when type.Contains("type"):
                    co.setType(trimmedData);
                    break;
        
                case string vel when vel.Contains("velocity"):
                    Vector3 velocity = getInitialVectorData(trimmedData);
                    objectRigidbody.velocity = velocity;
                    co.hasSetVelocity();
                    break;
                    
            }
        }
            co.clearNetForce(); //This is to make sure that the for during the next update is not additive to the force for the next update.t it can be used later.
    }

    void setPositionToPerihelion()
    {
        foreach(CelestialObject co in CelestialObjects)
        {
            if(co.getType().Equals("planet"))
            {
                if(co.getPerihelion() != 0 && co.getAphelion() != 0)
                {
                    co.calculateSemiMajorAxis();
                    co.getRigidbody().transform.position = new Vector3(co.getPerihelion(),0,0);
                }
            }
        }
    }
    void linkOrbitObjectNameToOrbitObjectandSetInitialVelocity() //Now that the data is recorded for each object the velocity can be implemented.
    {
        foreach (CelestialObject co in CelestialObjects)
        {
            if (co.getOrbitTargetName() != null)
            {
                for (int i = 0; i<CelestialObjects.Count; i++)
                {
                    if (CelestialObjects[i].getName().Equals(co.getOrbitTargetName()))
                    {
                        co.setOrbitTarget(CelestialObjects[i]); //which point (here it is taking the very crude approximation that it is another object) does the object orbit <== This sets it. 
                        break;
                    }
                }                
            }
        }
        calculateNetForce(); //Self explanatory.
        
        foreach (CelestialObject co in CelestialObjects)
        {
            if(!co.getSetVelocityStatus() && co.getOrbitTarget() != null) //it only sets a velocity if the velocity itself isn't predefined in the data txt. 
                                                                        //It also nullifies the ability for objects that don't have another object to orbit from having initial velcity.
            {
                Vector3 separationVector = co.getOrbitTarget().getRigidbody().transform.position - co.getRigidbody().transform.position;
                // float speed = Mathf.Pow(co.getNetForce().magnitude * separationVector.magnitude/co.getRigidbody().mass,(float)0.5);
                float speed = Mathf.Pow(G*(co.getRigidbody().mass + co.getOrbitTarget().getRigidbody().mass)*((2/separationVector.magnitude)-(1/co.getSemiMajorAxis())),(float)0.5);
                Vector3 velocityDirection =  Vector3.Cross(co.getOrbitPlaneVector(),separationVector).normalized;
                Vector3 velocity = velocityDirection * speed;
                co.getRigidbody().velocity = velocity;
            }
            co.getGameObject().SetActive(true); //Look at explanation in the classes as to why they were inactive.
            co.getRigidbody().isKinematic = false; //............................. as to why they were set to isKinematic.
            co.clearNetForce(); //This is to make sure that the for during the next update is not additive to the force for the next update.
        } 
    }

    void calculateNetForce() //Newtons law of gravitation
    {
        for (int i = 0; i < CelestialObjects.Count -1; i++) //This is the object concerning
        {
            for (int j = i+1; j < CelestialObjects.Count; j++) // This is to get all other objects acting on it.
            {
                Vector3 separationVector = CelestialObjects[j].getRigidbody().transform.position - CelestialObjects[i].getRigidbody().transform.position; //Must contain .transform.position, otherwise returns something unknown
                Vector3 Force = G*CelestialObjects[i].getRigidbody().mass*CelestialObjects[j].getRigidbody().mass * separationVector.normalized / (Mathf.Pow(separationVector.magnitude, 2));
                CelestialObjects[i].setNetForce(Force);
                CelestialObjects[j].setNetForce(Force * (-1)); // This is such that it doesn't have to calculate twice.
            }
        }
    }    


//**********************************************************Start has passed, now proceed to updating*******************************************************//

    async void FixedUpdate() //If a function happens during the start phase define it before here
    {   
            updateVelocityAccordingNewtonLawGravitation();

            
            
            foreach (CelestialObject co in CelestialObjects)
            {
                if(co.getName().Equals("Rocket2")){
                    //Debug.Log(co.getRigidbody().velocity.x);
                    //Vector3 rocketPositionVector = new Vector3(co.getRigidbody().position.x, co.getRigidbody().position.y, co.getRigidbody().position.z);
                    if(co.getRigidbody().velocity.x > 1.697f*currentSimulationSpeed)
                    {
                        if(mainSceneisPaused == false)
                        {
                            pauseMainScene();
                        }

                    }
                    
                    // if(co.getRigidbody().velocity.magnitude /currentSimulationSpeed > maxVelocity && co.getRigidbody().position.z > 500)
                    // {
                    //     maxVelocity = co.getRigidbody().velocity.magnitude / currentSimulationSpeed;
                    //     //Vector3 jupiterFlyByDistance = new Vector3(jupiterPositionVector.x - rocketPositionVector.x, jupiterPositionVector.y - rocketPositionVector.y, jupiterPositionVector.z - rocketPositionVector.z);
                    //     Debug.Log("The max velocity is: "+ maxVelocity + "The position is: " + co.getRigidbody().position.x + " "+ co.getRigidbody().position.y +" "+ co.getRigidbody().position);
                         
                    // }

                    
                }
                // if(co.getName().Equals("Jupiter"))
                // {
                //     //Vector3 jupiterPositionVector = new Vector3(co.getRigidbody().position.x, co.getRigidbody().position.y, co.getRigidbody().position.z);
                // }
                // transform.LookAt(target);

            } 
            
            
            // updateFixedUpdateCountPerSecond += 1;
        
    }
    void Update()
    {
        // if (currentSimulationSpeed != speed)
        // {
        //     changeSimulationSpeed(speed);
                
        // }
        // foreach(CelestialObject co in CelestialObjects)
        // {   
        //     //if(co.getName().Equals("Jupiter"))
        //     if(co.getName().Equals("Rocket2") && co.getRigidbody().position.z > 500f)
        //     {
        //         //changeSimulationSpeed(0.25f);
        //         //Debug.Log("YESS");
        //     }
        //     if(co.getName().Equals("Rocket2") && co.getRigidbody().position.z <500f)
        //     {
        //         //changeSimulationSpeed(20);
        //     }

        // }       

    }

    void updateVelocityAccordingNewtonLawGravitation()
    {
        calculateNetForce();
        foreach (CelestialObject co in CelestialObjects)
        {
            co.getRigidbody().AddForce(co.getNetForce());
            co.clearNetForce();// This is to make sure that the force during the next update is not additive from the force of the last update.
            if(co.getName().Equals("Rocket2")){
                Debug.Log(co.getRigidbody().velocity.magnitude + "Escape Velocity: " + Mathf.Sqrt(2*G*333000/co.getRigidbody().transform.position.magnitude));
                //Debug.Log(co.getRigidbody().position.x +" Position z " +co.getRigidbody().position.z);
            }
        }
    } 

//**********************************Actions that are not called at the start or periodically, but rather depend on the actions of the user************************************//
    // public void changeSimulationSpeed(float test) //A method that implements the slider.
    // {
    //     speed = test;
    //     foreach (CelestialObject co in CelestialObjects)
    //     {
    //         co.getRigidbody().velocity *= (speed/currentSimulationSpeed);
    //         //co.getRigidbody().velocity *= speed;
    //     }
    //     G *= Mathf.Pow((speed/currentSimulationSpeed),2);
    //     currentSimulationSpeed = speed;
    //     //currentSimulationSpeed = speed;                                           
    //     //G = newG;
    // }

    public void hasSpeedChanged(float dab)
    {
        //speed = dab;
        // simulationSpeed.text = speed.ToString() + " X" ;

        Time.timeScale = dab;
        Time.fixedDeltaTime = (float)0.02 * dab/Time.fixedDeltaTime;
        //newG = G/Mathf.Pow(speed, 2);
    }

    public void loadMainScene(string scenename)
    {
        Debug.Log("Loading Main Scene");
        SceneManager.LoadScene(scenename);
    }

    // public void loadTrajectoryScene(string scenename)
    // {

        

    //     // Debug.Log("Loading Trajectory Scene");
    //     // Debug.Log("Active scene is "+ SceneManager.GetActiveScene().name);
    //     // SceneManager.LoadScene(scenename, LoadSceneMode.Single);
    //     // while (SceneManager.GetActiveScene().buildIndex != SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/TrajectoryScene.unity"))
    //     // {
    //     //     Debug.Log("It has not loaded");
    //     // }
    //     // SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenename));
    //     // Debug.Log("Active scene is "+ SceneManager.GetActiveScene().name);
    // }
    

    public void pauseMainScene()
    {
        mainSceneisPaused = !mainSceneisPaused;
        foreach(CelestialObject co in CelestialObjects)
        {
            if(co.getName().Equals("Rocket2"))
            {   
                Debug.Log("Velocity Before: " + co.getRigidbody().velocity.magnitude);
                co.getRigidbody().velocity = new Vector3(2.23f*currentSimulationSpeed, co.getRigidbody().velocity.y, co.getRigidbody().velocity.z );
                Debug.Log("Velocity After: " + co.getRigidbody().velocity.magnitude);
            }
        }
        //StartCoroutine(LoadScene());
        //SceneManager.LoadScene("TrajectoryScene", LoadSceneMode.Single);
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        
        //loadTrajectoryScene("TrajectoryScene");
        //Debug.Log(SceneManager.GetActiveScene().name);
        /* foreach(CelestialObject co in CelestialObjects)
        {
            Instantiate(co.getGameObject());
            duplicatedCelestialObjects.Add(co);
        } */
        
        
        
        //Debug.Log(mainSceneisPaused);
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TrajectoryScene", LoadSceneMode.Single);
        //asyncOperation.allowSceneActivation = false;
        //Debug.Log("Progress: " + asyncOperation.progress);

        while(!asyncOperation.isDone)
        {
            /* Debug.Log("Loading: " + (asyncOperation.progress * 100 + "%"));
            if (asyncOperation.progress >=0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            } */
            yield return null;
        }    
        foreach(CelestialObject co in CelestialObjects)
        {
            SceneManager.MoveGameObjectToScene(co.getGameObject(), SceneManager.GetSceneByName("TrajectoryScene"));
        }
        SceneManager.UnloadSceneAsync("TrajectoryScene");

    } 

    //To Do: change the zoom of the camera. Possibly give the ability to change cameras.
}
