using UnityEngine;

/// <summary>
/// The SelectDojoBackground Class Changes the Dojo Background when we call ChangeDojoBG();
/// </summary>
public class SelectDojoBackground : MonoBehaviour
{
    public static SelectDojoBackground instance;    // static reference to our dojo bg changer
    public Texture2D[] bgToUse;                     // the array that holds our different background textures.
    private MeshRenderer mRenderer;                 // variable we will use to cache this Mesh Renderer
    private Material thisMaterial;                  // variable we will use to cache the Material of our Mesh Renderer

    // Use this for initialization
    void Start()
    {
        //cache our MeshRenderer on the Quad
        mRenderer = GetComponent<MeshRenderer>();
        //cache the Material of our new mRenderer reference.
        thisMaterial = mRenderer.sharedMaterial;
        //point instance to this!
        instance = this;
    }


    /// <summary>
    /// This method Increments the bgInt, and uses it to specify which element in the Texture2D Array will be used
    /// for the BG.  Each Material has the same Normal Map, and there are 4 textures it can be.  
    /// </summary>
    public void ChangeDojoBGNext()
    {
        //then we increase the bgInt... next time this method is called we will check the number again and change the bg.
        GameVariables.dojoBGNum++;
        //store the selected int value in PlayerPrefs
        PlayerPrefs.SetInt("BGint", GameVariables.dojoBGNum);

        //make sure that bgInt does not get larger than the number of elements in the Texture2D array.
        if (GameVariables.dojoBGNum > bgToUse.Length - 1)
        {
            //if it is larger we change the value to zero(the first element).
            GameVariables.dojoBGNum = 0;
            //store the selected int value in PlayerPrefs
            PlayerPrefs.SetInt("BGint", GameVariables.dojoBGNum);

        }

        //then we assign the thisMaterial reference's mainTexture the bgToUse array and pass the bgInt number as the element number.
        thisMaterial.mainTexture = bgToUse[GameVariables.dojoBGNum];



    }

    /// <summary>
    /// This method decrements the bgInt, and uses it to specify which element in the Texture2D Array will be used
    /// for the BG.  Each Material has the same Normal Map, and there are 4 textures it can be.  
    /// </summary>
    public void ChangeDojoBGPrevious()
    {
        //we decrement the bgInt... 
        GameVariables.dojoBGNum--;
        //store the selected int value in PlayerPrefs
        PlayerPrefs.SetInt("BGint", GameVariables.dojoBGNum);


        //make sure that bgInt does not get smaller than 0.
        if (GameVariables.dojoBGNum < 0)
        {
            //if it does make it the max int based on the values in the array(the last element).
            GameVariables.dojoBGNum = bgToUse.Length - 1;
            //store the selected int value in PlayerPrefs
            PlayerPrefs.SetInt("BGint", GameVariables.dojoBGNum);

        }

        //then we assign the thisMaterial reference's mainTexture the bgToUse array and pass the bgInt number as the element number.
        thisMaterial.mainTexture = bgToUse[GameVariables.dojoBGNum];



    }

}
