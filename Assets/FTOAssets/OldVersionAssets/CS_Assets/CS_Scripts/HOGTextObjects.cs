using UnityEngine;
using System.Collections;

namespace HiddneObjectGame
{
	public class HOGTextObjects:MonoBehaviour 
	{
		public Transform[] textObjects;
		
		void ChangeText( int textIndex, string changeValue )
		{
			textObjects[textIndex].Find("Text").GetComponent<TextMesh>().text = textObjects[textIndex].Find("Text/Shadow").GetComponent<TextMesh>().text = changeValue;
		}
	}
}