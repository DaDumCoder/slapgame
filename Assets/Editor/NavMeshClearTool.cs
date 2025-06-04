#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.AI;  // 🔥 Yeh line important hai!

public class NavMeshClearTool
{
    [MenuItem("Custom Tools/Clear Baked NavMesh")]
    static void ClearBakedNavMesh()
    {
        NavMeshBuilder.ClearAllNavMeshes();  // Ab koi error nahi aayega
        Debug.Log("🧹 Scene ka baked NavMesh clear kar diya gaya!");
    }
}
#endif
