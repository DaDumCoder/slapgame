using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GlovesCollectableSc : MonoBehaviour
{
    public int playerGloves;
    public Transform gloveR, gloveL;
    public Renderer[] rends;

    public void Collect(Transform handR,Transform handL)
    {
        gameObject.tag = "Untagged";
        TransformGlove(gloveR, handR);
        TransformGlove(gloveL, handL);

        // 🟢 Add score for correct player based on glove layer
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
            scoreManager.AddScoreByPlayerLayer(gameObject.layer);
        Destroy(gameObject);
    }

    void TransformGlove(Transform glove,Transform hand)
    {
        glove.parent = hand;
        glove.DOLocalMove(Vector3.zero, .4f).SetEase(Ease.InCubic);
        glove.DOPunchScale(glove.localScale, .4f,0);
        glove.DOLocalRotate(Vector3.zero, .35f);
        //glove.DOScale(Vector3.zero, .6f).SetEase(Ease.InBack);
    }

    private void OnDestroy()
    {
        Destroy(gloveR.gameObject,.45f);
        Destroy(gloveL.gameObject,.45f);

        try
        {
            Respawner respawner = FindObjectOfType<Respawner>();
            respawner.SpawnerAfter(respawner.glove,
                Random.Range(2, 6),
                transform.position,
                transform.parent);
        }
        catch (System.Exception)
        {

        }

    }

    public void SetMat(Material mat,LayerMask layer,int player)
    {
        playerGloves = player;
        foreach (Renderer rend in rends)
        {
            rend.sharedMaterial = mat;
        }
        gameObject.layer = layer;
    }


    public void Randomize()
    {
        TriggerGloves triggerGloves = GetComponentInParent<TriggerGloves>();

        int p = triggerGloves.showedGloves[Random.Range(0, triggerGloves.showedGloves.Count)];
        SetMat(triggerGloves.levelManager.presets[p-1].gloveMat, LayerMask.NameToLayer("OnlyPlayer" + (p).ToString()), p);
    }
}
