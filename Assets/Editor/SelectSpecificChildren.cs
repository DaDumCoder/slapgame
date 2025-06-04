using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SelectSpecificChildren : EditorWindow
{
    [MenuItem("Tools/Select Specific Children")]
    static void SelectChildrenWithNames()
    {
        // Grab the selected GameObject in the Hierarchy
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogWarning("Select a GameObject in the Hierarchy first!");
            return;
        }

        // Target names to match
        string[] targetNames = { "Borders", "Door", "Gradiant", "Quad (3)", "Quad (4)", "Quad (5)", "Borders (2)", "Borders (1" +
                ")" };

        List<GameObject> matchedChildren = new List<GameObject>();

        // Search children recursively
        foreach (Transform child in selected.GetComponentsInChildren<Transform>(true))
        {
            foreach (string name in targetNames)
            {
                if (child.name == name)
                {
                    matchedChildren.Add(child.gameObject);
                }
            }
        }

        if (matchedChildren.Count == 0)
        {
            Debug.Log("No matching children found.");
            return;
        }

        // Select the matched children in the Editor
        Selection.objects = matchedChildren.ToArray();
        Debug.Log($"Selected {matchedChildren.Count} matching children.");
    }
}
