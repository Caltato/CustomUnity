using System.Collections;
using UnityEditor;
using UnityEngine;

public class AddParent : ScriptableObject
{
    [MenuItem("GameObject/Create Parent &#p", false, 0)]
    static void MenuInsertParent()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel |
            SelectionMode.OnlyUserModifiable);
        if (transforms.Length != 0)
        {
            GameObject newParent = new GameObject("GameObject");
            newParent.transform.position = FindThePivot(transforms);
            Transform newParentTransform = newParent.transform;

            if (transforms.Length == 1)
            {
                Transform originalParent = transforms[0].parent;
                transforms[0].parent = newParentTransform;
                if (originalParent)
                    newParentTransform.parent = originalParent;
            }

            else
            {
                Transform originalParent = transforms[0].parent;
                int count = 0;
                foreach (Transform transform in transforms)
                {
                    if (transform.IsChildOf(originalParent))
                        count++;
                    transform.parent = newParentTransform;
                }
                if (count == transforms.Length)
                    newParent.transform.parent = originalParent;
            }
            Selection.objects = new GameObject[] { newParent };
        }
    }

    static private Vector3 FindThePivot(Transform[] trans)
    {
        if (trans == null || trans.Length == 0)
            return Vector3.zero;
        if (trans.Length == 1)
            return trans[0].position;

        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float minZ = Mathf.Infinity;

        float maxX = -Mathf.Infinity;
        float maxY = -Mathf.Infinity;
        float maxZ = -Mathf.Infinity;

        foreach (Transform tr in trans)
        {
            if (tr.position.x < minX)
                minX = tr.position.x;
            if (tr.position.y < minY)
                minY = tr.position.y;
            if (tr.position.z < minZ)
                minZ = tr.position.z;

            if (tr.position.x > maxX)
                maxX = tr.position.x;
            if (tr.position.y > maxY)
                maxY = tr.position.y;
            if (tr.position.z > maxZ)
                maxZ = tr.position.z;
        }

        return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, (minZ + maxZ) / 2.0f);
    }
}

public class CutNPaste : ScriptableObject
{
    [MenuItem("GameObject/Cut %x")]
    static void Cut()
    {

    }
}

public class ChildrenCount : MonoBehaviour
{

}

public class InspectorLock : ScriptableObject
{
    private static EditorWindow selectedWindow;

    [MenuItem("Window/Toggle Inspector Lock %_L", false, 2102)]
    static void LockToggle()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Window/Refresh Inspector Lock %#_L", false, 2102)]
    static void RefreshLock()
    {
        if (ActiveEditorTracker.sharedTracker.isLocked)
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();

            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}