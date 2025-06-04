using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int[] playerScores = new int[3]; // Index 0 → Player1, Index 1 → Player2, Index 2 → Player3
    public TextMeshProUGUI[] scoreTexts;    // Assign 3 Text objects in Inspector

    public void AddScoreByPlayerLayer(int layer)
    {
        // Convert Layer name to player index
        for (int i = 0; i < 3; i++)
        {
            string expectedLayerName = "OnlyPlayer" + (i + 1).ToString();
            if (layer == LayerMask.NameToLayer(expectedLayerName))
            {
                playerScores[i]++;
                UpdateScoreText(i);
                break;
            }
        }
    }
    public void AddSlapScoreByPlayerLayer(int layer)
    {
        Debug.Log($"AddSlapScoreByPlayerLayer called with layer {layer} ({LayerMask.LayerToName(layer)})");

        for (int i = 0; i < 3; i++)
        {
            string expectedLayerName = "Player" + (i + 1).ToString();  // Changed here
            if (layer == LayerMask.NameToLayer(expectedLayerName))
            {
                playerScores[i] += 50;
                Debug.Log($"Player {i + 1} score increased to {playerScores[i]}");
                UpdateScoreText(i);
                break;
            }
        }
    }



    void UpdateScoreText(int index)
    {
        if (index >= 0 && index < scoreTexts.Length)
        {
            scoreTexts[index].text = $"{playerScores[index]}";
        }
    }
}
