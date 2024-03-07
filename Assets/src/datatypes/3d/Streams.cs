using System.Runtime.CompilerServices;                                                              // Used for [MethodImpl(MethodImplOptions.AggressiveInlining)]  
using System.Runtime.InteropServices;                                                               // Used for [StructLayout(LayoutKind.Sequential)]
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace DataType3D                                                                       // Create a streams-specific namespace for DataType3D
{
    [StructLayout(LayoutKind.Sequential)]                                                           // Laid out sequentially so that it is placed into memory in the same order as the 'appdata_tan' struct
    struct Stream                                                                                   // Create a struct to store the data in a stream buffer
    {
        public float3 position;

        public float4 tangent;

        public float3 normal;

        public float2 texCoord0;
    }

    public struct DataStream : IMeshStreams
    {
        NativeArray<Stream> stream;
        NativeArray<int3> triangles;

        public void Setup(Mesh.MeshData meshData, int vertexCount, int indexCount, StreamType type)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>                             // Create an array of vertex attributes
            (
                4, Allocator.Temp                                                                   // Can optionally choose to unitilize the array and allow garbage data with 'NativeArrayOptions.UninitializedMemory'
            );

            if (type == StreamType.Single)
            {
                descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
                descriptor[1] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Tangent, dimension: 4
                );
                descriptor[2] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Normal, dimension: 3
                );
                descriptor[3] = new VertexAttributeDescriptor
                (
                    VertexAttribute.TexCoord0, dimension: 2
                );
            }
            else if (type == StreamType.Multi)
            {

                descriptor[0] = new VertexAttributeDescriptor(dimension: 3);
                descriptor[1] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Tangent, dimension: 4, stream: 1
                );
                descriptor[2] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Normal, dimension: 3, stream: 2
                );
                descriptor[3] = new VertexAttributeDescriptor
                (
                    VertexAttribute.TexCoord0, dimension: 2, stream: 3
                );
            }
            else
            {
                Debug.LogError("Invalid stream type. Please enter 'Single' or 'Multi'!");
            }

            meshData.SetVertexBufferParams(vertexCount, descriptor);                                // Set the vertex buffer parameters (Reserves memory for the vertex attributes buffer)
            descriptor.Dispose();                                                                   // Dispose the vertex attributes

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);                          // Set the index buffer parameters (Reserves memory for the vertex index buffer)

            meshData.subMeshCount = 1;                                                              // Set the number of sub meshes (The number of seperated meshes in a single larger mesh)
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));                           // Specifies what part of the index buffer to use for the sub mesh

            stream = meshData.GetVertexData<Stream>();                                              // Get the vertex data for the multistream buffer
            triangles = meshData.GetIndexData<int>().Reinterpret<int3>(4);                          // Get the indicies for a triangle and convert them to an int3
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]                                          // Optimize the 'SetVertex' function by inlineing it and making sure it is included more than once
        public void SetVertex(int index, Vertex vertex) => stream[index] = new Stream               // Set the vertexes for the multistream buffer
        {
            position = vertex.position,
            tangent = vertex.tangent,
            normal = vertex.normal,
            texCoord0 = vertex.texCoord0
        };

        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;           // Set the triangles for the multistream buffer
    }
}