using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;                                                                    // Included for math functions and types
using static Unity.Mathematics.math;                                                        // Included to use math functions without needing to prefix with 'Math.'
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]                                // Requires a MeshFilter (Reference to a mesh) and MeshRenderer (Reference to the mesh renderer) to manipulate and generate the mesh
public class TerrainGeneration : MonoBehaviour
{
                                                                                            // Awake is called before Start when the instance of the script is being loaded
    private void Awake()
    {
        /////////////////////////////////////////////////////////////////
        ///
        ///         Allocate Mesh Data
        ///
        /////////////////////////////////////////////////////////////////

        int vertexAttributeCount = 4;                                                       // Number of vertex attributes (Position, Normal, Tangent, UV)
        int vertexCount = 4;                                                                // Number of vertices in the mesh
        int triangleIndexCount = 6;                                                         // Number of triangles in the mesh
        half h0 = half(0.0f), h1 = half(1.0f);                                              // Temporary variables for half float conversion

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);                // Allocate the mesh data to the mesh data array
        Mesh.MeshData meshData = meshDataArray[0];                                          // Get the mesh data from the mesh data array

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>                   // Create an array of vertex attributes
        (
			vertexAttributeCount, Allocator.Temp                                            // Can optionally choose to unitilize the array and allow garbage data with 'NativeArrayOptions.UninitializedMemory'
        );

        vertexAttributes[0] = new VertexAttributeDescriptor(dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor
        (
            VertexAttribute.Normal, dimension: 3, stream: 1
        );
        vertexAttributes[2] = new VertexAttributeDescriptor
        (
            VertexAttribute.Tangent, VertexAttributeFormat.Float16, 4, 2
        );
        vertexAttributes[3] = new VertexAttributeDescriptor
        (
            VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2, 3
        );

        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);                      // Set the vertex buffer parameters (Reserves memory for the vertex attributes buffer)
        vertexAttributes.Dispose();                                                         // Dispose the vertex attributes

        meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt16);              // Set the index buffer parameters (Reserves memory for the vertex index buffer)

        /////////////////////////////////////////////////////////////////
        ///
        ///         Retrieve Mesh Data
        ///
        /////////////////////////////////////////////////////////////////

        NativeArray<float3> positions = meshData.GetVertexData<float3>();
        positions[0] = 0.0f;
        positions[1] = right();
        positions[2] = up();
        positions[3] = float3(1.0f, 1.0f, 0.0f);

        NativeArray<float3> normals = meshData.GetVertexData<float3>(1);
        normals[0] = normals[1] = normals[2] = normals[3] = back();

        NativeArray<half4> tangents = meshData.GetVertexData<half4>(2);
        tangents[0] = tangents[1] = tangents[2] = tangents[3] = half4(h1, h0, h0, half(-1f));

        NativeArray<half2> texCoords = meshData.GetVertexData<half2>(3);
        texCoords[0] = h0;
        texCoords[1] = half2(h1, h0);
        texCoords[2] = half2(h0, h1);
        texCoords[3] = h1;

        NativeArray<ushort> triangleIndices = meshData.GetIndexData<ushort>();
        triangleIndices[0] = 0;
        triangleIndices[1] = 2;
        triangleIndices[2] = 1;
        triangleIndices[3] = 1;
        triangleIndices[4] = 2;
        triangleIndices[5] = 3;

        var bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));              // Manually set the bounds of the mesh

        meshData.subMeshCount = 1;                                                          // Set the number of sub meshes (The number of seperated meshes in a single larger mesh)
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount)                 // Specifies what part of the index buffer to use for the sub mesh
        {
            bounds = bounds,
            vertexCount = vertexCount
        }, MeshUpdateFlags.DontRecalculateBounds);

        /////////////////////////////////////////////////////////////////
        ///
        ///         Generate Mesh
        ///
        /////////////////////////////////////////////////////////////////

        var terrainMesh = new Mesh                                                          // Create a new mesh for the terrain
        {
            bounds = bounds,
            name = "Procedural Terrain Mesh"
        };

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, terrainMesh);                   // Apply the mesh data to the mesh

        GetComponent<MeshFilter>().mesh = terrainMesh;                                      // Assign the mesh to the MeshFilter 
    }
}
