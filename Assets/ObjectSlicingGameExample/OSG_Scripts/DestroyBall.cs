using UnityEngine;
using System.Collections.Generic;

namespace SlicingGame
{
    /// <summary>
    /// This Class handles the balls destruction, instantiation of ball gibs, and anything else related to the destruction of the balls.  The Main Method 
    /// "CutBall(int)", is called by the OSGTouchSlicer when a collision/ray-cast occurs against a ball.
    /// </summary>
    public class DestroyBall : MonoBehaviour
    {
        public bool inGameScene;                            // boolean - is this ball in a game scene? determines whether ball cut loads level or increases score..

        //public int sceneToLoadIfNot;                        // if not in game scene you are in a menu scene.  what scene(Integer) should be loaded for this ball's destruction
        public string sceneToLoadIfNot;

        public GameObject[] ballSplatterPrefabs;            // this is an array of splatter gameobjects.  Quads that are oriented upwards to be spawned the dir of swipe.

        public GameObject ballExplosionParticleSystem;      // the particle system that is the ball explosion I.e. liquid drops,vhunks,seeds... i reduced mine to just round drops.(The Guts of Ball)

        private Rigidbody thisRB;                           // a reference to this rigidbody.
        private BallDestroyCombo comboCounter;              // a reference to our ComboCounter class.  This is attached to a canvas in the scene(to control the fade/position of Hit Combo Text)
        private MeshRenderer[] ballChildMeshRenderers;      // these are the child Mesh Renderers of the whole ball objects (when not in a game Scene).

        private Vector3 resetPos;                           // a vector3 that has a value of 0,0,0
        private Animator ballTweenIconAnim;                 // a reference to the animator attached to a canvas in the scene(tweens the ball icon in top,left corner)

        private AudioSource gameSfxAudioSource;             // a reference to the gameSfxAudioSource.. also a child of screenFader prefab(if errors regarding either, start game from splashScene.
        public AudioSource thisObjectsAudioSource;          // reference to this objects audio source (for menu ball only)... temple bell sound
        public AudioClip[] splatSounds;                     // an array of splat sounds.  The splat sounds should be dragged to this field in the inspector (may need to lock inspector, depending on locale)
        public AudioClip templeBellSound;                   // temple bell sound to play quietly when starting a scene with a menu ball.
        public float minClipPitch = 1f;                     // this is the minimum pitch a splat sound will be played at(in addition to multiple sounds, we also use random pitch to increase variety)
        public float maxClipPitch = 1.2f;                   // this is the maximum pitch a splat sound will be played at.  Same as above.

        public GameObject gibsForVerticalCuts;              // this is where you drag the ball's "Vertical cut gibs" prefab.
        public GameObject gibsForHorizontalCuts;            // this is where you drag the ball's "Horizontal cut gibs" prefab.
        public GameObject gibsFor1DiagonalCuts;             // this is where you drag the ball's "1Diagonal cut gibs" prefab
        public GameObject gibsForDiagonalCuts;              // this is where you drag the ball's "Diagonal cut gibs" prefab

        public GameObject sliceForVerticalCuts;             // this is where you drag the ball's "Vertical Slice" particle prefab
        public GameObject sliceForHorizontalCuts;           // this is where you drag the ball's "Horizontal Slice" particle prefab
        public GameObject sliceFor1DiagonalCuts;            // this is where you drag the ball's "1Diagonal Slice" particle prefab
        public GameObject sliceForDiagonalCuts;             // this is where you drag the ball's "Diagonal Slice" particle prefab


        private List<GameObject> listOfGibsAvailable;       // This is the list in which we will store the above gameObjects.  This is the List for Gibs
        private List<GameObject> listOfSlicesAvailable;     // This is the list in which we will store the above gameObjects.  This is the List for Slices

        private List<CapsuleCollider> ballCollidersInScene; // this the reference to all of the capsule colliders in the scene.  we will use this to disable colliders on Non-Selected balls(menu scene)
        private GameObject[] menuBallGameObjects;           // the array of ball in the scene (if we are not in a game scene), we GetComponent all of these ball's CapsuleColliders for changing to inactive.
                                                            //          if a menu ball has already been sliced... that way you cannot slice other balls, after cutting the ball of another gamemode.


