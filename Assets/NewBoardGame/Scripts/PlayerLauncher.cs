using UnityEngine;
using System.Collections;

public class PlayerLauncher : MonoBehaviour {

	public Rigidbody rb;
	public Transform target;

	public float h = 25;
	public float gravity = -18;
    float _time;

	public bool debugPath;

    float springAnimationTime = 0.7f;

	void Start() {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindObjectOfType<BoardNode>().transform;
        rb.useGravity = false;
	}

	void Update() {
		if (debugPath) {
			DrawPath ();
		}
	}

    public void StartLaunchRoutine(Transform _target)
    {
        StartCoroutine(Launch(_target));
    }

    IEnumerator Launch(Transform _target)
    {
        rb.isKinematic = false;
        //handle the animation first
        GameObject _spring = GetComponent<PlayerMover>().currentNode.gameObject.GetComponent<BoardNode>().spring.gameObject;
        _spring.SetActive(true);
        Transform standingTarget = GetComponent<PlayerMover>().currentNode.gameObject.GetComponent<BoardNode>().standingTarget;
        transform.position = standingTarget.position;
        transform.SetParent(standingTarget); // player stays on the moving spring
        yield return new WaitForSeconds(springAnimationTime);
        transform.SetParent(null);
        transform.localScale = new Vector3(1, 1, 1); //un squish the player
        _spring.SetActive(false);

        // launch the player
        target = _target;
        Physics.gravity = Vector3.up * gravity;
        rb.useGravity = true;
        rb.velocity = CalculateLaunchData().initialVelocity;
        yield return new WaitForSeconds(_time);
        rb.isKinematic = true;
    }

	LaunchData CalculateLaunchData() {
		float displacementY = target.position.y - rb.position.y;
		Vector3 displacementXZ = new Vector3 (target.position.x - rb.position.x, 0, target.position.z - rb.position.z);
		float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
        _time = time; // time until we land
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	void DrawPath() {
		LaunchData launchData = CalculateLaunchData ();
		Vector3 previousDrawPoint = rb.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up *gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = rb.position + displacement;
			Debug.DrawLine (previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}

	struct LaunchData {
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
		
	}
}
	