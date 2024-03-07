using DataType3D;
using DataType3D.Generators;
using DataType3D.Streams;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Generates a procedural mesh (Requires MeshFilter and MeshRenderer)
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour 
{
    /// <summary>
    /// The mesh object to be generated
    /// </summary>
    Mesh mesh;

    void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };

        GenerateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    /// <summary>
    /// Generates the mesh object and displays it
    /// </summary>
    void GenerateMesh() 
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);    // Allocate mesh data
        Mesh.MeshData meshData = meshDataArray[0];                              // Get mesh data

        MeshJob<SquareGrid, DataStream>.ScheduleParallel                        // Schedule the job to be executed
        ( 
            meshData, default, StreamType.Multi
        ).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);              // Apply and dispose mesh data
    }
}