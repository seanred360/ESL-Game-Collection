using UnityEngine;
using UnityEngine.UI;

public class bl_SpectrumUI : MonoBehaviour {

	[SerializeField]private RectTransform MainBar;

    private RectTransform ContainRect;

    public RectTransform RootRect
    {
        get
        {
            if(ContainRect == null)
            {
                ContainRect = GetComponent<RectTransform>();
            }
            return ContainRect;
        }
    }
    public RectTransform RectTransform
    {
        get
        {
            return MainBar;
        }
    }

    public Vector2 anchoredPosition
    {
        get
        {
          return  RectTransform.anchoredPosition;
        }
        set
        {
            RectTransform.anchoredPosition = value;
        }
    }

    public Vector2 sizeDelta
    {
        get
        {
            return RectTransform.sizeDelta;
        }
        set
        {
            RectTransform.sizeDelta = value;
        }
    }

    private Image _spectrumImage;
    public Image SpectrumImage
    {
        get
        {
            if(_spectrumImage == null)
            {
                _spectrumImage = RectTransform.GetComponent<Image>();
            }
            return _spectrumImage;
        }
    }

    public Color SpectrumColor
    {
        get
        {
            return SpectrumImage.color;
        }
        set
        {
            SpectrumImage.color = value;
        }
    }
}