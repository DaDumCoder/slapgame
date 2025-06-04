using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToCam : MonoBehaviour
{
    Transform cam;

    

    private void Awake()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
