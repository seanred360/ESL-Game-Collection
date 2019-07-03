using UnityEngine;
using System.Collections;

namespace HiddenObjectGame
{
	/// <summary>
	/// This script animates a sprite or a text mesh with several colors over time. You can set a list of colors, and the speed at which they change.
	/// </summary>
	public class HOGAnimateColors:MonoBehaviour 
	{
		//A list of the colors that will be animated
		public Color[] colorList;
		
		//The index number of the current color in the list
		public int colorIndex = 0;
		
		//How long the animation of the color change lasts, and a counter to track it
		public float changeTime = 1;
		public float changeTimeCount = 0;
		
		//How quickly the sprite animates from one color to another
		public float changeSpeed = 1;
		
		//Is the animation paused?
		public bool isPaused = false;
		
		//Is the animation looping?
		public bool isLooping = true;

		/// <summary>
		/// Start is only called once in the lifetime of the behaviour.
		/// The difference between Awake and Start is that Start is only called if the script instance is enabled.
		/// This allows you to delay any initialization code, until it is really needed.
		/// Awake is always called before any Start functions.
		/// This allows you to order initialization of scripts
		/// </summary>
		void Start() 
		{
			//Apply the chosen color to the sprite or text mesh
			SetColor();
		}
		
		// Update is called once per frame
		void Update() 
		{
			//If the animation isn't paused, animate it over time
			if ( isPaused == false )
			{
				if ( changeTime > 0 )
				{
					//Count down to the next color change
					if ( changeTimeCount > 0 )
					{
						changeTimeCount -= Time.deltaTime;
					}
					else
					{
						changeTimeCount = changeTime;
						
						//Switch to the next color
						if ( colorIndex < colorList.Length - 1 )
						{
							colorIndex++;
						}
						else
						{
							if ( isLooping == true )    colorIndex = 0;
						}
					}
				}
				
				//If we have a text mesh, animated its color
				if ( GetComponent<TextMesh>() )
				{
					GetComponent<TextMesh>().color = Color.Lerp(GetComponent<TextMesh>().color, colorList[colorIndex], changeSpeed * Time.deltaTime);
				}
				
				//If we have a sprite renderer, animated its color
				if ( GetComponent<SpriteRenderer>() )
				{
					GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, colorList[colorIndex], changeSpeed * Time.deltaTime);
				}
			}
			else
			{
				//Apply the chosen color to the sprite or text meshh
				SetColor();
			}
		}

		/// <summary>
		/// Applies the chosen color to the sprite based on the index from the list of colors
		/// </summary>
		void SetColor()
		{
			//If you have a text mesh component attached to this object, set its color
			if ( GetComponent<TextMesh>() )
			{
				GetComponent<TextMesh>().color = colorList[0];
			}
			
			//If you have a sprite renderer component attached to this object, set its color
			if ( GetComponent<SpriteRenderer>() )
			{
				GetComponent<SpriteRenderer>().color = colorList[0];
			}
		}

		/// <summary>
		/// Sets the pause state of the color animation
		/// </summary>
		/// <param name="pauseState">Pause state, true paused, false unpaused</param>
		void Pause( bool pauseState )
		{
			isPaused = pauseState;
		}

	}
}
