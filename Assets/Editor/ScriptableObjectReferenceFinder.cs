using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ScriptableObjectReferenceFinder : EditorWindow
{
    private ScriptableObject targetSO;
    private List<Object> referencingObjects = new List<Object>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Find References to ScriptableObject")]
    static void ShowWindow()
    {
        GetWindow<ScriptableObjectReferenceFinder>("Find SO References");
    }

    void OnGUI()
    {
        GUILayout.Label("Select ScriptableObject to find references to it", EditorStyles.boldLabel);

        targetSO = (ScriptableObject)EditorGUILayout.ObjectField("ScriptableObject", targetSO, typeof(ScriptableObject), false);

        if (GUILayout.Button("Find References") && targetSO != null)
        {
            referencingObjects.Clear();
            FindAllReferences();
        }

        GUILayout.Space(10);
        GUILayout.Label($"Found {referencingObjects.Count} references:", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var obj in referencingObjects)
        {
            EditorGUILayout.ObjectField(obj, obj.GetType(), true);
        }

        EditorGUILayout.EndScrollView();
    }

    void FindAllReferences()
    {
        // »щем в сценах (MonoBehaviour)
        var sceneObjects = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (var mono in sceneObjects)
        {
            if (mono == null || EditorUtility.IsPersistent(mono))
                continue;

            if (ObjectHasReference(mono, targetSO))
                referencingObjects.Add(mono);
        }

        // »щем в ассетах проекта (ScriptableObject и других)
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (so == null)
                continue;

            if (ObjectHasReference(so, targetSO))
                referencingObjects.Add(so);
        }
    }

    bool ObjectHasReference(UnityEngine.Object obj, ScriptableObject target)
    {
        SerializedObject so = new SerializedObject(obj);
        SerializedProperty prop = so.GetIterator();
        while (prop.NextVisible(true))
        {
            if (prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue == target)
            {
                return true;
            }
        }
        return false;
    }
}
