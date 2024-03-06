using DataType3D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace DataType3D.Streams
{

    public struct MultiStream : IMeshStreams 
    {
        public void Setup(Mesh.MeshData meshData, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>                         // Create an array of vertex attributes
            (
                4, Allocator.Temp                                                               // Can optionally choose to unitilize the array and allow garbage data with 'NativeArrayOptions.UninitializedMemory'
            );

            descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor
            (
                VertexAttribute.Normal, dimension: 3, stream: 1
            );
            descriptor[2] = new VertexAttributeDescriptor
            (
                VertexAttribute.Tangent, VertexAttributeFormat.Float16, 4, 2
            );
            descriptor[3] = new VertexAttributeDescriptor
            (
                VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2, 3
            );

            meshData.SetVertexBufferParams(vertexCount, descriptor);                            // Set the vertex buffer parameters (Reserves memory for the vertex attributes buffer)
            descriptor.Dispose();                                                               // Dispose the vertex attributes

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);                      // Set the index buffer parameters (Reserves memory for the vertex index buffer)

            meshData.subMeshCount = 1;                                                          // Set the number of sub meshes (The number of seperated meshes in a single larger mesh)
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));                       // Specifies what part of the index buffer to use for the sub mesh
        }
    }
}