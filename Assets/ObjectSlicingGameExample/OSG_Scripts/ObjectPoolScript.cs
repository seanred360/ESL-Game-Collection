using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is a Basic Object Pool Script.  In the ColorSwitchClone project this class is
/// used on the ColorChangePickups and the StarPickups.  It is added to an Empty GameObject
/// in the game scene.  This is similar to the pooling script that Unity Technologies makes in their
/// Object Pooling Live Training.  A few quick modifications can be made to it, to make it
/// much more powerful.  To learn more about this Script visit the Unity3d.com/learn page
/// and navigate to the "Live Trainings".. you'll find an object pooling video there.
/// </summary>
public class ObjectPoolScript : MonoBehaviour

{
    public static ObjectPoolScript current;     //reference to this pool script(used when getting an object from this pool)
    public GameObject pooledObject;             //the object we are pooling
    public int pooledAmount = 20;               //the number of those objects that will be instantiated at runtime
    public bool willGrow = true;                //if you need more than the pooledAmout, then it will instantiate new ones as requested.

    public List<GameObject> pooledObjects;      //the list that will hold all of our goodies!!!


    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        //sets the static reference to "this" instance of the script.
        current = this;
    }

    // Start is called just before any of the Update methods is called the first time
    public void Start()
    {
        //initialize the list, b/c it was just declared above
        pooledObjects = new List<GameObject>();
        //loop the length of the "pooledAmount"
        for (int i = 0; i < pooledAmount; i++)
        {
            //on each iteration create a GameObject named "obj" and then Instantiate the GameObject("pooledObject"). (The GO cast in front of Instantiate isn't needed)
            GameObject obj = (GameObject)Instantiate(pooledObject);
            //we set the obj (pooledObject we just instantiated to inactive
            obj.SetActive(false);
            //then we add it to our list of "pooledObjects" using Add(obj)
            pooledObjects.Add(obj);
        }
    }


    /// <summary>
    /// This Method returns us a GameObject from the pool.  It will create more GameObjects of the pooledObject type if you
    /// set "willGrow" to true in the inspector.  IMO I wouldn't I usually test enough to know how many I need... because when
    /// you need more you are instantiating them on the fly... which is the reason we are pooling objects.  BUT an argument 
    /// can always be made for all of the "what ifs", and that as a safety it is nice to have enabled.  It depends on the use
    /// case.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledObject()
    {
        //we loop through the list for as many objects as are inside it... (pooledObjects.Count)
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            //while iterating... if the object we come to equals null we...
            if (pooledObjects[i] == null)
            {
                //create another like initially....
                GameObject obj = (GameObject)Instantiate(pooledObject);
                //set it to inactive
                obj.SetActive(false);
                //say that the object we were looking for is the object we just instantiated
                pooledObjects[i] = obj;
                //then return the pooled element we just had to back fill with instantiate "like it was the real pooled McCoy"
                return pooledObjects[i];
                //BUT that is only if for some reason it is iterating through and there is a missing object... instantiating one
                //for it is better than the alternative... NullReferenceExcepions... This was a good way to handle the possibility.
            }
            //If everything actually worked as EXPECTED.. Great.. we have and inactive object ready to go
            if (!pooledObjects[i].activeInHierarchy)
            {
                //return that inactive obj to the caller (they will have to move it, and activate it)
                return pooledObjects[i];
            }
        }
        //and finally if the pool "willGrow?" then...
        if (willGrow)
        {
            //create a new GameObject name "obj" and Instantiate a (GameObject)pooledObject.. cast isn't necessary, but what the hay.
            GameObject obj = (GameObject)Instantiate(pooledObject);
            //add that new shiny obj of ours to the list, using .Add(obj)
            pooledObjects.Add(obj);
            //and return the obj
            return obj;
        }
        //nothing useful to return here; return null
        return null;
    }
}