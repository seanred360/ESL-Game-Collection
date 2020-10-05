using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SlicingGame
{
    /// <summary>
    /// The OSGTouchSlicer Class is responsible for recording the movement/direction of the Ball Slicer, and calling the Destroy Class's Methods that destroy Ball , Bombs , and PowerUps
    /// </summary>
    public class OSGTouchSlicer : MonoBehaviour
    {
        public static OSGTouchSlicer currentSlicer;                     // our static reference to this class
        public bool emulateTouchesWithMouse;                            // emulate touches with mouse is usually for editor testing but with simple 1 click games, it is a cheap way to ensure multi-platform support.
        public bool useColliderAndRaycast;                              // This boolean will also enable a long box collider that protrudes into scene.  Ticking box, helps with a little redundancy.(set true)
        public LayerMask sliceableObjects;                              // sliceObjects is our new LayerMask.  Only contains objects that need to be "Slice-able".
        public int maxQueueSize;                                        // maxQueueSize is the number of Transforms that will be stored.  2 seems like a fair number.  this gets reasonable direction results.
        public float minimumSliceDistanceForAudio;                      // the amount of distance the user has to swipe before a swipe sound is player... I.e. 5-10 units works well.
        public float minimumTimeBetweenSwipes;                          // this is the minimum amount of time between swipe sounds play.  we initialize swordSwipeCounter to this, and reset to after swipe(0.4 is good)
        public AudioClip[] swordSlashSounds;                            // an array of AudeioCLips to be used as swipe sound.  I added several similar, 
        private float swordSwipeCounter;                                // this is the timer for the swipe Sound... (so that it doesn't continuously play sfx during constant movement.
        private Vector2 fingerPos;                                      // fingerPos is where we store Input.mousePosition and touch.position.. then we read fingerPos
        private float angle;                                            // this is the angle that is calculated (we check to see if angle is up/down, left/right... etc, and we return (int gibsToUse)
        private int gibsToUse = 0;                                      // gibsToUse is where we store the int that we transmit to 
        private GameObject sliceColliderParent;                         // this is the immediate child of this GameObject(it is the parent of the BackUp Collider(used for fringe cases)-Redundancy
        private Queue<Vector3> positionQueue = new Queue<Vector3>();    // this is our positions queue... it stores the positions from the last (public int maxQueueSize (i.e. 3)) frames.
        private Vector2 newVec = new Vector2(0, 0);                     // this is the vector2 we store the ScreenToWorldPoint(fingerPos) & where the slicer's position
        private Camera mainCamera;                                      // reference to our MainCamera.
        private Rigidbody thisRb;                                       // our variable that will hold a cached reference to this Rigidbody.
        private Transform thisTransform;                                // the variable that will hold a cached reference to this Transform.
        private List<Collider> overlapList = new List<Collider>();

        // Use this for pre-initialization
        void Awake()
        {
            //assign this rigidbody to our thisRB variable.
            thisRb = GetComponent<Rigidbody>();
            //assign this transform to our thisTransform variable.
            thisTransform = transform;
            //setup a reference to our main camera
            mainCamera = Camera.main;
            //make sure the static ref is set to This.
            currentSlicer = this;
            //give the initial swordSwipeCounter the value we decided on for "minimumTimeBetweenSwipes"
            swordSwipeCounter = minimumTimeBetweenSwipes;
            //make sure that we have the proper gameObject for our sliceColliderParent(one gameobjects below this gameObject is its parent, so we use GetChild(0))
            sliceColliderParent = thisTransform.GetChild(0).gameObject;
            //then once we have the sliceColliderParent we set it inactive
            sliceColliderParent.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            //if we have emulateTouchesWithMouse checked/true then we run CheckForMouseMovement every frame, else we use touches and CheckForTouchMovement() every frame.
            if (emulateTouchesWithMouse)
            {
                //call mouse move method
                CheckForMouseMovement();
            }
            else
            {
                //else we call touch method
                CheckForTouchMovement();
            }

            //we call AddSlicerPositionToQueue() which is important.  This method adds or current position to the queue.  Once the queue is filled to maxQueueSize it will
            //start deleting them, but after initial startup it should always have several (later we will subtract current Position from Position 2 frames earlier... that is how we
            //will get our direction for ball cuts/particle splatters.
            AddSlicerPositionToQueue();

            //newVec gets assigned ScreenToWorldPoint(fingerPos)... our fingerPos var is where we are touch pos or mouse pos is on screen.  This translates that into world space, and
            //we store that new vec in "newVec" 
            newVec = mainCamera.ScreenToWorldPoint(fingerPos);

        }


        /// <summary>
        /// OnTriggerEnter This Unity Function checks if any "Ball" are in our TriggerCollider.  There is a long
        /// collider that protrudes into the scene, that is activated when the slicer is moving.  Most of the time
        /// the ray-casting is responsible for the ball "destroy" but occasionally the slice collider is responsible.
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            //if the object in our trigger collider is a "Ball" then...
            if (other.CompareTag(Tags.ballTag))
            {
                //if the other object is also active in the scene hierarchy(we make this check because we also have a ray-caster destroying ball... we don't want them to get a hold
                //of the same ball simultaneously... so to be safe we ask... Is it still active? because if not it might be because the ray-cast already started killing it..
                if (other.gameObject.activeInHierarchy)
                {
                    //commented for release.
                    //Debug.Log("Collider is doing its job");

                    //we assign our "gibsToUse" variable the returned value of the Method SpawnProperBallDebris()... (see that Method for better understanding).
                    gibsToUse = SpawnProperBallDebris();

                    //now we create a DestroyBall variable named "destroy".  We then grab the "other" gameObject that entered our trigger, and we cache its DestroyBall Component in our new
                    //"destroy" variable using GetComponent<>();
                    DestroyBall destroy = other.GetComponent<DestroyBall>();
                    //then we call "CutBall()" on our destroy variable, and pass it our "gibsToUse" variable.
                    destroy.CutBall(gibsToUse);
                }

            }
            //if the object in our trigger collider is not a "Ball", but it's a "bomb" then we have a similar approach but with a different class & method.
            if (other.CompareTag(Tags.bombTag))
            {
                //again we make sure that the object is in fact still active in the scene hierarchy... hopefully because we don't want to try to destroy it the same time as the ray-casting counterpart.
                if (other.gameObject.activeInHierarchy)
                {
                    //commented out for release.
                    //Debug.Log("Power-up/Bomb Being Destroyed Via Collider");

                    //now we create a DestroyBomb var named "destroy", and we use GetComponent<>() to get and store other.gameObjects "DestroyBomb" script...
                    DestroyBomb destroy = other.GetComponent<DestroyBomb>();
                    //then we call ActivateDestructionPerObjectType() on our new destroy variable.
                    destroy.ActivateDestructionPerObjecType();

                }
            }

        }

        /// <summary>
        /// This is the method that is called often, and anytime the user has the mouse button, or finger touching the screen.  It is
        /// responsible for moving and activating the slicing collider and ray-casting into the scene to look for ball to "cut".
        /// see ReleasSlicingDevice Method Summary for extra info.
        /// </summary>
        private void GrabSlicingDevice()
        {

            //we assign thisRb.position to our newVec.  This will move the "slicer" to the newVec vector2 using ScreenToWorldPoint(fingerPos) from Update().
            thisRb.position = newVec;
            //if we have decided to use the Collider && Ray-cast ((currently recommended, but can cause extra "close" hits(bombs included))), then we will...
            if (useColliderAndRaycast)
            {
                //now we set the slicerColliderParent to active... The Child GameObject of THIS GameObject...(just under the sliceParent is an empty GO with a box collider
                //protruding a ways down the z-axis.  Since we moved it to newVec first it shouldn't be active until after its in the new position.


                //Even though we were moving it first and THEN activating it...it was STILL being activated in the old position, or
                //at the very least IN TRANSIT... silliness.  So we have to make a silly one line method so we can call it in a fraction of a second later... sorry guys.


                //since the ray-cast does the bulk of the work anyway, its okay that the collider is not on instantly

                //call/invoke the collider activation with a fraction of a second delay... (To avoid the above mentioned issue)
                Invoke("ActivateColliderAfterDelay", 0.0125f);

            }
            CheckForRaycastHit();
            //So now that the collider is active, and we are moving everywhere the finger/mouse moves, we start ray-casting by calling CheckForRaycastHit();
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                //CheckForRaycastHit();
            }

        }


        /// <summary>
        /// This Method was made out of necessity only... In the GrabSlicingDevice Method we move the slicer to the new location (current Location), and if we have it set
        /// to "useColliderAndRaycast" then it activates the collider after the move.  WELL.... the move is not finished before it gets activated.  Which can leave you hitting
        /// Bombs that you did not mouse over... THAT IS THE WORST POSSIBLE SCENARIO!  So we through that silly one liner inside of a method that we could invoke with a slight
        /// delay. (0.15f seems fine).  Since if the extra collider was not enabled we would be relying on the ray-cast solely anyway... its not that big of a deal to wait an extra
        /// .15 seconds for the activation.  I wish this wasn't necessary but it is.  Not sure why.
        /// </summary>
        private void ActivateColliderAfterDelay()
        {
            sliceColliderParent.SetActive(true);
        }


        /// <summary>
        /// This Method is responsible for releasing the OSG_TouchSlicer GO, and Deactivating the "Slice" collider we have
        /// protruding into the scene.  This is called when the user stops touching or holding mouse1 depending on settings.
        /// The collider is not often responsible for destroying ball, usually it is the Ray-cast, but i wanted some 
        /// redundancy for some fringe cases.  If you are moving really fast the ray-cast is usually the killer, otherwise 
        /// slower loitering around the "edge of the ball" will cause the collider to 
        /// destroy the ball.
        /// </summary>
        private void ReleaseSlicingDevice()
        {
            //again... if we have "useColliderAndRaycast" checked/true...
            if (useColliderAndRaycast)
            {
                //cancel any invokes that are going on...
                CancelInvoke();
                //we set the sliceColliderParent to inactive.
                sliceColliderParent.SetActive(false);
            }

        }



        /// <summary>
        /// This method checks Input.Touches for finger touch/movement and moves the "TouchSLicer" accordingly
        /// This is how Grab/Release_SlicingDevice() is called.  Grab when we have a finger,movement, etc.., and
        /// release when we do not.
        /// </summary>
        private void CheckForTouchMovement()
        {
            //if our touch count is greater than zero...
            if (Input.touchCount > 0)
            {
                //loop through Input.touches
                foreach (Touch touch in Input.touches)
                {
                    //in any of the touches if touch.phase has began.. then do this...
                    if (touch.phase == TouchPhase.Began)
                    {
                        //store touch.position in our "fingerPos" variable
                        fingerPos = touch.position;
                        //we call GrabSlicingDevice();
                        GrabSlicingDevice();
                    }
                    //if any of the touches have moved, then we also need to update our position data...
                    if (touch.phase == TouchPhase.Moved)
                    {
                        //store touch.position in our "fingerPos" variable
                        fingerPos = touch.position;

                        //clearly we are still touching/moving so we still want to be grabbing the slicing device...
                        GrabSlicingDevice();

                        //since there is a finger on the screen (moving) we will decrement the swordSwipeCounter variable by time.deltatime. 
                        //So every frame we subtract time.deltaTime from our swordSwipeCounter.
                        swordSwipeCounter -= Time.deltaTime;

                        //here we check to see if the float returned from our GetDistanceOfSliceGesture is greater than our "minimumSliceDistanceForAudio" (5 - 10 units..
                        // i use 7f), and if that is the case then we also make sure that our swordSwipeCounter is less than or equal to 0f.
                        if (GetDistanceOfSliceGesture() > minimumSliceDistanceForAudio && swordSwipeCounter <= 0f)
                        {
                            //if the conditions are met then its time to play a slash sound!

                            //we use a oneShotAudio (PlayClipAtPoint) method to play a random swipe sound from our swordSlashSounds array, and then because PlayClipAtPoint
                            //needs a position to instantiate/play the sound... we chose 0,0, a distance that is close to the camera position. When the game was still using
                            //3d sounds, this ensured that the sound was in range/audible... the audio listener on the camera was pretty far down the z-axis, so we just edited
                            //the z position of the oneShotAudeio device.
                            AudioSource.PlayClipAtPoint(swordSlashSounds[Random.Range(0, swordSlashSounds.Length)], new Vector3(0, 0, -75));
                            //then we reset the swordSwipeCounter to the same value we initialized it with... our "minimumTimeBetweenSwipes" (i set to 0.4f)
                            swordSwipeCounter = minimumTimeBetweenSwipes;
                        }
                    }

                    //finally if any of our touches have ended, then we need to call the ReleasSlicingDevice()
                    if (touch.phase == TouchPhase.Ended)
                    {
                        //fingerPos = touch.position;

                        //us calling the ReleasSlicingDevice()
                        ReleaseSlicingDevice();
                    }
                }
            }

        }

        /// <summary>
        /// This method checks Input.GetMouseButtonDown for click/movement and moves the "TouchSLicer" accordingly
        /// This is how Grab/Release_SlicingDevice() is called.  Grab when we have a finger,movement, etc.., and
        /// release when we do not.
        /// NOTE: if the boolean EmulateTouchesWithMouse is set to try this is how things are moved.  This can be left on
        /// during development so that WebGL,Standalone, or Mobile can play the game without requiring logic to detect platform.
        /// A single click, and mouse position is accepted as a Touch and FingerPos on mobiles... I would normal just use Input.
        /// Touches, but when testing in the editor, I usually like to use the mouse.  It prevents constant builds being required
        /// to test little changes
        /// </summary>
        private void CheckForMouseMovement()
        {
            //if left mouse button has been clicked down
            if (Input.GetMouseButtonDown(0))
            {
                //store the mouse position in fingerPos variable(whether we use mouse or touch inputs we use the same variable)
                fingerPos = Input.mousePosition;

                //if the left button is clicked "down" this frame or like below when we check if the left button is "held" down.
                //we call the GrabSlicingDevice() method;
                GrabSlicingDevice();

            }
            if (Input.GetMouseButton(0))
            {
                //if left click is "held" down then we update our fingerPos variable with the mouse position.
                fingerPos = Input.mousePosition;

                //again we grab the SlicingDevice()
                GrabSlicingDevice();

                //so here the left mouse button must be held down, and so we will assume it is moving...  We will decrement the 
                //swordSwipeCounter variable by time.deltatime.  So every frame we subtract time.deltaTime from our swordSwipeCounter.
                swordSwipeCounter -= Time.deltaTime;

                //here we use our GetDistanceOfSliceGesture() method.  We determine if the current "distance" is greater than the
                //minimum required distance to play the "slash" sound, and if the timer that allows the sound to only play every half
                //a second or so.  That way it doesn't constantly play the slash sound)
                if (GetDistanceOfSliceGesture() > minimumSliceDistanceForAudio && swordSwipeCounter <= 0f)
                {
                    //play slash sound!
                    AudioSource.PlayClipAtPoint(swordSlashSounds[Random.Range(0, swordSlashSounds.Length)], new Vector3(0, 0, -75));
                    //reset the swordSwipeCounter to default value.(it will start being reduced again when someone is holding/moving
                    //their finger or mouse.
                    swordSwipeCounter = minimumTimeBetweenSwipes;
                }

            }
            //if the mouse button was release then it is time to stop dragging/moving the slicer...
            if (Input.GetMouseButtonUp(0))
            {
                //we call release slicing device... and we are done!
                ReleaseSlicingDevice();

                //***NOTE***
                //This CheckForMouseMovement() is almost identical to the CheckForTouchMovement().the
                //comments for this method are a little older, and therefore not a copy and paste of the Touch version, but they say
                //basically the same thing.
            }


        }

        /// <summary>
        /// This Method is responsible for adding our Touch Slicer's position to our positionQueue.  The way we deal with the "slice" or "cut" directions
        /// in this complete project template.  To get the "Direction" or "Slice Angle" we subtract our current position from the first position in the Queue.
        /// The variable "maxQueueSize" determines the size of the Queue.  I usually set maxQueueSize to 2 or 3.  2 works pretty well.  As soon as the game starts
        /// we fill the Queue in the first 2 frames, and each subsequent frame just pushes the older entries out.  If you do not have rapid position/direction changes
        /// and come at a ball from a fixed location a couple of ball lengths away, then the cuts are pretty accurate.  As accurate as the 8 direction cut can be. Up,
        /// down,left,right,up left, up right, down left, down right.  Quickly moving back and fourth, or changing cut direction to late into a slice causes it to cut 
        /// at the wrong direction.  This results in unavoidable funny looking cuts, but it does not happen every time.  Ideally a few extra particle systems, and a 
        /// splatter on the "Screen" side (not BG splatter) would hide it the majority of the time.  There was a much nicer system in the prototype(a little more
        /// info in the Remarks), but it was not performant on mobile/web.  
        /// If anyone has ideas besides having LOTS of pre-cut animated directions, or using mesh deformation (unless you have a performant way to do it in real time 
        /// with several ball), then I would love to see it!!  After spending way to long on this I settled because it was super simple/understandable... forgive me!! lol
        /// </summary>
        /// <remarks>
        /// This is not the ideal way to solve the issue(as you will see it doesn't always look good, but it will suffice).  The original prototype included some
        /// actual mesh deformations, filled faces, and displayed the "ball Core" UV coordinates.  Then another version that at the very least had accurate cut orientations, 
        /// but occasionally struggled with texture projection.  This project went through way to many iterations/re-factors, and asset creation, and since it's a free kit
        /// I had to draw a line... so the cheaper, more performant, and not so pretty option was chosen. :-(
        /// </remarks>
        private void AddSlicerPositionToQueue()
        {
            //if our positionQueue is already full then...
            if (positionQueue.Count >= maxQueueSize)
            {
                //we need to dequeue the oldest entry
                positionQueue.Dequeue();

            }
            //add our current position to the queue...
            positionQueue.Enqueue(thisTransform.position);

        }

        /// <summary>
        /// This is the method used in conjunction with AddSlicerPositionToQueue() that returns the Vector3 that is our movement direction... 
        /// We create a local Vector3 and then we assign it the value of our current position being subtracted from the oldest position in our
        /// queue.
        /// </summary>
        /// <returns>This method returns a vector3 that is the direction OSGTouchSlicer is moving.</returns>
        public Vector3 GetSlicerDirection()
        {
            //we create a new vec3 variable named slicerDirection and we store our position minus the oldest entry in our position queue.
            Vector3 slicerDirection = thisTransform.position - positionQueue.Peek();
            //then we return the slicerDirection that we created.
            return slicerDirection;

        }


        /// <summary>
        /// This method simply takes the current position of our TouchSlicer and the oldest stored position in our positionQueue.
        /// We call this method and ask for the distance to decide whether or not we should instantiate a OneShotAudio that plays
        /// a slash sound.  There are other uses for it but that is currently the only thing it is used for.
        /// </summary>
        /// <returns>Method returns the distance between our current position and the oldest position in our positionQueue in the form of a float.</returns>
        private float GetDistanceOfSliceGesture()
        {
            //we create a new float named "dist" and use Vector3.Distance to return the distance between our position and the oldest position in our positionQueue
            float dist = Vector3.Distance(thisTransform.position, positionQueue.Peek());
            //then we return that distance as a float.
            return dist;
        }



        /// <summary>
        /// This Method is responsible for checking the direction of our most recent slice(from our current position and oldest positionQueue position).
        /// It checks the angle of the swipe and based on the direction it spawns the correct ball debris prefabs.  (i.e. if you swipe up/down it
        /// will spawn left and right Ball Halves.  If you swipe left/right it will spawn top and bottom Ball Halves).  If you swipe up-left
        /// to down-right, or down-left to up-right it will give you the appropriate "diagonal" halves.
        /// </summary>
        /// <returns>This method returns an integer which we feed to our BallDestroy's "CutBall(int)" Method.  0 = vertical, 1 = horizontal,
        /// 2 = 1Diagonal, 3 = Diagonal </returns>
        private int SpawnProperBallDebris()
        {

            //OK. Our float variable "angle" get assigned the result of Mathf.Atan2(x,y) * 57.2957795f (this is 360 / pi * 2).  as opposed to
            //using Mathf.Rad2Deg we use the float amt it would return anyway... Mathf is expensive enough to use, maybe not so much in Rad2Deg,
            //but why not just multiply by a float...

            //we get the angle in radians and then multiple by 57.295.... to turn it back into degrees.  

            //Then we have 8 conditionals that check the degrees we now have, and we determine where these degrees fall.  

            //I have included a unit circle from the development of this kit("UnitCircle"), Look at the approximate "highlighted" areas on the 
            //UnitCircle and compare to the "angle" in conditionals below. They should help some people visualize what is going on if I didn't explain well.
            //We opted for 8way directional mode as opposed to just 4way(up,down,right,left)... but as stated above... sometimes the "cuts" don't look right
            //because of how we did the swipe position queue... unfortunately i couldn't get the necessary delta from 1 frame :-(
            angle = Mathf.Atan2((thisTransform.position.x - positionQueue.Peek().x), (thisTransform.position.y - positionQueue.Peek().y)) * 57.2957795f;


            /////////////////////////////////////////////////////////////
            ///____First Directions Down,Up,Right,Left_____/////////////
            ////////////////////////////////////////////////////////////

            ////////////////

            ////// UP & DOWN //////

            if (angle > 157.5f || angle < -157.5f)
            {
                //swipe down
                return 0;
            }

            if (angle > -22.5f && angle < 22.5f)
            {
                //swipe up
                return 0;
            }


            ////// LEFT & RIGHT //////

            if (angle > 67.5f && angle < 112.5f)
            {
                //swipe right
                return 1;
            }

            if (angle > -112.5f && angle < -67.5f)
            {
                //swipe left
                return 1;
            }

            ////////////////


            //////////////////////////////////////////////////////////////
            //__Secondary Directions UpRight,UpLeft,DownRight,DownLeft_///
            //////////////////////////////////////////////////////////////

            ////////////////


            ////// 1DIAGONAL //////

            if (angle > 22.5 && angle < 67.5)
            {
                //up and right
                return 2;

            }
            if (angle > -157.5 && angle < -112.5)
            {
                //down and left
                return 2;

            }

            ////// DIAGONAL //////

            if (angle > -67.5 && angle < -22.5)
            {
                //up and left
                return 3;

            }
            if (angle > 112.5 && angle < 157.5)
            {
                //down and right
                return 3;

            }

            ///////////////////

            ///////////////////////////////////////////////////////

            //if anything goes wrong.. return 1.  Horizontal
            // because that is probably the most likely cut direction
            return 1;


        }


        /// <summary>
        /// This method adds 1 to the player score based on the gameMode they are playing.  The scores Are kept individually,
        /// because some of the gameModes are more difficult than others... so there is a Arcade,RegularGameMode, and ChillModeScore.
        /// </summary>
        private void AddToBallDestroyedScore()
        {

            //if the GameControllers "gameModes" var is set to GameModes.RegularGameMode then...
            if (GameController.GameControllerInstance.gameModes == GameModes.RegularGameMode)
            {
                //then RegularModeScore gets incremented by 1 :-)
                GameVariables.RegularModeScore++;
            }

            //if the GameControllers "gameModes" var is set to GameModes.ChillGameMode then...
            if (GameController.GameControllerInstance.gameModes == GameModes.ChillGameMode)
            {
                //then ChillModeScore gets incremented by 1 :-)
                GameVariables.ChillModeScore++;
            }

        }


        /// <summary>
        /// This Method is responsible for "destroying" our ball if we swipe across them.  The ray-cast comes from the
        /// camera and ends at the location of the ball(after ScreenPointToRay().
        /// </summary>
        private void CheckForRaycastHit()
        {
            //create a RaycastHit var and name it hit;
            RaycastHit hit;
            //create a Ray named ray, and give it the value of ScreenPointToRay(our fingerPos)
            Ray ray = mainCamera.ScreenPointToRay(fingerPos);

            //Ray-cast into our scene but only to our custom layer mask (myBallAndPowerUpLayerMask)...
            //which only contains the "Ball", and "Bomb" Layers.  We are only ray-casting 100 units
            //into scene
            if (Physics.Raycast(ray, out hit, 100f, sliceableObjects))
            {

                //we store our hit object in a new GameObject we create named hitObj.
                GameObject hitObj = hit.transform.gameObject;
                //Debug.Log("Ray-cast is doing its job");

                //if the hitObj has a tag of "Ball"...
                if (hitObj.CompareTag(Tags.ballTag))
                {
                    Vector3 hitBallPos = hitObj.transform.position;
                    //make sure the obj is still active in the scene(make sure the onTriggerEnter event did not
                    //already start deactivating this gameobject)
                    if (hitObj.activeInHierarchy)
                    {

                        //gibs to use is assigned the value that SpawnProperBallDebris() returns.  We will need
                        //this for later when we destroy the ball.
                        gibsToUse = SpawnProperBallDebris();

                        //we Create a variable labeled "destroy" and we GetComponent() on the hitObj.
                        DestroyBall destroy = hitObj.GetComponent<DestroyBall>();
                        //then we call the CutBall() method on our hitObj, and pass the gibsToUse var as a parameter.
                        destroy.CutBall(/*debrisRot,*/ gibsToUse);

                        //this portion is for overlapping balls.  Before you kind of had to have the (useColliderAndRaycast Checked), but now
                        //it can just be used for the fringe cases/as a backup.  Because we will do a small little OverlapSphere check at the hitObj's
                        //position...

                        //create an array and store the overlapping objects (within 4 units(remember everything is big in this example)
                        Collider[] overlappingBallColliders = Physics.OverlapSphere(hitBallPos, 4f, sliceableObjects);
                        //then we will add the array elements to our overlapList Collection
                        overlapList.AddRange(overlappingBallColliders);
                        //then we will call a method that will check the elements in the list.  (check tags, and then take appropriate action if needed)
                        CheckSwipeOverlap(gibsToUse);

                    }
                }
                else
                {
                    //else since the tag is not "Ball" and there was only 2 objects on our LayerMask(which is Balls & Bombs)
                    //then it has to be a bomb... so we assume it is and move forward.
                    if (hitObj.activeInHierarchy)
                    {
                        //we again create a new variable but this time its for our Bomb class "DestroyBomb". We
                        // GetComponent on our objHit and then after words we call the "ActivateDestructionPerObjecType()".
                        DestroyBomb destroy = hitObj.GetComponent<DestroyBomb>();
                        //call the "ActivateDestructionPerObjecType()"
                        //it will destroy itself and activate effects accordingly.
                        destroy.ActivateDestructionPerObjecType();
                    }
                }
            }
        }


        /// <summary>
        /// This method is called by the RayCasting method.  If two(or three, etc), balls happen to be overlapping, then a ball can occlude other balls.  Because of this,
        /// Having the "useColliderAndRaycast" boolen checked, was kind of necessary.  (Because only the Collider, would penetrate a collection of balls).  Now we do a small sphere
        /// cast in the area the first ball was cut.  And if there were other balls really close, then we collect them, and this method gets called.  Here we loop through them, and destroy
        /// them as usual.  The parameter that is passed in is our gibs number.  Because the other possible occluded targets are soo close to our cut ball, it makes since the cut angle would
        /// be similiar, so we will just make it the same.  So we pass the current gibs int, when we call this method.
        /// </summary>
        /// <param name="gibsIntFromOtherBall"></param>
        public void CheckSwipeOverlap(int gibsIntFromOtherBall)
        {
            //loop through all elements in the overlapList.
            for (int i = 0; i < overlapList.Count; i++)
            {
                //if any are tagged "Ball" we then.....
                if (overlapList[i].gameObject.CompareTag(Tags.ballTag))
                {
                    // ... create a gameobject Var and store the current element.
                    GameObject gObj = overlapList[i].gameObject;
                    // we use GetCompoent to get a reference to the gameobjects DestroyBall script
                    DestroyBall destroy = gObj.GetComponent<DestroyBall>();
                    //then we call "CutBall" on it, and pass in the gibs number that was passed, from the original cut ball (remember this ball is probably touching the first cut ball), so we will make the cut the same.
                    destroy.CutBall(gibsIntFromOtherBall);
                    //debug, from testing.  commented out for publish.
                    //Debug.Log("Neighbor Ball number:" + i.ToString());
                }
            }
            //then we clear the list.  We can keep using this same list.
            overlapList.Clear();
            //Debug.Log("Clearing List, for use during next cuts...");

        }

    }
}
