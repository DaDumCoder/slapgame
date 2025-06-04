using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveSc : MonoBehaviour
{
    public Renderer rend;
    public TrailRenderer trail;
    public ParticleSystem fx;


    [HideInInspector] public Vector3 initScale;
    private void Awake()
    {
        initScale = transform.localScale;
    }
}
