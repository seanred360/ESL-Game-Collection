using UnityEngine;
using System.Collections;

/// <summary>
/// This is the BallLauncher Class(it also launches the Bombs though too).  Numerous Instances of this Class
/// are controlled by the BallLauncherController.  The multiple instances of this class are attached to the empty
/// gameobjects just under the bottom of the screen(in the scene).  Each a potential launch point.
/// </summary>
public class BallLauncher : MonoBehaviour {

    public float force;                                     // the force at which the ball/other objects are launched
    public float forceMin, forceMax;                        // the min and max force the ball/bomb can be launched at
    [Range(-0.01f, -1f)]    
    public float minXValue;                                 // the minimum value the ball will be fired to the left(negative number)
    [Range(0.01f, 1f)]  
    public float maxXValue;                                 // the minimum value the ball will be fired to the right(positive number)
    public ObjectPoolScript[] ballPoolScripts;              // the array of all the ObjectPoolScripts for the ball
    public ObjectPoolScript bombPoolScript;                 // the ref to the ObjectPoolScript for the Bombs
    public AudioClip cannonThud;                            // the cannonThud is the audioClip that sounds when a ball is fired.
    private bool useAudioClip;                              // boolean that is set to true if we are going to use the launch sound
    private GameObject ballPoolsGameObject;                 // the gameObject that is the parent to all of the empties with each ObjectPoolScript(balls)
    private GameObject bombPoolGameObject;                  // the gameObject that is the parent to the bombs ObjectPoolScript(bombs)

    // Use this for initialization
    void Start ()
    {
        //call method that sets up the pool reference
        PoolReferenceSetup();

        //if cannonThud does not equal null then we...
        if(cannonThud != null)
        {
            //useAudioClip boolean is changed to true.  So if we have a sound assigned, then it will be used.  If not... it won't. Obviously.
            useAudioClip = true;
        }
    }

    /// <summary>
    /// This PoolReferenceSetup Method creates our references to the ball pools(all of them), and the Bomb Pool(only one of those).
    /// </summary>
    public void PoolReferenceSetup()
    {
        //find the GameObject Tagged "Ball Pool".
        ballPoolsGameObject = GameObject.FindGameObjectWithTag(Tags.BallPools);
        //then fill the ballPoolScripts array with the ObjectPoolScripts (all the children of the ballPoolsGameObject).
        ballPoolScripts = ballPoolsGameObject.GetComponentsInChildren<ObjectPoolScript>();

        //find the GameObject Tagged "OtherPools".
        bombPoolGameObject = GameObject.FindGameObjectWithTag(Tags.OtherPools);

        //then fill the bombPoolScripts array with the ObjectPoolScripts (the child of the bombPoolsGameObject).
        bombPoolScript = bombPoolGameObject.GetComponentInChildren<ObjectPoolScript>();
    }



    /// <summary>
    /// The Method that Loads and Fires Random Balls.  Here we set the force, direction, etc.. then we call
    /// a method that actually retrieves the ball from the pool for us, then once it is done, and ready, we change its velocity.
    /// </summary>
    public void LoadAndFireRandomBall()
    {
        //force equals a random number between the forceMin and forceMax
        force = Random.Range(forceMin, forceMax);
        //create a new float "randomXValue" get assigned Random.Range between minXValue and maxXValue
        float randomXValue = Random.Range(minXValue, maxXValue);
        //create a new Vector3 named expVec... it gets assigned a new Vector3 (  we pass randomXValue for X  ,  1 for Y  , and  0 for Z  )
        Vector3 expVec = new Vector3(randomXValue, 1, 0);

        //call method that gets ball from pool and make a variable to hold the reference.
        GameObject clone = RetrieveBallFromPool() as GameObject;
        //create a new Rigidbody named "rb" gets assigned the clones Rigidbody
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        //we use TransformDirection(  expVec multiplied by our force  ) and assign it to rb.velocity
        rb.velocity = transform.TransformDirection(expVec * force);
        //if the boolean useAudioClip is true...
        if (useAudioClip)
        {
            //we playClipAtPoint to instantiate a oneShotAudio (our cannonThud clip, at position 0,0,-80 ( -80 on the z-axis bc this is closest to the camera (audioListener) )
            AudioSource.PlayClipAtPoint(cannonThud, new Vector3(0, 0, -80f));
        }
    }


