using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace MatchingGameTemplate
{
	/// <summary>
	/// Selects a certain button when this canvas/panel/object is enabled
	/// </summary>
	public class MGTSelectButton : MonoBehaviour 
	{
		// The button to select
		public GameObject selectedButton;

		// The previously selected button
		internal GameObject previousButton;

		// Force the button to be selected if no other button is selected.
		public bool forceSelect = false;
		/// <summary>
		/// Runs when this object is enabled
		/// </summary>
		void OnEnable() 
		{
			if ( EventSystem.current ) 
			{
				//Remember the currently selected button so that we can revert to it when needed
				previousButton = EventSystem.current.currentSelectedGameObject;
				
				// Select the button
				if (selectedButton)    EventSystem.current.SetSelectedGameObject (selectedButton);
			}
		}

		void Update()
		{
			// If no button was selected, make another check
			if ( forceSelect == true && EventSystem.current.currentSelectedGameObject == null )    EventSystem.current.SetSelectedGameObject(selectedButton);
		}

		/// <summary>
		/// Reverts to the previosly selected button, before enabling this object
		/// </summary>
		public void RevertSelection()
		{
			// Select the previous button
			EventSystem.current.SetSelectedGameObject(previousButton);
		}
	}
}
