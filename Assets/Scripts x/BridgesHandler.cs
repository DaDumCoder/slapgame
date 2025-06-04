using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgesHandler : MonoBehaviour
{
    public List<BridgeSc> bridgesList = new List<BridgeSc>();
    List<int> showedBridges = new List<int>();
    public LevelManager levelManager;

    private void Start()
    {
        //RandomizeBridges(bridgesList);
    }

    public void RandomizeBridges(List<BridgeSc> bridgesList)
    {
        int count = bridgesList.Count;
        for (int i = 0; i < levelManager.presets.Length; i++)
        {
            for (int j = 0; j < count / levelManager.presets.Length; j++)
            {
                BridgeSc bridge = bridgesList[Random.Range(0, bridgesList.Count)];
                bridge.SetMat(levelManager.presets[i].gradiantMat, levelManager.presets[i].doorMat, LayerMask.NameToLayer("AllButPlayer" + (i + 1).ToString()),i+1);
                bridgesList.Remove(bridge);
            }
        }
    }

    public BridgeSc ShowMyBridge(int num)
    {
        if (showedBridges.Contains(num)) return null;
        showedBridges.Add(num);

        BridgeSc bridge = bridgesList[Random.Range(0, bridgesList.Count)];
        bridge.SetMat(levelManager.presets[num-1].gradiantMat, levelManager.presets[num-1].doorMat, LayerMask.NameToLayer("AllButPlayer" + num.ToString()), num);
        bridgesList.Remove(bridge);

        return bridge;
    }
}
