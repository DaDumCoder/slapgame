using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSc : MonoBehaviour
{
    public Renderer gradiant, gradiantL, gradiantR, doorL, doorR;
    public Transform targetBridge,targetDoor;
    public List<EnemySc> enemies;
    public void SetMat(Material gradiantMat,Material doorMat,LayerMask layer,int num)
    {
        Material[] doorMats = new Material[2];
        doorMats[0] = doorL.sharedMaterials[0];
        doorMats[1] = doorMat;
        gradiant.sharedMaterial = gradiantMat;
        gradiantL.sharedMaterial = gradiantMat;
        gradiantR.sharedMaterial = gradiantMat;
        doorL.sharedMaterials = doorMats;
        doorR.sharedMaterials = doorMats;

        gradiant.gameObject.layer = layer;
        targetBridge.gameObject.tag = "BridgePos" + num;
    }

    public float GetEnemySize()
    {
        if (enemies.Count>0)
        {
            return enemies[0].character.size;
        }
        else
        {
            return 0;
        }
        
    }
}