    /// <summary>
    /// The Method that Loads and Fires Bombs. Here we set the force, direction, etc.. then we call
    /// a method that actually retrieves the ball from the pool for us, then once it is done, and ready, we change its velocity.
    /// </summary>
    public void LoadAndFireBomb()
    {
        //force gets assigned Random.Range (between forceMin and forceMax)... 
        force = Random.Range(forceMin, forceMax);
        //create a new randomXValue and assign it a Random Value between minXValue and maxXValue...
        float randomXValue = Random.Range(minXValue, maxXValue);
        //create a new expVec = new Vector ( randomXValue for X , 1 for Y, 0 for Z )
        Vector3 expVec = new Vector3(randomXValue, 1, 0);

        //call method that gets a ball from the pool and make a variable to hold that reference.
        GameObject clone = RetrieveBombFromPool() as GameObject;//See RetrieveBombFromPool Comments below...

        //Create Rigidbody gets assigned the clones rigidbody (the clone is our launched bomb)
        Rigidbody rb = clone.GetComponent<Rigidbody>() as Rigidbody;
        //we use TransformDirection(  expVec multiplied by our force  ) and assign it to rb.velocity
        rb.velocity = transform.TransformDirection(expVec * force);
        //if the boolean useAudioClip is true...
        if (useAudioClip)
        {
            //we playClipAtPoint to instantiate a oneShotAudio (our cannonThud clip, at position 0,0,-80 ( -80 on the z-axis bc this is closest to the camera (audioListener) )
            AudioSource.PlayClipAtPoint(cannonThud, new Vector3(0, 0, -80f));
        }
    }


    /// <summary>
    /// This Method Grabs us a Ball from the pool puts it in position and activates it.  This method is
    /// called from the "LoadAndFireRandomBall()". Since this method just does the retrieval and activation of
    /// our pooled object it allows us to make the Fire Method that calls this a little shorter.  As calling this method only 
    /// takes a line.
    /// </summary>
    /// <returns></returns>
    public GameObject RetrieveBallFromPool()
    {
        //new int named r get assigned a Random value between zero and the length of the array of objectPoolScript
        int r = Random.Range(0, ballPoolScripts.Length);
        //objectPoolScript named tempPool gets accesses the elements in the array at position R
        ObjectPoolScript tempPool = ballPoolScripts[r];
        //new GameObject named obj and it calls GetPooledObejct on the tempPool. 
        GameObject obj = tempPool.GetPooledObject();
        //if obj is null..
        if (obj == null)
        {
            //we return null...
            return null;
        }
        //we set obj at our position...
        obj.transform.position = transform.position;
        //we set obj's rotation to 000
        obj.transform.rotation = Quaternion.identity;
        //set the obj to active
        obj.SetActive(true);
        //then return the obj
        return obj;
    }


    /// <summary>
    /// This Method Grabs us a Bomb from the pool puts it in position and activates it. This method is
    /// called from the "LoadAndFireBomb()". Since this method just does the retrieval and activation of
    /// our pooled object it allows us to make the Fire Method that calls this a little shorter.  As calling this method only 
    /// takes a line.
    /// </summary>
    /// <returns></returns>
    public GameObject RetrieveBombFromPool()
    {
        //create a ObjectPoolScript var and name it tempPool and then assign it the element at position "type" in the array.
        //ObjectPoolScript tempPool = bombAndPowerUpPoolScripts[type];
        //new gameObject "obj" gets a pooled object from tempPool reference using GetPooledObject.
        GameObject obj = bombPoolScript.GetPooledObject();
        //if the obj equals null...
        if (obj == null)
        {
            //return null
            return null;
        }
        //the obj's position gets our position
        obj.transform.position = transform.position;
        //the obj's rotation gets our rotation
        obj.transform.rotation = transform.rotation;
        //the obj gets set active
        obj.SetActive(true);
        //return obj...
        return obj;
    }
}
