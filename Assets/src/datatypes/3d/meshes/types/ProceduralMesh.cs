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
    /// A set of enums to represent the types of mesh to be generated
    /// </summary>
    public enum MeshType
    {
        SquareGrid, SharedSquareGrid
    };

    /// <summary>
    /// The mesh object to be generated
    /// </summary>
    Mesh mesh;

    /// <summary>
    /// The type of mesh to be generated (Shared verticies or not)
    /// </summary>
    [SerializeField]
    MeshType meshType;

    /// <summary>
    /// The type of stream to be used for the mesh
    /// </summary>
    [SerializeField]
    StreamType streamType;

    /// <summary>
    /// The material of the mesh
    /// </summary>
    [SerializeField]
    Material meshMat;

    /// <summary>
    /// The resolution of the mesh
    /// </summary>
    [SerializeField, Range(1, 100)]
    int resolution = 1;

    /// <summary>
    /// The jobs to be executed
    /// </summary>
    static MeshJobScheduleDelegate[] jobs = 
    {
        MeshJob<SquareGrid, DataStream>.ScheduleParallel,
        MeshJob<SharedSquareGrid, DataStream>.ScheduleParallel
    };

    void Awake()
    {
        mesh = new Mesh
        {
            name = "Procedural Mesh"
        };

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void OnValidate() => enabled = true;                                        // Enable the script on load or when the resolution changes in the inspector

    void Update()
    {
        GenerateMesh();                                                         // Generate the mesh (In update to allow changing the resolution during runtime)

        enabled = false;                                                        // Disable the script so it isn't called constantly
    }

    /// <summary>
    /// Generates the mesh object and displays it
    /// </summary>
    void GenerateMesh() 
    {
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);    // Allocate mesh data
        Mesh.MeshData meshData = meshDataArray[0];                              // Get mesh data

        jobs[(int)meshType](mesh, meshData, resolution, default, streamType).Complete();

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);              // Apply and dispose mesh data

        if (meshMat != null)
        {
            GetComponent<Renderer>().material = meshMat;
        }
        else
        {
            meshMat = Resources.Load<Material>("Materials/Default");

            GetComponent<Renderer>().material = meshMat;
        }
    }
}