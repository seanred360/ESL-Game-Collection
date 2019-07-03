using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SineMove : MonoBehaviour
{
    public float amplitude = 10f;
    public float duration = 0.5f;

    void Start()
    {
        transform.DOLocalMoveY(transform.localPosition.y - amplitude, duration)
            .ChangeStartValue(transform.localPosition.y + amplitude)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .Goto(duration / 2, true);

        transform.DOLocalMoveX(transform.localPosition.x + amplitude, duration / 2)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.InOutElastic)
            .Play();
    }
}