        // Use this for initialization
        void Start()
        {
            //call our setup method for the ball debris. (See Method Description)
            SetupBallDebirsArray();
            // we will use resetPos as the level origin 0,0,0
            resetPos = new Vector3(0, 0, 0);
            //if inGameScene (checked true in the inspector)... (only uncheck if the ball is for a Menu Scene)
            if (inGameScene)
            {
                ///Setup Scene References with GameObject.FindGameObjectWithTag...

                //setup our animator reference... 
                ballTweenIconAnim = GameObject.FindGameObjectWithTag(Tags.ballTweenIcon).GetComponent<Animator>();
                //setup our ComboCounter reference... 
                comboCounter = GameObject.FindGameObjectWithTag(Tags.comboCanvasTag).GetComponent<BallDestroyCombo>();

            }
            else
            {
                //if not in game scene then we are in menu scene.  Lets get this balls audio source so we can use it on slice...
                thisObjectsAudioSource = GetComponent<AudioSource>();

                //finish initializing our List of CapsuleColliders.
                ballCollidersInScene = new List<CapsuleCollider>();
                //we fill the menuBallColliders array with the menu ball gameobjects..
                menuBallGameObjects = GameObject.FindGameObjectsWithTag(Tags.ballTag);
                //now we loop through all of the menu balls...
                for (int i = 0; i < menuBallGameObjects.Length; i++)
                {
                    //with each iteration and each "Ball" found we do a GetCompoennt for the CapsuleCollider... and add that collider to the list.
                    ballCollidersInScene.Add(menuBallGameObjects[i].GetComponent<CapsuleCollider>());
                }
                //so now that, that is done later we can disable all the ball colliders.  After someone cuts a menu ball that level will be selected...
                //there is no need to allow them to cut another.  it will only produce a weird scene load issue, where half way through loading one level
                //another load starts... Fader would blink out a few times. etc...  Now that cannot happen :0

            }
            //Some of these References are for Menu Scenes only.. others are just references that we need to setup...

            //this gets all of the MeshRenderers for the attached ball.. some of the prefabs are setup minutely different.
            ballChildMeshRenderers = GetComponentsInChildren<MeshRenderer>();
            //reference to this rigidbody
            thisRB = GetComponent<Rigidbody>();
            //reference to Game Music Audio Source (Child of ScreenFaderSingletons "MODFaderPrefab(DontDestroyOnLoad)"  The only prefab in the Resources folder.
            ////////gameMusicAudioSource = GameObject.FindGameObjectWithTag(Tags.GameMusicAudio).GetComponent<AudioSource>();
            //reference to Game Sfx Audio Source (Child of ScreenFaderSingletons "MODFaderPrefab(DontDestroyOnLoad)"  The only prefab in the Resources folder.
            gameSfxAudioSource = GameObject.FindGameObjectWithTag(Tags.GameSfxAudio).GetComponent<AudioSource>();
            //setup the temple bell audio source for when not in a game scene.
            thisObjectsAudioSource = GetComponent<AudioSource>();

        }


        /// <summary>
        /// The RandomizeSfx Method chooses randomly between multiple audio clips and tweaks their pitch.
        /// </summary>
        /// <param name="clips"></param>
        public void PlayRandomBallSplatSound(params AudioClip[] clips)
        {
            //Create a random number between 0 and length of the clips array we pass in.
            int randomIndex = Random.Range(0, clips.Length);
            //Choose a randomPitch to provide extra variety to the clips array passed(uses our min and max pitch vars)
            float randomPitch = Random.Range(minClipPitch, maxClipPitch);
            //Set the pitch to our randomly created pitch
            gameSfxAudioSource.pitch = randomPitch;
            //Set the clip to the clip at our randomly chosen index.
            gameSfxAudioSource.clip = clips[randomIndex];
            //and Play()
            gameSfxAudioSource.Play();
        }



