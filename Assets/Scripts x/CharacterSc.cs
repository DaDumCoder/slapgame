using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSc : MonoBehaviour
{
    public Renderer rend;
    public Animator anim;
    public Rigidbody rb;
    public Color hitColor;
    public Color deadColor;
    public float size = 1;
    public GloveSc gloveR, gloveL;
    public bool dead = false,move = true;
    public RagdollPart[] ragdolParts;
    public Collider coll;

    Material mat;
    private void Awake()
    {
        mat = rend.material;
    }
    public IEnumerator HitEffect()
    {
        //Time.timeScale = .8f;
        Color c = mat.GetColor("_EmissionColor");
        mat.SetColor("_EmissionColor", hitColor);
        //Debug.Break();
        yield return new WaitForSeconds(.2f);
        if (dead)
        {
            mat.SetColor("_EmissionColor", deadColor);
        }
        else
        {
            mat.SetColor("_EmissionColor", c);
        }
        
        //Time.timeScale = 1.0f;
    }

    public void SetRagdolState(bool state)
    {
        foreach (RagdollPart ragdoll in ragdolParts)
        {
            ragdoll.coll.enabled = state;
            ragdoll.rb.useGravity = state;
            ragdoll.rb.isKinematic = !state;
        }
        coll.enabled = !state;
        if(rb!=null) rb.useGravity = !state;
        anim.enabled = !state;
    }

}
