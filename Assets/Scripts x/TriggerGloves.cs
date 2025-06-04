using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGloves : MonoBehaviour
{
    public List<GlovesCollectableSc> gloves;
    public List<int> showedGloves = new List<int>();
    public LevelManager levelManager;
    
    private void Awake()
    {
        foreach (GlovesCollectableSc glove in gloves)
        {
            glove.gameObject.SetActive(false);
        }
        RandomizeGloves(new List<GlovesCollectableSc>(gloves));
    }
    public void RandomizeGloves(List<GlovesCollectableSc> glovesList)
    {
        int count = glovesList.Count;
        for (int i = 0; i < levelManager.presets.Length; i++)
        {
            for (int j = 0; j < count / levelManager.presets.Length; j++)
            {
                GlovesCollectableSc glove = glovesList[Random.Range(0, glovesList.Count)];
                glove.SetMat(levelManager.presets[i].gloveMat, LayerMask.NameToLayer("OnlyPlayer" + (i + 1).ToString()), i + 1);
                glovesList.Remove(glove);
            }
        }
    }

    public void ShowMyGloves(int player)
    {
        if (showedGloves.Contains(player)) return;

        showedGloves.Add(player);
        foreach (GlovesCollectableSc glove in gloves)
        {
            if (glove.playerGloves == player)
            {
                glove.gameObject.SetActive(true);
            }
        }
    }
}
