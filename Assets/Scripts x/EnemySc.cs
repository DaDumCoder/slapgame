using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemySc : MonoBehaviour
{
    public Collider collDetect;

    public CharacterSc character;
    public Renderer hintImg;
    public Material warningMat, handMat;

    Vector3 initScale;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    private void Start()
    {
        character.size = Random.Range(1f, 2f);
        character.SetRagdolState(false);
        SetGloveSize();
        character.rend.material.SetColor("_BaseColor", FindObjectOfType<LevelManager>().enemiesColor.Evaluate((character.size-1f)));
    }
    public void SetGloveSize()
    {
        character.gloveL.transform.localScale = (character.gloveL.transform.localScale * character.size);
        character.gloveR.transform.localScale = (character.gloveR.transform.localScale * character.size);

        transform.localScale = initScale + (initScale * .1f * character.size);
    }



    public void Hit(Vector3 slapPos,Transform player,float playerSize)
    {
        if (character.dead) return;


        if (playerSize >= character.size)
        {
            //SLAP
            character.dead = true;
            collDetect.transform.DOScale(Vector3.zero, .1f);
            hintImg.transform.parent.DOScale(Vector3.zero, .1f).OnComplete(()=>{ hintImg.gameObject.SetActive(false); });
            character.SetRagdolState(true);
            foreach (RagdollPart ragdoll in character.ragdolParts)
            {
                ragdoll.rb.AddExplosionForce(800, slapPos, 15f);
            }
            StartCoroutine(character.HitEffect());
            transform.parent.parent.GetComponent<BridgeSc>().enemies.Remove(this);
        }
        else
        {
            //SLAP BACK
            //collDetect.transform.DOScale(Vector3.zero, .1f);
            StartCoroutine(SlapBack(player));
        }
    }

    IEnumerator SlapBack(Transform playerTrans)
    {
        PlayerSc player = playerTrans.GetComponent<PlayerSc>();

        player.character.dead = true;
        player.character.anim.SetFloat("Movement", 0);

        transform.DOLookAt(playerTrans.position, .4f);
        yield return new WaitForSeconds(.3f);

        Vector3 line = (playerTrans.position - transform.position).normalized;
        //Debug.Break();
        
        if ((line.x*line.z) > 0)
        {
            character.anim.SetTrigger("SlapL");
            character.gloveL.trail.emitting = true;
        }
        else
        {
            character.anim.SetTrigger("SlapR");
            character.gloveR.trail.emitting = true;
        }
        yield return new WaitForSeconds(.25f);

        if ((line.x * line.z) > 0)
        {
            player.Hit(transform.position);
            character.gloveL.fx.Play();
            character.gloveL.trail.emitting = false;
        }
        else
        {
            player.Hit(transform.position);
            character.gloveR.fx.Play();
            character.gloveR.trail.emitting = false;
        }
    }

    public void SetHintState(bool state,float size)
    {
        hintImg.DOComplete();
        if (state)
        {
            if (character.size <= size)
            {
                hintImg.sharedMaterial = handMat;
            }
            else
            {
                hintImg.sharedMaterial = warningMat;
            }

            hintImg.transform.parent.DOScale(Vector3.one,.2f);
        }
        else
        {
            hintImg.transform.parent.DOScale(Vector3.zero, .1f);
        }
    }

}

[System.Serializable]
public struct RagdollPart
{
    public Collider coll;
    public Rigidbody rb;
}
