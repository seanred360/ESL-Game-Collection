using UnityEngine;
using System.Collections;

/// <summary>
/// Simple Script to Rotate GameObjects Indefinitely.  I.e. a ferris wheel, or a Gem/Coin Object.
/// </summary>
public class RotateObject : MonoBehaviour
{

    public Vector3 rotation;                //vector3 represents how we would like this to rotate. I.e. 0,0,40 would spin a object counter clockwise on z-axis.
    public Space space;                     //space the rotation occurs in.  Local or World?
    private Rigidbody thisRB;               //thisRB is where we cache THIS Rigidbody.
    private Transform thisTransform;        //thisTransform is where we cache THIS Transform.

    //Get Rigidbody Component OnEnable that way we can rotate the gameobject in the most efficient way, but
    //if it does not have one, then we will just rotate its transform.
    void OnEnable()
    {
        //if our gameobject has a rigidbody component then we will move/rotate it via that...
        if (GetComponentInChildren<Rigidbody>() != null)
        {
            //lets setup a reference to our rigidbody component, if it exists.
            thisRB = GetComponent<Rigidbody>();
        }
        //lets cache our transform component.
        thisTransform = this.transform;
    }

    //This is the rotation... uses RigidBody.MoveRotation or transform.Rotate(if no RigidBody).
    void Update()
    {
        //if thisRB exists then...
        if (thisRB)
        {
            //and its not kinematic...
            if (!thisRB.isKinematic)
            {
                //create new vector3 named "newRot" which is assigned rotation * Time.deltaTime
                Vector3 newRot = rotation * Time.deltaTime;
                //create new Quaternion named "rot" and assign it Quaternion.Euler(and pass in newRot). 
                //This will give us a Quaternion from our rotation vec3 and we can use that directly with our Transforms Rotation.
                Quaternion rot = Quaternion.Euler(newRot);
                //we use Rigidbody.MoveRotation and we pass in the current (thisRB.rotation(Quaternion) * rot(our new Quaternion))
                thisRB.MoveRotation(thisRB.rotation * rot);

            }
            //else if it is kinematic..
            else
            {
                //we move via transform. Our cached transform.Rotate (rotation * Time.deltaTime, in whatever space you selected(Local or World).
                thisTransform.Rotate(rotation * Time.deltaTime, space);
            }
        }
        //else if there is not a Rigidbody then...
        else
        {
            //we rotate via the transform again. (same as Kinematic RB)
            thisTransform.Rotate(rotation * Time.deltaTime, space);
        }
    }
}