        /// <summary>
        /// This is the Method that Cuts our Ball... It is passed one parameter, which is the ballDebrisNum (an int between 0-3) vertical,Horizontal,1Diagonal,Diagonal... that number
        /// tells this method which pieces to instantiate.  a left and right half, or a top and bottom... it'll be based on the direction of the most recent swipe direction
        /// determined by our queue in OSGTouchSlicer that compares are current position from our position 3 frames prior.  This does not always yield a result that looks 
        /// proper but most of the time if there is even a small approach that is steady and true, it works for the 8 swipe directions.
        /// </summary>
        /// <param name="ballDebrisNum"></param>
        public void CutBall(int ballDebrisNum)
        {
            //get transform data to send to the Combo Canvas
            Vector3 ballPos = thisRB.position;

            //are we in the game scene, or still at a menu?
            if (inGameScene)
            {
                //if in game scene and we have a combo counter in the scene.
                if (comboCounter)
                {
                    //call ChekTimeAndRecordball(Vector3) on the ComberCounter reference.  we feed it this ballPos for a parameter.
                    comboCounter.CheckTimeAndRecordBall(ballPos);

                }
            }



            // the ballDebrisNum switch instantiates the correct AnimatedGibsObject. (the int we passed into this Cutball Method)  Based on that int we will either
            // instantiate 0 - Verticals Pieces, 1 - Horizontal Pieces, 2 - 1Diagonal Pieces, 3 - Diagonal Pieces 
            switch (ballDebrisNum)
            {
                case 0:
                    //case 0 - it is a vertical swipe instantiate the gameobject/prefab that has an animation of the balls left and right
                    //sides breaking away.... easy enough.

                    Instantiate(listOfGibsAvailable[ballDebrisNum], transform.position, Quaternion.identity);
                    //commented out Debug for release.
                    //Debug.Log("Vertical");

                    break;
                case 1:
                    //case 1 - we do the same thing, but for horizontal (left to right swiping).

                    Instantiate(listOfGibsAvailable[ballDebrisNum], transform.position, Quaternion.identity);
                    //commented out Debug for release.
                    //Debug.Log("Horizontal");

                    break;

                case 2:
                    //case 2 - we do the same but with the first Diagonal direction.

                    Instantiate(listOfGibsAvailable[ballDebrisNum], transform.position, Quaternion.identity);
                    //commented out Debug for release.
                    //Debug.Log("1Diagnol");

                    break;

                case 3:
                    //case 3 - we do the same but with the second Diagonal direction.

                    Instantiate(listOfGibsAvailable[ballDebrisNum], transform.position, Quaternion.identity);
                    //commented out Debug for release.
                    //Debug.Log("Diagnol_reg");

                    break;

                default:
                    //if there is a prob we will use case 1 like last time.  horizontal swipes are most likely IMO..

                    //Instantiate(ballDebrisTypes[ballDebrisNum].animatedballPiecePrefabs, transform.position, Quaternion.identity);

                    Instantiate(listOfGibsAvailable[ballDebrisNum], transform.position, Quaternion.identity);

                    break;
            }


            //Here we Instantiate a "sliceEffectPrefab" that is simple a stretched diamond shape(not unlike the blade effect), and we instantiate it at the "cut" position
            //but we add that position to our direction vector * 4f to give it a little bit of a lead.  Then we subtract a new Vecto3 with a random val on the Z-Axis(9.8 - 10f)
            //to bring it a little closer to the camera (down the z axis), so that it is on top of the ball destroy and ball destroy particle system.  I liked it
            //better on top of everything else that was going on(not at the same depth).  We used 9.8-10 to try and make it less likely that the Slice's were right on top of
            //other slices (to avoid any z-buffer fighting).  We probably should have brought it forward each time(like we do the splatters later on in this Class but it's not
            //that big of deal... I will most likely do that at a later time.
            Instantiate(listOfSlicesAvailable[ballDebrisNum], (transform.position + OSGTouchSlicer.currentSlicer.GetSlicerDirection().normalized * 4f) - new Vector3(0, 0, Random.Range(9.8f, 10f)), Quaternion.identity);


            //Then we call this method which creates the splatter effect in the direction of the swipe and the juice splash particle system from the "ball cut".  This
            //method takes a transform as a parameter... so that it has the position the cut happened at.  For that we use the "particleSystemParent" var again.
            PlayBallDestroyParticle(transform);

            //finally we play a random ball splat sound.  I added several to the project, and to a AudioClip array... then they are
            //also randomly have their pitch modified... to give the illusion of even more variety.  So we call that method passing our
            //array of splat sounds.
            PlayRandomBallSplatSound(splatSounds);

            //are we in a game scene (we need to check or uncheck this depending on the scene)??  I.e. the menu balls
            //have a Destroyball class on them... so for those menu ball  we uncheck "inGameScene".
            if (inGameScene)
            {
                //if we are in the game scene

                //switch to determine what to do based on the gameMode we are in.
                switch (GameController.GameControllerInstance.gameModes)
                {
                    case GameModes.RegularGameMode:
                        //if we are in regular gameMode, then if "Cutball" is called increment the RegularModeScore.
                        GameVariables.RegularModeScore++;

                        break;

                    case GameModes.ChillGameMode:
                        //if we are in relax gameMode, then if "Cutball" is called increment the ChillModeScore.
                        GameVariables.ChillModeScore++;

                        break;
                    default:
                        //noScore+-
                        break;
                }


                //here we activate a animator trigger that tweens the pineapple icon in the top left corner of
                //the screen every time this method is called(Cutting of the Ball).
                ballTweenIconAnim.SetTrigger(Tags.cutBallAnimTrigger);

                //we deactivate the ball gameObject so it will go back in to the pool.
                thisRB.gameObject.SetActive(false);

                //for good measure we store them all in one place... 0,0,0.  The Position and Velocity get zero'd
                thisRB.position = resetPos;
                thisRB.velocity = resetPos;


            }
            else
            {
                //else we are not in a game scene then we must be in a menu scene or elsewhere.

                //so since we are at a menu call FadeAndLoadSpecificLevel on our ScreenFaderSingleton and we pass it the
                //variable sceneToLoadIfNot which is under the "inGameScene" boolean in the inspector.  If setting up a Menu
                //Ball we should also enter the int("sceneToLoadIfNot") for the level/scene we want to load when that ball is cut.
                ScreenFaderSingleton.Instance.FadeAndLoadSpecificLevel(sceneToLoadIfNot);

                //we disable the collider so it can not be hit again... Also prevents other ball from being triggered.
                for (int i = 0; i < ballCollidersInScene.Count; i++)
                {
                    ballCollidersInScene[i].enabled = false;
                }

                //play the temple bell sound... ahhh refreshing...
                thisObjectsAudioSource.Play();

                //look through any potential child mesh renderers (depending on ball of prefab setup), and make sure they are disabled... we shouldn't
                //still see anything if a ball has been cut and destroyed.  These Menu Ball are not pooled so we just get them out of the way.
                for (int i = 0; i < ballChildMeshRenderers.Length; i++)
                {
                    //disable any child mesh renderers
                    ballChildMeshRenderers[i].enabled = false;

                }

            }

        }


