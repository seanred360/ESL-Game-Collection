using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Class that can be used to Destroy or Clean-Up gameObjects.  There are lots of ways it can be used.  Here are a few examples.  1.) You can use 
/// it to silently destroy some Empty GameObjects that you used as folders in the scene.  2.) You could also make this part of a prefab you instantiate on 
/// NPC/Character death... it could explode(add force) to some gibs (body parts of the NPC),  then plays an explosion sound, and a squishy death sound.  Its 
/// a useful destroy object script.
/// </summary>
public class DestroyGameObject : MonoBehaviour
{
    public AudioClip destroySound, destroySoundTwo;	    //sound(s) to play when object is destroyed
    public float delay;                                 //delay before object is destroyed
    public bool destroyChildren;                        //should the children be detached (and kept alive) before object is destroyed?
    public float pushChildAmount;                       //push children away from center of parent

    void Start()
    {
        //get a list of children game objects
        Transform[] children = new Transform[transform.childCount];
        // we loop through all of the children in this gameObject
        for (int i = 0; i < transform.childCount; i++)
        {
            //as we loop through transform.childCount we make each transform in the children[] list of transforms a child objects transform.
            children[i] = transform.GetChild(i);
        }

        //if "destroyChildren" is false we call transform.DetachChildren on this object.  
        if (!destroyChildren)
        {
            //detaching the children
            transform.DetachChildren();
        }

        //lets loop through the children list this time(since now all the transforms are contained), and add force/torque to the children!
        for (int i = 0; i < children.Length; i++)
        {
            //create a Transform name child and assign it the first transform in children
            Transform child = children[i];
            //if "child" has a Rigidbody and is pushChildAmount != 0 then...
            if (child.GetComponent<Rigidbody>() && pushChildAmount != 0)
            {
                //create a Rigidbody name childRB and get a reference to its Rigidbody we confirmed it had just a moment ago.
                Rigidbody childRB = GetComponent<Rigidbody>();
                //create a new Vector3 named pushDir and give it a direction vector child.position - this objects position...
                //where ever the child was detached it is most likely not at the same exact world space coordinates as the parent, so
                //in subtracting the child's position from this.transform.position we will get the direction it should be blasted.
                Vector3 pushDir = child.position - transform.position;
                //addForce to our childRB and addTorque to our childRB...
                childRB.AddForce(pushDir * pushChildAmount, ForceMode.Force);
                childRB.AddTorque(Random.insideUnitSphere, ForceMode.Force);
            }
        }
        //if destroy sound was added, then play it
        if (destroySound)
        {
            //AudioSource.PlayClipAtPoint instantiates an audio source at the position that we feed it, and then destroys the audio
            //source once the sound has played.  We use the same AudioSource.function below for soundTwo.
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        //if second destroy sound was added, then play it
        if (destroySoundTwo)
        {
            AudioSource.PlayClipAtPoint(destroySoundTwo, transform.position);
        }
        //destroy  parent
        Destroy(gameObject, delay);
    }
}