using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerBridge : MonoBehaviour
{
    public Transform[] bridges;
    bool triggered = false;

    public bool HigherUpBridge(Vector3 pos)
    {
        if (triggered) return false;
        triggered = true;

        if (pos.x< 3f && pos.x>-3f)
        {
            bridges[1].DOLocalMoveY(-6.32f, .2f);
        }
        else
        {
            if (pos.x < -3f)
            {
                bridges[0].DOLocalMoveY(-6.32f, .2f);
            }
            else
            {
                bridges[2].DOLocalMoveY(-6.32f, .2f);
            }
        }

        return true;

    }

    /*public void ActiveBridge(int b)
    {
        for (int i = 0; i < length; i++)
        {

        }
    }*/
}
