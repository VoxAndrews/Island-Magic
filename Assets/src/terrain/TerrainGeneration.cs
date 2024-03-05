using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]                        // Requires a MeshFilter (Reference to a mesh) and MeshRenderer (Reference to the mesh renderer) to manipulate and generate the mesh
public class TerrainGeneration : MonoBehaviour
{
                                                                                    // Awake is called before Start when the instance of the script is being loaded
    private void Awake()
    {
        int vertexAttributeCount = 4;                                               // Number of vertex attributes (Position, Normal, Tangent, UV)
        int vertexCount = 4;                                                        // Number of vertices in the mesh

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);        // Allocate the mesh data to the mesh data array
        Mesh.MeshData meshData = meshDataArray[0];                                  // Get the mesh data from the mesh data array

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>           // Create an array of vertex attributes
        (
			vertexAttributeCount, Allocator.Temp
		);

        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);              // Set the vertex buffer parameters
        vertexAttributes.Dispose();                                                 // Dispose the vertex attributes

        var terrainMesh = new Mesh                                                  // Create a new mesh for the terrain
        {
            name = "Procedural Terrain Mesh"
        };

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, terrainMesh);           // Apply the mesh data to the mesh

        GetComponent<MeshFilter>().mesh = terrainMesh;                              // Assign the mesh to the MeshFilter 
    }
}
