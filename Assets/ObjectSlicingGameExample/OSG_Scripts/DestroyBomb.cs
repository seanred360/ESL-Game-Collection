using UnityEngine;

namespace SlicingGame
{
    /// <summary>
    /// The BombDestroy Class handles all of the destruction, destruction effects, of the bomb.
    /// </summary>
    public class DestroyBomb : MonoBehaviour
    {
        private GameObject thisGameObject;                                  // reference to the current gameObject.

        [Header("GameOverBombAnimationGO")]

        public GameObject gameOverBombGibs;                                 // our prefab that has the game over bomb explode animation; throwing gibs.

        [Header("ParticleSystemsToInstantiate")]

        public GameObject gameOverBombParticleEffect;                       // a reference to the particle effect for the bombs destruction.

        [Header("These References Set themselves up")]

        public ChromaticAberration chroma;                                  // the reference to our chromatic aberration script on the MainCam
        public SimpleCameraShake shake;                                     // the reference to our camera shake script on the MainCam

        void Awake()
        {
            //cache a reference to this gameObject.
            thisGameObject = this.gameObject;

            //Setup Shake and Chroma Script References... both scripts are on the main camera
            shake = Camera.main.GetComponent<SimpleCameraShake>();
            chroma = Camera.main.GetComponent<ChromaticAberration>();
        }


        /// <summary>
        /// This Method is called when one of our Bombs is destroyed.  It's called from our
        /// OSGTouchSlicer when the collider or ray-cast comes into contact with the object.
        /// The method is called, and then the bomb is destroyed.
        /// </summary>
        public void ActivateDestructionPerObjecType()
        {
            GameOverBombDestroy();
        }


        /// <summary>
        /// This is the Method to call when destroying a Bomb in regular mode.  Then it calls a method
        /// that ends the game in the game controller.  We instantiate an animated bomb explosion(gibs), and a explosion
        /// effect particle system.  Next we call subtle camera shake, and chromatic aberration methods.  Lastly
        /// it calls a Method that creates a blast wave that pushes nearby balls away(to simulate explosion force).  
        /// Then we SetActive(false) the gameobject and we are done.
        /// </summary>
        void GameOverBombDestroy()
        {
            //Call this method on the GameController Static Reference so that the game ends
            //(the game over bomb is only in regular mode and if its hit, it ends the game)
            GameController.GameControllerInstance.TakeBallAndEndGame();

            //Instantiate the gameOverBombGibs (Which is a prefab that is set to play a unity animation and that
            // makes it look like pieces of the bomb are flying outward).
            Instantiate(gameOverBombGibs, transform.position, Quaternion.identity);
            //Instantiate our Bomb Explosion Particle System.  
            Instantiate(gameOverBombParticleEffect, transform.position - new Vector3(0, 0, 20), Quaternion.identity);

            //call our Start Camera Shake Method;
            shake.StartShake();
            //call our Start Chromatic Aberration Method;
            chroma.StartAberration();

            //Call the Method at the bottom of this class, which adds an outward force from the 
            //explosion which only effects "Ball" Tagged Objects
            ExplosionBlastForce();
            //Set this object inactive so we can use it again.
            thisGameObject.SetActive(false);
        }


        /// <summary>
        /// This method is responsible for adding force to nearby balls.  It is called when a bomb explodes.
        /// </summary>
        public void ExplosionBlastForce()
        {
            //create 2 new floats.  
            //We will use this for the radius of the explosion.
            float radius = 48f;
            //we will use this for the force of the explosion
            float explosionForce = 3f;
            //create a new vector3 and zero it out.
            Vector3 explosionOrigin = new Vector3(0, 0, 0);
            //assign the transform.position of the bomb(when this method is called) to explosionOrigin.
            explosionOrigin = transform.position;
            //lets create an array of colliders and then check Physics.OverlapSphere and pass that function
            //our explosionOrigin, and our desired radius from earlier.  
            Collider[] colliders = Physics.OverlapSphere(explosionOrigin, radius);

            //now that we have our array of colliders within our radius and at our position we will start
            //by looping through them.
            for (int i = 0; i < colliders.Length; i++)
            {
                //we check to make sure a few conditions are met when looping through our array of "hit"
                //colliders.  We make sure that the colliders we hit are tagged "Ball", and we make sure
                //that they do have a rigidbody(because we are going to attempt to addForce() to the hit
                //colliders.
                if (colliders[i].CompareTag(Tags.ballTag) && colliders[i].GetComponent<Rigidbody>() != null)
                {

                    //now that we know we are only talking about our "Ball" tagged Rigidbodies we...

                    //create a new vector3 and we subtract our transform.position from the hit colliders
                    //position(this vector3 basically now stores the direction we are going to "Push"
                    //the ball in with addForce()
                    Vector3 pushDir = colliders[i].transform.position - transform.position;

                    //we setup a quick final reference to the hit colliders Rigidbody so that we can apply
                    //force and torque to the balls
                    Rigidbody hitRB = colliders[i].GetComponent<Rigidbody>();

                    //commented out debug info for release time
                    //Debug.Log(colliders[i].name.ToString());

                    //here is where we addForce to our hit ball collider.  The vector3 that we pass addForce()
                    //is our pushDir from earlier multiplied by our explosionForce float.  We use ForceMode.Impulse 
                    // so that its kind of a "Punch" of force pushing the objects away from the explosion position.
                    //Then we add a little bit of spin to the ball using addTorque and feeding Random.insideUnitSphere,
                    //while again using ForceMode.Impulse;

                    hitRB.AddForce(pushDir * explosionForce, ForceMode.Impulse);
                    hitRB.AddTorque(Random.insideUnitSphere, ForceMode.Impulse);

                }

                //we do these steps for all of the balls with colliders and rigidbodies that we find within our radius.
            }
        }
    }
}
