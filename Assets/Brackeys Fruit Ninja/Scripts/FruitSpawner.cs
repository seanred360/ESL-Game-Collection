using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

	public GameObject originalPrefab;
	public GameObject[] prefabs;
	public Transform[] spawnPoints;
	public Sprite[] sprites;
	ObjectPooler objectPooler;

	public float minDelay = .1f;
	public float maxDelay = 1f;

    private void Awake()
    {
		sprites = LevelDataChanger.instance.LoadSprites();
	}

    // Use this for initialization
    void Start () 
	{
		StartCoroutine(SpawnFruits());
	}

	IEnumerator SpawnFruits ()
	{
		while (true)
		{
			float delay = Random.Range(minDelay, maxDelay);
			yield return new WaitForSeconds(delay);

			int spawnIndex = Random.Range(0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints[spawnIndex];

			//GameObject spawnedFruit = objectPooler.GetPooledObject();
			GameObject spawnedFruit = Instantiate(originalPrefab);
			spawnedFruit.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0,sprites.Length)];
			spawnedFruit.AddComponent<BoxCollider2D>();
			spawnedFruit.transform.position = spawnPoint.position;
			spawnedFruit.SetActive(true);
			Destroy(spawnedFruit, 5f);
		}
	}
}
