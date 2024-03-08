using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralMesh))]
public class ProceduralMeshEditor : Editor
{
    #region SerializedProperties
    SerializedProperty streamType;                                                  // The streamType property used set the stream type when the mesh is generated
    SerializedProperty meshType;                                                    // The meshType property used to set the type of mesh
    SerializedProperty meshMat;                                                     // The material property used to set the material of the mesh
    SerializedProperty resolution;                                                  // The resolution property used to set the resolution of the mesh
    #endregion

    private void OnEnable()
    {
        streamType = serializedObject.FindProperty("streamType");                   // Find the streamType property
        meshType = serializedObject.FindProperty("meshType");                       // Find the meshType property
        meshMat = serializedObject.FindProperty("meshMat");                         // Find the meshMat property
        resolution = serializedObject.FindProperty("resolution");                   // Find the resolution property
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();                                                  // Update the serializedObject

        EditorGUILayout.LabelField("Procedural Mesh Settings", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("This script, when attached to an object, allows you to generate procedural meshes. Adjust the settings below to generate your mesh. Mouse over the different options to learn more.", EditorStyles.wordWrappedLabel);

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

        EditorGUILayout.PropertyField(meshMat, new GUIContent("Mesh Material", "The material used to render the mesh (If blank, the default material provided in the 'Resources/Materials' folder will be used)"));
        EditorGUILayout.PropertyField(streamType, new GUIContent("Stream Type", "Switches between the different stream types (Multi-stream for complex meshes, Single-stream for simpler meshes)"));
        EditorGUILayout.PropertyField(meshType, new GUIContent("Mesh Type", "Switches between the different mesh types (Seperate verticies or shared verticies)"));
        EditorGUILayout.PropertyField(resolution, new GUIContent("Resolution", "Updates the resolution of the mesh (Higher = More Quads)"));


        serializedObject.ApplyModifiedProperties();                                 // Apply the changes to the serializedObject
    }
}
