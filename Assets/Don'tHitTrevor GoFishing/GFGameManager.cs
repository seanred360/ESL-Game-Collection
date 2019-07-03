using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GFGameManager : MonoBehaviour
{
    public GameObject missButton;
    public GameObject hitParticle;
    public Camera cam;
    public Animator anim;
    public AudioManager audioManager;
    public Canvas gameOverCanvas;
    public GameObject water;
    public float duration, strength, randomness;
    public int vibrato;
    Vector2 screenPos;
    Vector2 worldPos;
    GameObject clone;
    bool waterIsFull = false;

    public void HitWater()
    {
        StartCoroutine(ShakeScale());
        StartCoroutine(ShakeTitle());

        audioManager.PlaySFX(Random.Range(0, audioManager.SClips.Length));
        screenPos = Input.mousePosition;
        worldPos = cam.ScreenToWorldPoint(screenPos);
        clone = Instantiate(hitParticle, worldPos, Quaternion.identity);
    }

    public void HitFish()
    {
        audioManager.PlayMusic(Random.Range(0, audioManager.MClips.Length));
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Fish") == null)
        {
            gameOverCanvas.gameObject.SetActive(true);
        }

        if (water == null)
        {
            water = GameObject.FindGameObjectWithTag("Water");
        }

        if (!water.gameObject.activeInHierarchy)
        {
            water = GameObject.FindGameObjectWithTag("Water");
        }

        if (gameOverCanvas.gameObject.activeSelf == true && waterIsFull == false)
        {
            water.GetComponent<ZippyWater2D>().height += 2 * Time.deltaTime;
            water.transform.position += new Vector3(0, 2f, 0f) * Time.deltaTime;
            if(water.transform.position.y >= 4f)
            {
                waterIsFull = true;
            }
        }
    }

    IEnumerator ShakeTitle()
    {
        yield return new WaitForSeconds(1f);
        missButton.transform.DOShakeRotation(duration, new Vector3(0, 0, strength), vibrato, randomness, false).SetLoops(0);
    }

    IEnumerator ShakeScale()
    {
        missButton.transform.DOShakeScale(1f, .05f, 10, 90, true).SetLoops(0);
        yield return null;
    }


}
