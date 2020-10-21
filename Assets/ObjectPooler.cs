using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    Sprite[] sprites;

    private void Awake()
    {
        instance = this;
        sprites = LevelDataChanger.instance.LoadSprites();
        amountToPool = sprites.Length;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.GetComponent<SpriteRenderer>().sprite = sprites[i];
            obj.AddComponent<PolygonCollider2D>();
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
