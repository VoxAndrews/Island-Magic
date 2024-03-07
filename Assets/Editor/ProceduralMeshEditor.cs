using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
    #region SerializedProperties
    SerializedProperty streamType;                                                  // The streamType property used set the stream type when the mesh is generated
    #endregion

    private void OnEnable()
    {
        streamType = serializedObject.FindProperty("streamType");                   // Find the streamType property

        if (streamType == null)
        {
            Debug.LogError("Failed to find the 'streamType' property. Make sure the property name is correct.");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();                                                  // Update the serializedObject

        EditorGUILayout.PropertyField(streamType);                                  // Draw the streamType property

        serializedObject.ApplyModifiedProperties();                                 // Apply the changes to the serializedObject
    }
}
