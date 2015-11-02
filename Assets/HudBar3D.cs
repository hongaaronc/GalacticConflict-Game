using UnityEngine;
using System.Collections;

public class HudBar3D : MonoBehaviour {
    public enum direction {
        LeftToRight, RightToLeft, BottomToTop, TopToBottom
    }

    public direction myDirection;
    public float minValue;
    public float maxValue;
    public bool wholeNumbers;
    public float value;

    public Transform background;
    public Transform fill;
    public Transform scalingContainer;

    public bool rescale = false;

    public Color backgroundColor;
    public Color fillColor;

    void OnValidate()
    {
        if (rescale)
        {
            rescale = false;
            scalingContainer.localPosition = scalingContainer.localScale / -2f;
        }
    }

	void Awake () {
        if (backgroundColor != null)
            background.GetChild(0).GetComponent<MeshRenderer>().material.color = backgroundColor;
        if (fillColor != null)
            fill.GetChild(0).GetComponent<MeshRenderer>().material.color = fillColor;
	}
	
	// Update is called once per frame
	void Update () {
        float valueAsPercent = (value - minValue) / (maxValue - minValue);
        if (myDirection == direction.LeftToRight)
        {
            fill.localScale = new Vector3(valueAsPercent, 1f, 1f);
            fill.localPosition = new Vector3(0f, 0f, 0f);
        }
        else if (myDirection == direction.RightToLeft)
        {
            fill.localScale = new Vector3(valueAsPercent, 1f, 1f);
            fill.localPosition = new Vector3(1f - valueAsPercent, 0f, 0f);
        }
        else if (myDirection == direction.BottomToTop)
        {
            fill.localScale = new Vector3(1f, valueAsPercent, 1f);
            fill.localPosition = new Vector3(0f, 0f, 0f);
        }
        else if (myDirection == direction.TopToBottom)
        {
            fill.localScale = new Vector3(1f, valueAsPercent, 1f);
            fill.localPosition = new Vector3(0f, 1f - valueAsPercent, 0f);
        }
	}
}
