using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Popingup());
    }
    IEnumerator Popingup()
    {
        transform.DOComplete();
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(0f, .5f));
        float time = Random.Range(.075f, .15f);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOScale(scale, time)).Append(transform.DOPunchScale(scale * Random.Range(.3f, .5f), time * 2, 0));

    }
}