        /// <summary>
        /// This is quite an ugly Method.  I just quickly did some copying and pasting, and now we are ready to go. This method adds our 
        /// Ball Gibs GameObject'ms, and our Slice Effect GameObjects to their respective lists. Normally I would use a loop, or find 
        /// some other way to do multiple similar things, but considering it is only 8 GameObjects being added two 2 lists, I just did
        /// this. I used the same ordering for both I.e. ( list[0] = Vertical, list[1] = Horizontal, list[2] = 1Diagonal, and list[3] = Diagonal)
        /// This way we can pass a integer to our "CutBall" method, and we can use that integer for Instantiating the proper gibs gameobject.
        /// </summary>
        private void SetupBallDebirsArray()
        {
            //finish initializing the list
            listOfGibsAvailable = new List<GameObject>();
            listOfSlicesAvailable = new List<GameObject>();

            //add the individual gibs gameobject to our Gibs List
            listOfGibsAvailable.Add(gibsForVerticalCuts);
            listOfGibsAvailable.Add(gibsForHorizontalCuts);
            listOfGibsAvailable.Add(gibsFor1DiagonalCuts);
            listOfGibsAvailable.Add(gibsForDiagonalCuts);

            //add the individual slices gameobject to our Slices List
            listOfSlicesAvailable.Add(sliceForVerticalCuts);
            listOfSlicesAvailable.Add(sliceForHorizontalCuts);
            listOfSlicesAvailable.Add(sliceFor1DiagonalCuts);
            listOfSlicesAvailable.Add(sliceForDiagonalCuts);

        }


