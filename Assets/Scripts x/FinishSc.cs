using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSc : MonoBehaviour
{
    public Transform doorPoint,insidePoint,terasPoint;
    public Animator doorAnim;
    public Renderer bulb;
    public ParticleSystem wonFx;

    public void BulbLight(Material mat)
    {
        StartCoroutine(bulbLighting(mat));
    }
    IEnumerator bulbLighting(Material mat)
    {
        Material matOld = bulb.material;
        yield return new WaitForSeconds(1.2f);
        bulb.material = mat;
        yield return new WaitForSeconds(.2f);
        bulb.material = matOld;
        yield return new WaitForSeconds(.1f);
        bulb.material = mat;
        yield return new WaitForSeconds(.2f);
        bulb.material = matOld;
        yield return new WaitForSeconds(.2f);
        bulb.material = mat;
    }
}
