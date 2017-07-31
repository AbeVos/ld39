/* This wizard will replace a selection with an object or prefab.
 * Scene objects will be cloned (destroying their prefab links).
 * Original coding by 'yesfish', nabbed from Unity Forums
 * 'keep parent' added by Dave A (also removed 'rotation' option, using localRotation
 */
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceSelection : ScriptableWizard
{

    private static GameObject replacement;
    private static bool keep;

    public GameObject ReplacementObject;
    public bool KeepOriginals;

    [MenuItem("Tools/Replace Selection...")]
    private static void CreateWizard()
    {
        DisplayWizard(
            "Replace Selection", typeof(ReplaceSelection), "Replace");
    }

    public ReplaceSelection()
    {
        ReplacementObject = replacement;
        KeepOriginals = keep;
    }

    private void OnWizardUpdate()
    {
        replacement = ReplacementObject;
        keep = KeepOriginals;
    }

    private void OnWizardCreate()
    {
        if (replacement == null)
            return;

        Undo.RegisterCompleteObjectUndo(this, name);

        Transform[] transforms = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach (Transform t in transforms)
        {
            GameObject g;
            PrefabType pref = PrefabUtility.GetPrefabType(replacement);

            if (pref == PrefabType.Prefab || pref == PrefabType.ModelPrefab)
            {
                g = (GameObject)PrefabUtility.InstantiatePrefab(replacement);
            }
            else
            {
                g = Instantiate(replacement);
            }

            Transform gTransform = g.transform;
            gTransform.parent = t.parent;
            g.name = replacement.name;
            gTransform.localPosition = t.localPosition;
            gTransform.localScale = t.localScale;
            gTransform.localRotation = t.localRotation;
        }

        if (keep) return;

        foreach (GameObject g in Selection.gameObjects)
        {
            DestroyImmediate(g);
        }

    }
}
#endif