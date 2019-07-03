using UnityEngine;
using System.Collections;

public class ZippyWater2DAnimateColors : MonoBehaviour {

	ZippyWater2D water;
	public Gradient topColor;
	public Gradient bottomColor;
	public float speed = 1f;

	float time;

	void Awake () {
		water = GetComponent<ZippyWater2D>();
	}
	
	void Update () {
		time += Time.deltaTime * speed;
		water.topColor = topColor.Evaluate(time % 1);
		water.bottomColor = bottomColor.Evaluate(time % 1);
		water.GenerateMeshColors();
	}
}
