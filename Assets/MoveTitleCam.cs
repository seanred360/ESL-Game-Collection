using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoveTitleCam : MonoBehaviour
{
    public Camera cam;
    public GameObject titleText;
    public Transform grass,bananaParticle, canvas;
    public Text touchToStart;
    public float duration, strength, randomness;
    public int vibrato;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMove(new Vector3(0, 0, 0), 2);
        StartCoroutine(FadeInTitleText());
        StartCoroutine(ShakeScale());
        grass.DOMoveX(1618f, 2f, false);
    }

 IEnumerator FadeInTitleText()
    {
        yield return new WaitForSeconds(2f);
        titleText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        StartCoroutine(ShakeTitle());
        StartCoroutine(FadeTextToFullAlpha(1f, touchToStart));
    }

    IEnumerator ShakeTitle()
    {
        yield return new WaitForSeconds(1f);
        titleText.transform.DOShakeRotation(duration, new Vector3(0,0,strength), vibrato, randomness, false).SetLoops(99999999);
    }

    IEnumerator ShakeScale()
    {
        touchToStart.transform.DOShakeScale(1f, .05f, 10, 90, true).SetLoops(999999999);
        yield return null;
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public void SpawnBananaParticle()
    {
        Vector3 posVec = cam.ScreenToWorldPoint(Input.mousePosition);
        posVec.z = touchToStart.transform.position.z;

        Transform clone;
        clone = Instantiate(bananaParticle, posVec , transform.rotation);
        clone.SetParent(canvas);
    }
}
