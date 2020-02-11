using UnityEngine;
using DG.Tweening;

public class PlayerEffectsHandler : MonoBehaviour
{
    public Transform tornado;

    private void Awake()
    {
        tornado.gameObject.SetActive(true);
        tornado.gameObject.SetActive(false);
    }

    public void ActivateTornado(float duration)
    {
        transform.DOMove(new Vector3(transform.position.x, 2f, transform.position.z), .5f);
        transform.DORotate(new Vector3(0, transform.rotation.y + 3000, 0), 8f, RotateMode.FastBeyond360);
        tornado.gameObject.SetActive(true);
        Invoke("DeactivateTornado",duration);
    }

    void DeactivateTornado()
    {
        tornado.gameObject.SetActive(false);
    }
}
