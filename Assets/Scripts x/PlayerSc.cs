using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSc : MonoBehaviour
{
    public int num;
    public bool isMainPlayer;
    public CharacterSc character;
    List<EnemySc> enemyHints = new List<EnemySc>();
    public GameManager gameManager;

    public ParticleSystem dizzyFx;
    public BridgeSc currentBridge;
    public TriggerGloves currentGloves;

    bool hited;

    private void Start()
    {
       
        character.SetRagdolState(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Gloves"))
        {
            other.GetComponent<GlovesCollectableSc>().Collect(character.gloveR.transform, character.gloveL.transform);
            StartCoroutine(AddGlove(.4f));
        }
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Slapping(other.GetComponentInParent<EnemySc>()));
        }
        if (other.CompareTag("Door"))
        {
            other.GetComponentInParent<DoorSc>().SetDoorState(true);
        }
        if (other.CompareTag("EnemyHint"))
        {
            EnemySc enemyHint = other.GetComponentInParent<EnemySc>();
            enemyHint.SetHintState(true, character.size);
            enemyHints.Add(enemyHint);
        }
        if (other.CompareTag("BridgeArea"))
        {
            gameManager.CamZoom(45, 1f);
        }
        if (other.CompareTag("TriggerGloves"))
        {
            currentGloves = other.GetComponent<TriggerGloves>();
            currentGloves.ShowMyGloves(num);
            Physics.IgnoreCollision(other, character.coll);
        }
        if (other.CompareTag("Bridges"))
        {
            currentBridge = other.GetComponent<BridgesHandler>().ShowMyBridge(num);
            Physics.IgnoreCollision(other, character.coll);
        }
        if (other.CompareTag("PlayerDetect"))
        {
            StartCoroutine(SlappingPlayer(other.GetComponentInParent<PlayerSc>()));
        }
        if (other.CompareTag("TriggerBridge"))
        {
            if (!other.GetComponent<TriggerBridge>().HigherUpBridge(transform.position))
            {
                character.dead = true;
                character.rb.isKinematic = true;
                character.anim.SetFloat("Movement", 0);
            }
            else
            {
                FindObjectOfType<FinishSc>().BulbLight(character.rend.sharedMaterial);
                if (num != 1)
                {
                    //LOSE
                    FindObjectOfType<GameManager>().Lose();
                }
            }
        }

        if (other.CompareTag("Boss"))
        {
            StartCoroutine(SlappingBoss(other.GetComponentInParent<BossSc>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            other.GetComponentInParent<DoorSc>().SetDoorState(false);
        }
        if (other.CompareTag("EnemyHint"))
        {
            EnemySc enemyHint = other.GetComponentInParent<EnemySc>();
            enemyHint.SetHintState(false, character.size);
            enemyHints.Remove(enemyHint);
        }
        if (other.CompareTag("BridgeArea"))
        {
            gameManager.CamZoom(60, 1.5f);
        }
    }

    // ---------------- Enemy slap (unchanged) -----------------
    IEnumerator Slapping(EnemySc enemy)
    {
        Vector3 line = (enemy.transform.position - transform.position).normalized;

        if (Mathf.Abs(line.x) < .2f) line.x = Random.Range(-1.0f, 1.0f);

        if (line.x > 0)
        {
            character.anim.SetTrigger("SlapL");
            character.gloveL.trail.emitting = true;
        }
        else
        {
            character.anim.SetTrigger("SlapR");
            character.gloveR.trail.emitting = true;
        }
        yield return new WaitForSeconds(.07f);

        if (line.x > 0)
        {
            enemy.Hit(character.gloveL.transform.position, transform, character.size);
            character.gloveL.fx.Play();
            character.gloveL.trail.emitting = false;
        }
        else
        {
            enemy.Hit(character.gloveR.transform.position, transform, character.size);
            character.gloveR.fx.Play();
            character.gloveR.trail.emitting = false;
        }
        DecreaseGlove();
    }
    // ----------------------------------------------------------

    IEnumerator SlappingBoss(BossSc boss)
    {
        if (hited || character.rb.isKinematic) yield break;

        gameManager.CamZoom(40, .5f);
        character.move = false;
        character.rb.isKinematic = true;
        character.anim.SetTrigger("SlapLoop");

        while (!character.dead && character.rb.isKinematic)
        {
            yield return new WaitForSeconds(.5f);
            FindObjectOfType<ScoreManager>().AddSlapScoreByPlayerLayer(gameObject.layer);
            boss.Hit(transform.position, transform, character.size);
        }
        character.rb.isKinematic = false;
        character.move = true;
        gameManager.CamZoom(60, 1.5f);
    }

    public void PlayGlove(int side)
    {
        if (side < 0)
            character.gloveL.fx.Play();
        else
            character.gloveR.fx.Play();
    }

    IEnumerator AddGlove(float time)
    {
        yield return new WaitForSeconds(time);

        character.gloveL.transform.DOComplete(); character.gloveR.transform.DOComplete();
        character.gloveL.transform.DOPunchScale(character.gloveL.transform.localScale * .65f, .3f, 0);
        character.gloveR.transform.DOPunchScale(character.gloveR.transform.localScale * .65f, .3f, 0);

        StartCoroutine(HitGloveEffect());

        yield return new WaitForSeconds(.3f);

        character.size += .1f;

        character.gloveL.transform.DOComplete(); character.gloveR.transform.DOComplete();
        character.gloveL.transform.localScale = (character.gloveL.initScale * character.size);
        character.gloveR.transform.localScale = (character.gloveR.initScale * character.size);

        transform.localScale = Vector3.one + (Vector3.one * .15f * character.size);
    }

    public void DecreaseGlove()
    {
        character.size = Mathf.Clamp(character.size - .2f, .9f, float.MaxValue);

        character.gloveL.transform.DOComplete(); character.gloveR.transform.DOComplete();
        character.gloveL.transform.localScale = (character.gloveL.initScale * character.size);
        character.gloveR.transform.localScale = (character.gloveR.initScale * character.size);

        transform.localScale = Vector3.one + (Vector3.one * .15f * character.size);

        //REFRESH HINTS
        foreach (EnemySc enemy in enemyHints)
            enemy.SetHintState(true, character.size);
    }

    IEnumerator HitGloveEffect()
    {
        character.gloveR.rend.material.EnableKeyword("_EMISSION");
        character.gloveL.rend.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(.2f);
        character.gloveR.rend.material.DisableKeyword("_EMISSION");
        character.gloveL.rend.material.DisableKeyword("_EMISSION");
    }

    // ---------- Hit with source detection ----------
    public void Hit(Vector3 slapPos, Collider sourceCollider = null)
    {
        if (hited) return;
        hited = true;

        // ▶️ Only log if source is another player layer or boss tag
        if (sourceCollider != null)
        {
            string src = "";
            string layerName = LayerMask.LayerToName(sourceCollider.gameObject.layer);

            if (layerName == "Player1" || layerName == "Player2" || layerName == "Player3")
                src = layerName;
            else if (sourceCollider.CompareTag("Boss"))
                src = "Boss";

            if (!string.IsNullOrEmpty(src))
                Debug.Log($"Player {num} was hit by {src}!");
        }

        character.dead = true;
        character.anim.SetTrigger("Fall");
        character.anim.SetFloat("Movement", 0);

        character.rb.isKinematic = false;
        character.rb.AddForce(-transform.forward * 1000);
        StartCoroutine(HitEffect());
    }
    // -----------------------------------------------

    IEnumerator HitEffect()
    {
        Time.timeScale = .7f;
        Color c = character.rend.material.GetColor("_EmissionColor");
        character.rend.material.SetColor("_EmissionColor", character.hitColor);
        yield return new WaitForSeconds(.1f);
        character.rend.material.SetColor("_EmissionColor", c);
        Time.timeScale = 1.0f;
    }

    public void WakeUp()
    {
        character.dead = false;
        hited = false;
    }

    public void PlayDizzyFx() => dizzyFx.Play();

    // ---------------- Player-vs-Player slap -----------------
    IEnumerator SlappingPlayer(PlayerSc player)
    {
        if (character.size <= player.character.size) yield break;

        transform.DOLookAt(player.transform.position, .25f);

        Vector3 line = (player.transform.position - transform.position).normalized;
        if (Mathf.Abs(line.x) < .2f) line.x = Random.Range(-1.0f, 1.0f);

        if (line.x > 0)
        {
            character.anim.SetTrigger("SlapL");
            character.gloveL.trail.emitting = true;
        }
        else
        {
            character.anim.SetTrigger("SlapR");
            character.gloveR.trail.emitting = true;
        }
        yield return new WaitForSeconds(.07f);

        Collider hitColl = (line.x > 0)
            ? character.gloveL.GetComponent<Collider>()   // ⚙️ pass glove collider
            : character.gloveR.GetComponent<Collider>();


        Debug.Log($"[Slap] {gameObject.layer} ({LayerMask.LayerToName(gameObject.layer)}) slapped {player.gameObject.layer} ({LayerMask.LayerToName(player.gameObject.layer)})");
        // 🏆 Give 50 slap points
        FindObjectOfType<ScoreManager>().AddSlapScoreByPlayerLayer(gameObject.layer);
        if (line.x > 0)
        {
            player.Hit(character.gloveL.transform.position, hitColl);  // ⚙️ collider param
            character.gloveL.fx.Play();
            character.gloveL.trail.emitting = false;
        }
        else
        {
            player.Hit(character.gloveR.transform.position, hitColl);
            character.gloveR.fx.Play();
            character.gloveR.trail.emitting = false;
        }
        DecreaseGlove();
    }
    // --------------------------------------------------------

    // --------------- Finish sequence (unchanged) -------------
    public void ToFinishPos() => StartCoroutine(toFinishPos());

    IEnumerator toFinishPos()
    {
        FinishSc finish = FindObjectOfType<FinishSc>();
        yield return new WaitForSeconds(.1f);
        character.move = false;
        character.rb.isKinematic = true;
        yield return new WaitForSeconds(.5f);
        character.anim.SetTrigger("Normal");
        transform.DOLookAt(finish.doorPoint.position, .35f);

        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(finish.doorPoint.position, 2f)
                .OnComplete(() => finish.doorAnim.SetBool("Open", true)))
         .Append(transform.DOMove(finish.insidePoint.position, .5f)
                .OnComplete(() =>
                {
                    finish.doorAnim.SetBool("Open", false);
                    transform.DORotate(new Vector3(0, 180, 0), 1f);
                    character.anim.SetFloat("Movement", 0);
                }))
         .Append(transform.DOMove(finish.terasPoint.position, .3f)
                .OnComplete(() =>
                {
                    character.anim.SetTrigger("Won");
                    FindObjectOfType<GameManager>().Won();
                }));
    }
    // ---------------------------------------------------------
}
