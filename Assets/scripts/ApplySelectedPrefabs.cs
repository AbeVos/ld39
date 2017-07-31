using UnityEditor;
using UnityEngine;


public class ApplySelectedPrefabs : EditorWindow
{
    public delegate void ApplyOrRevert(GameObject goCurrentGo, Object objPrefabParent, ReplacePrefabOptions eReplaceOptions);
    [MenuItem("Tools/Apply all selected prefabs %#a")]
    private static void ApplyPrefabs()
    {
        SearchPrefabConnections(ApplyToSelectedPrefabs);
    }

    [MenuItem("Tools/Revert all selected prefabs %#r")]
    private static void ResetPrefabs()
    {
        SearchPrefabConnections(RevertToSelectedPrefabs);
    }

    //Look for connections
    private static void SearchPrefabConnections(ApplyOrRevert applyOrRevert)
    {
        var tSelection = Selection.gameObjects;

        if (tSelection.Length <= 0) return;

        var iCount = 0;
        //Iterate through all the selected gameobjects
        foreach (var go in tSelection)
        {
            var prefabType = PrefabUtility.GetPrefabType(go);
            //Is the selected gameobject a prefab?
            if (prefabType != PrefabType.PrefabInstance &&
                prefabType != PrefabType.DisconnectedPrefabInstance) continue;
            //Prefab Root;
            var goPrefabRoot = ((GameObject)PrefabUtility.GetPrefabParent(go)).transform.root.gameObject;
            var goCur = go;
            var bTopHierarchyFound = false;
            var bCanApply = true;

            //We go up in the hierarchy to apply the root of the go to the prefab
            while (goCur.transform.parent != null && !bTopHierarchyFound)
            {
                //Are we still in the same prefab?
                if (goPrefabRoot == ((GameObject)PrefabUtility.GetPrefabParent(goCur.transform.parent.gameObject)).transform.root.gameObject)
                {
                    goCur = goCur.transform.parent.gameObject;
                }
                else
                {
                    //The gameobject parent is another prefab, we stop here
                    bTopHierarchyFound = true;
                    if (goPrefabRoot != ((GameObject)PrefabUtility.GetPrefabParent(goCur)))
                    {
                        //Gameobject is part of another prefab
                        bCanApply = false;
                    }
                }
            }

            if (applyOrRevert == null || !bCanApply) continue;
            iCount++;
            applyOrRevert(goCur, PrefabUtility.GetPrefabParent(goCur), ReplacePrefabOptions.ConnectToPrefab);
        }
        Debug.Log(iCount + " prefab" + (iCount > 1 ? "s" : "") + " updated");
    }

    //Apply      
    private static void ApplyToSelectedPrefabs(GameObject goCurrentGo, Object objPrefabParent, ReplacePrefabOptions eReplaceOptions)
    {
        PrefabUtility.ReplacePrefab(goCurrentGo, objPrefabParent, eReplaceOptions);
    }

    //Revert
    private static void RevertToSelectedPrefabs(GameObject goCurrentGo, Object objPrefabParent, ReplacePrefabOptions eReplaceOptions)
    {
        PrefabUtility.ReconnectToLastPrefab(goCurrentGo);
        PrefabUtility.RevertPrefabInstance(goCurrentGo);
    }

}