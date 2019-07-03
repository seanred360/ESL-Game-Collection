using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// This script handles a hidden object. When the objects are created, they all have this script. The objects which are specifically hidden also have a collider which
	/// Lets us click them. When clicked, the hidden object will be added to the list of found objects.
	/// </summary>
	public class HOGHiddenObject:MonoBehaviour 
	{
		//The name of the object in singular form
		public string objectName = "Object";
		
		//The article for the name. For example: "An Apple", or "A Shoe"
		public string nameArticle = "An";
		
		//The name of the object in plural form, example: "Apples"
		public string namePlural = "Objects";
		
		//The tags that describe this object (red,small,animal,round,etc)
		public string[] keywords;
		
		//The rotation of the icon of this object
		public float iconRotation = 0;
		
		//Is this object hidden?
		public bool isHidden = false;
		
		//Delay of the animation when the object first appears
		internal float animationDelay = 0;
		
		//Various animations for when the object appears, is found, or hints at its own location
		public AnimationClip animationIntro;
		public AnimationClip animationHint;
		public AnimationClip animationFind;
		public AnimationClip animationIcon;

        // Was this object already clicked?
        internal bool isClicked = false;

		// Use this for initialization
		void Start() 
		{
			//If there is an animation delay for the object, wait for the delay and then play the animation
			if ( animationDelay > 0 )
			{
				//DelayAnimation();
			}
		}

		IEnumerator DelayAnimation( float delayValue )
		{
			//Remember the original scale of the object 
			Vector3 objectScale = transform.localScale;
			
			//Set the scale to 0, making the object invisible
			transform.localScale *= 0;
			
			//Wait the delay
			yield return new WaitForSeconds(delayValue);
			
			//Reset the object's scale to its original
			transform.localScale = objectScale;
			
			//Play the intro animation of the object, if it exists
			if ( GetComponent<Animation>() )   GetComponent<Animation>().Play(animationIntro.name);
		}
		
		//This function plays the object intro animation
		void ObjectIntro()
		{
			if ( GetComponent<Animation>() )   GetComponent<Animation>().Play(animationIntro.name); 
		}
		
		//This function plays the object hint animation
		void ObjectHint()
		{
			if ( GetComponent<Animation>() )   GetComponent<Animation>().Play(animationHint.name); 
		}
		
		//This function plays the object find animation
		void ObjectFind()
		{
			if ( GetComponent<Animation>() )   GetComponent<Animation>().Play(animationFind.name); 
		}
		
		//This function plays the object icon remove animation
		void ObjectIcon()
		{
			if ( GetComponent<Animation>() )   GetComponent<Animation>().Play(animationIcon.name); 
		}
		
		//This function runs when you click on it
		IEnumerator OnMouseDown()
		{
            //If this is one of the hidden objects, it is clickable
			if ( isClicked == false && isHidden == true )
			{
                isClicked = true;

                //Take the object out of the hierarchy
                transform.parent = null;
				
				//Update the number of found objects in the game controller
				GameObject.FindGameObjectWithTag("GameController").SendMessage("UpdateFoundObjects");
				
				//Play the find animation, if it exists
				if ( GetComponent<Animation>() )   
				{
					if ( animationFind )
					{
						GetComponent<Animation>().Play(animationFind.name); 

						yield return new WaitForSeconds(animationFind.length);
					}
				}
				
				//Finally, destroy the object
				Destroy(gameObject);
			}
		}
		
		//This function sets the rotation of the object as an icon ( used for the hidden object icons in the top bar )
		void SetIconRotation()
		{
			transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, iconRotation);
		}

		void Hidden( bool hiddenValue )
		{
			isHidden = hiddenValue;
		}
	}
}






