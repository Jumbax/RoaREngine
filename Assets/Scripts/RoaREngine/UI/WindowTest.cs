// Create a foldable header menu that hides or shows the selected Transform position.
// If you have not selected a Transform, the Foldout item stays folded until
// you select a Transform.

using UnityEditor;

public class FoldoutHeaderUsage : EditorWindow
{
    bool showPosition = true;
    string status = "Select a GameObject";

    [MenuItem("Examples/Foldout Header Usage")]
    static void CreateWindow()
    {
        GetWindow<FoldoutHeaderUsage>();
    }

    public void OnGUI()
    {
        showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, status);

        if (showPosition)
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
        // End the Foldout Header that we began above.
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}