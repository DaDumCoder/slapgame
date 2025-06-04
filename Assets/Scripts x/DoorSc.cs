using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSc : MonoBehaviour
{
    public Animator anim;
    public void SetDoorState(bool state)
    {
        anim.SetBool("Open", state);
    }
}
