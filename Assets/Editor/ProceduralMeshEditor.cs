using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
    #region SerializedProperties
    SerializedProperty streamType;                                                  // The streamType property used set the stream type when the mesh is generated
    SerializedProperty resolution;                                                  // The resolution property used to set the resolution of the mesh
    #endregion

    private void OnEnable()
    {
        streamType = serializedObject.FindProperty("streamType");                   // Find the streamType property
        resolution = serializedObject.FindProperty("resolution");                   // Find the resolution property
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();                                                  // Update the serializedObject

        EditorGUILayout.PropertyField(streamType);                                  // Draw the streamType property
        EditorGUILayout.PropertyField(resolution);                                  // Draw the resolution property

        serializedObject.ApplyModifiedProperties();                                 // Apply the changes to the serializedObject
    }
}
