using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BossSc : MonoBehaviour
{
    public CharacterSc character;
    public Collider collDetect;

    public Renderer hintImg;
    public Material warningMat, handMat;

    bool hitting = false;
    public Image bar;


    private void Start()
    {
        character.SetRagdolState(false);
        SetGloveSize();
    }
    public void SetGloveSize()
    {
        character.gloveL.transform.DOComplete(); character.gloveR.transform.DOComplete();
        character.gloveL.transform.localScale = (character.gloveL.initScale * character.size);
        character.gloveR.transform.localScale = (character.gloveR.initScale * character.size);

        transform.localScale = Vector3.one + (Vector3.one * .3f * character.size);

        bar.fillAmount = (character.size - 1) / 2.25f;
    }


    public void Hit(Vector3 slapPos, Transform player, float playerSize)
    {
        if (character.dead) return;

        if (!hitting)
        {
            StartCoroutine(ChargeSlapBack(player, slapPos));
        }
        DecreaseGlove();
    }
    public void DecreaseGlove()
    {
        character.size = Mathf.Clamp(character.size - .2f, .9f, float.MaxValue);

        SetGloveSize();

        StartCoroutine(character.HitEffect());
    }

    IEnumerator ChargeSlapBack(Transform playerTrans,Vector3 slapPos)
    {
        hitting = true;
        character.anim.SetTrigger("Charge");

        PlayerSc player = playerTrans.GetComponent<PlayerSc>();

        transform.DOLookAt(playerTrans.position, .4f);

        yield return new WaitForSeconds(2f);

        if (1.0f >= character.size)
        {
            //DEAD
            character.dead = true;
            collDetect.transform.DOScale(Vector3.zero, .1f);
            hintImg.transform.parent.DOScale(Vector3.zero, .1f).OnComplete(() => { hintImg.gameObject.SetActive(false); });
            character.SetRagdolState(true);
            foreach (RagdollPart ragdoll in character.ragdolParts)
            {
                ragdoll.rb.AddExplosionForce(800, slapPos, 15f);
            }
            StartCoroutine(character.HitEffect());
            player.character.anim.SetTrigger("Normal");

            bar.transform.parent.gameObject.SetActive(false);

            player.ToFinishPos();
        }
        else
        {
            //SLAP BACK
            player.character.dead = true;
            player.character.anim.SetFloat("Movement", 0);


            yield return new WaitForSeconds(.3f);

            Vector3 line = (playerTrans.position - transform.position).normalized;
            //Debug.Break();

            if ((line.x * line.z) > 0)
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
        player.character.anim.ResetTrigger("SlapLoop");
        player.character.rb.isKinematic = false;
        hitting = false;

    }
}