        /// <summary>
        /// This Method is responsible for playing the particle systems of ball death!  They instantiate a ball juice splash particle system
        /// and a quad that is a directional splatter, using the swipe direction.
        /// </summary>
        /// <param name="parentGO"></param>
        public void PlayBallDestroyParticle(Transform parentGO)
        {
            //create a float named randomChance. this is the chance that a directional splatter quad will be instantiated(we don't want to do it every time because that would
            //probably yield too many transparent materials which will choke out lesser mobile devices like mine.
            float randomChance = Random.Range(0f, 5f);
            //we create a new quaternion that has a specified forward and upward direction.  I.e. for us forward is down the z axis away from the camera... so we just use
            //Vec3.forward.  Our upward direction will be the direction of our swipe (which we know we can get from GetSlicerDirection()... all of the splatter quads
            //are rotated to be facing the Vec3.up orientation, so that our swipes "upwards" will give them the proper orientation.
            Quaternion sliceRot = Quaternion.LookRotation(Vector3.forward, OSGTouchSlicer.currentSlicer.GetSlicerDirection());

            //I initially was going to make a static class that dealt with weighted randoms but i did not get around to it.  In this case we stored a random float in
            //randomChance and here we are saying... if when this was called randomChance ended up being > 0.75f (a fair chance), then we will continue and instantiate a
            //splatter quad.  This way the numbers will stay down a little bit...
            if (randomChance > 0.75f)
            {
                //Instantiate the splatter quad... first we call instantiate, then we get a random splatter quad out of the array of ballSplatterPrefabs.. we just use random.range
                //for that.  then we set it to parentGO.position(the transform we sent as a parameter to this method).  Then we add the swipe direction normalized * 4f.. which gives it
                //some lead in the direction we are swiping.  THEN we subtract a new Vec3 with only a Z-axis value (we do this to make sure it is far enough behind everything
                // but still in front of the DojoBGQuad)... we also don't want them all on top of each other so we are going to start splatter instantiation at 55f on the z axis
                //(5 units in front of DojoBG), and then every time we instantiate one we will use "GameVariables.splatterQuadSpawnDistance" and subtract 0.01f from it with every splatter...
                // 55 to 45 units away from the camera keeps them out of the other effects, and right on top of the DojoBG.. that is good for a few hundred splatters... which is probably
                //a little above average, so we are covered.
                Instantiate(ballSplatterPrefabs[Random.Range(0, ballSplatterPrefabs.Length)], (parentGO.position + OSGTouchSlicer.currentSlicer.GetSlicerDirection().normalized * 4f) + new Vector3(0, 0, GameVariables.splatterQuadSpawnDistance), sliceRot);
                //increment the GameVariables.splatterQuadSpawnDistance and start bringing them forward... the game controller resets it to 55f each round.
                GameVariables.splatterQuadSpawnDistance -= 0.01f;
                //commented out for release
                //Debug.Log(GameVariables.splatterQuadSpawnDistance.ToString());
            }
            //the last thing we do is instantiate our ballExplosionParticleSystem... again... at our parentGO.position(but this time with no lead(right where it died)), and
            //at the rotation of the parentGO.  I think everyone can agree that is plenty of instantiation for this class.  I wanted to pool more but the way i set everything
            //up made that tedious.  My device is a Samsung Ace Style dual core... it runs the game with the animated gibs at 40-55 fps... and its a pretty low quality phone
            //these days.
            Instantiate(ballExplosionParticleSystem, parentGO.position + new Vector3(0, 0, -3f), parentGO.rotation);
        }
    }
}
