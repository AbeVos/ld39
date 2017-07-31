#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GroupSelected : EditorWindow
{


    /// <summary>
    ///  Creates an empty node at the center of all selected nodes and parents all selected underneath it. 
    ///  Basically a nice re-creation of Maya grouping!
    /// </summary>;

    [MenuItem("Tools/Group Selected", priority = 80)]
    private static void Init()
    {
        Transform[] selected = Selection.GetTransforms(SelectionMode.ExcludePrefab | SelectionMode.TopLevel);

        GameObject emptyNode = new GameObject();
        Vector3 averagePosition = selected.Aggregate(Vector3.zero, (current, node) => current + node.position);
        if (selected.Length > 0)
        {
            averagePosition /= selected.Length;
        }
        emptyNode.transform.position = averagePosition;
        emptyNode.name = "Group";
        foreach (Transform node in selected)
        {
            node.parent = emptyNode.transform;
        }
    }
}
#endif
