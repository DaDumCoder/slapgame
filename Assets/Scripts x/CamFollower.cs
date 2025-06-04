using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    public Transform following;
    Vector3 offset;
    //0 12.35 -11.26 normal
    //0 13.5 -21 FINISH

    Vector3 velocity;
    public float smoothFactor = 1;

    void Start()
    {
        offset = transform.position - following.position;
    }

    void FixedUpdate()
    {
        Vector3 follow = following.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, follow, ref velocity, smoothFactor);
    }
}
