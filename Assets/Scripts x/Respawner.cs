using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject glove;


    public void SpawnerAfter(GameObject obj,float time,Vector3 pos,Transform parent)
    {
        StartCoroutine(spawnAfter(obj,time,pos,parent));
    }
    IEnumerator spawnAfter(GameObject obj,float time, Vector3 pos,Transform parent)
    {
        yield return new WaitForSeconds(time);

        Instantiate(obj, pos, Quaternion.identity,parent).GetComponent<GlovesCollectableSc>().Randomize();
    }
}
