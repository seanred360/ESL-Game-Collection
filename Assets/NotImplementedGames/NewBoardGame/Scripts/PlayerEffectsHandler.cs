using UnityEngine;
using DG.Tweening;

public class PlayerEffectsHandler : MonoBehaviour
{
    public Transform tornado;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        tornado.gameObject.SetActive(true);
        tornado.gameObject.SetActive(false);
    }

    public void ActivateTornado(float duration)
    {
        anim.SetBool("isSpinning", true);
        transform.DOMove(new Vector3(transform.position.x, 2f, transform.position.z), .5f);
        transform.DORotate(new Vector3(0, transform.rotation.y + 3000, 0), 8f, RotateMode.FastBeyond360);
        tornado.gameObject.SetActive(true);
        Invoke("DeactivateTornado",duration);
    }

    void DeactivateTornado()
    {
        anim.SetBool("isSpinning", false);
        tornado.gameObject.SetActive(false);
    }
}
