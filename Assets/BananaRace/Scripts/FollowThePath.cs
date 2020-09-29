using UnityEngine;

public class FollowThePath : MonoBehaviour {

    public Transform[] waypoints;
    public Animator playerAnim;
    public AudioManager audioManager;
    GameObject gameManager;

    [SerializeField]
    private float moveSpeed = 1f;

    //[HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    public bool BackMoveAllowed = false;

    // Use this for initialization
    private void Start () {
        gameManager = GameObject.Find("GameManager");
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed && !GameControl.gameOver)
        {
            Move();
        }

        if (BackMoveAllowed && !GameControl.gameOver)
        {
            BackMove();
        }
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Length - 1) // check if gameover
        {
            playerAnim.SetFloat("animSpeed", 1f);
            transform.position = Vector3.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                playerAnim.SetFloat("animSpeed", 0f);
                //audioManager.PlaySFX(3);
                waypointIndex += 1;
            }
        }
    }

    public void BackMove()
    {
        if (waypointIndex >= 0) //avoid array out of index exception
        {
            if (waypointIndex <= waypoints.Length - 1) // check if gameover
            {
                playerAnim.SetFloat("animSpeed", 1f);
                transform.position = Vector3.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
                moveSpeed * Time.deltaTime);

                if (transform.position == waypoints[waypointIndex].transform.position)
                {
                    playerAnim.SetFloat("animSpeed", 0f);
                    //audioManager.PlaySFX(3);
                    waypointIndex -= 1;
                }
            }
        }
    }
}
