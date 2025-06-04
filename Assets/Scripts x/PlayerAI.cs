using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    public PlayerSc player;
    public Transform target;
    public Animator anim;
    
    Vector3 previousPosition = Vector3.zero;
    public NavMeshAgent navMeshAgent;
    float curSpeed = 0;

    public CharacterSc character;

    bool toBridge,waitToBridge;

    private void Update()
    {
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = (curMove.magnitude / Time.deltaTime) / navMeshAgent.speed;
        previousPosition = transform.position;

        anim.SetFloat("Movement", curSpeed);

        /*if (toBridge && curSpeed <.1f)
        {
            toBridge = false;
        }*/
    }
    private void FixedUpdate()
    {
        if (!character.move)
        {
            navMeshAgent.SetDestination(transform.position);
            return;
        }
        if (character.dead )
        {
            navMeshAgent.SetDestination(transform.position);
            target = null;
            toBridge = false;
            waitToBridge = false;
            return;
        }

        if (player.currentBridge != null && character.size>=player.currentBridge.GetEnemySize()
            && !waitToBridge)
        {
            StartCoroutine(ToBridge());
        }

        if (toBridge && (target != player.currentBridge.targetBridge || target != player.currentBridge.targetDoor))
        {
            target = GetNearestGlove();
            toBridge = false;
            waitToBridge = false;
        }

        if (target==null)
        {
            target = GetNearestGlove();
        }
        else
        {
            navMeshAgent.SetDestination(target.position);
        }
        

        
    }

    IEnumerator ToBridge()
    {
        waitToBridge = true;
        yield return new WaitForSeconds(Random.Range(2f,10f));
        toBridge = true;
        target = player.currentBridge.targetBridge;
        while (toBridge && Vector3.Distance(transform.position, player.currentBridge.targetBridge.position) >1f)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        target = player.currentBridge.targetDoor;
    }



    public Transform GetNearestGlove()
    {
        if (player.currentGloves == null)
        {
            return null;
        }

        List<GlovesCollectableSc> gloves = new List<GlovesCollectableSc>();
        gloves.AddRange(player.currentGloves.GetComponentsInChildren<GlovesCollectableSc>());

        //if (gloves.Count <= 0) return transform;

        float minDist = Mathf.Infinity;
        Transform nearestObj = transform;
        foreach (GlovesCollectableSc glove in gloves)
        {
            float dist = Vector3.Distance(glove.transform.position, transform.position);
            if (player.num == glove.playerGloves && dist < minDist)
            {
                minDist = dist;
                nearestObj = glove.transform;
                if (dist < 2)
                {
                    break;
                }
            }
        }
        return nearestObj;
    }
}
