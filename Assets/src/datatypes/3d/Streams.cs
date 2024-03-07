using System.IO;
using System.Runtime.CompilerServices;                                                              // Used for [MethodImpl(MethodImplOptions.AggressiveInlining)]  
using System.Runtime.InteropServices;                                                               // Used for [StructLayout(LayoutKind.Sequential)]
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;                                                            // Used for [NativeDisableContainerSafetyRestriction]
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace DataType3D.Streams                                                                        // Create a streams-specific namespace for DataType3D
{
    [StructLayout(LayoutKind.Sequential)]                                                           // To lay out the data correctly for Single-stream buffers
    struct Stream                                                                                   // Create a struct to store the data in a stream buffer
    {
        public float3 position;

        public float3 normal;

        public float4 tangent;

        public float2 texCoord0;
    }

    public struct DataStream : IMeshStreams
    {
        [NativeDisableContainerSafetyRestriction]                                                   // Disable safety restrictions for the container to allow for compilation (In case th compiler complains that the same container is used multiple times)
        NativeArray<Stream> singleStream;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<TriangleUInt16> triangles;

        [NativeDisableContainerSafetyRestriction]                                                   // Streams 0-3 used for multi-stream buffers
        NativeArray<float3> stream0, stream1;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<float4> stream2;

        [NativeDisableContainerSafetyRestriction]
        NativeArray<float2> stream3;

        StreamType inputStreamType;                                                                 // Grabs the input stream type from the Setup function

        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount, StreamType type)
        {
            inputStreamType = type;

            var descriptor = new NativeArray<VertexAttributeDescriptor>                             // Create an array of vertex attributes
            (
                4, Allocator.Temp                                                                   // Can optionally choose to unitilize the array and allow garbage data with 'NativeArrayOptions.UninitializedMemory'
            );

            descriptor[0] = new VertexAttributeDescriptor
            (
                VertexAttribute.Position, dimension: 3
            );

            if (type == StreamType.Single)
            {
                Debug.Log("SINGLE: Setting stream attributes");

                descriptor[1] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Normal, dimension: 3
                );
                descriptor[2] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Tangent, dimension: 4
                );
                descriptor[3] = new VertexAttributeDescriptor
                (
                    VertexAttribute.TexCoord0, dimension: 2
                );
            }
            else if (type == StreamType.Multi)
            {
                Debug.Log("MULTI: Setting stream attributes");

                descriptor[1] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Normal, dimension: 3, stream: 1
                );
                descriptor[2] = new VertexAttributeDescriptor
                (
                    VertexAttribute.Tangent, dimension: 4, stream: 2
                );
                descriptor[3] = new VertexAttributeDescriptor
                (
                    VertexAttribute.TexCoord0, dimension: 2, stream: 3
                );
            }
            else
            {
                Debug.LogError("Invalid stream type, cannot set stream attributes. Please enter 'Single' or 'Multi'!");
            }

            meshData.SetVertexBufferParams(vertexCount, descriptor);                                // Set the vertex buffer parameters (Reserves memory for the vertex attributes buffer)
            descriptor.Dispose();                                                                   // Dispose the vertex attributes

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);                          // Set the index buffer parameters (Reserves memory for the vertex index buffer)

            meshData.subMeshCount = 1;                                                              // Set the number of sub meshes (The number of seperated meshes in a single larger mesh)
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)                             // Specifies what part of the index buffer to use for the sub mesh
                {
                    bounds = bounds,
                    vertexCount = vertexCount
                },

                MeshUpdateFlags.DontRecalculateBounds |                                             // Don't recalculate the bounds or validate the indices (Job not done yet)
                MeshUpdateFlags.DontValidateIndices
            );                           

            if (type == StreamType.Single)
            {
                Debug.Log("SINGLE: Getting the Vertex Data for the stream");

                singleStream = meshData.GetVertexData<Stream>();                                    // Get the vertex data for the single-stream buffer
            }
            else if (type == StreamType.Multi)
            {
                Debug.Log("MULTI: Getting the Vertex Data for the streams");

                stream0 = meshData.GetVertexData<float3>();                                         // Get the vertex data for the multi-stream buffer
                stream1 = meshData.GetVertexData<float3>(1);
                stream2 = meshData.GetVertexData<float4>(2);
                stream3 = meshData.GetVertexData<float2>(3);
            }
            else
            {
                Debug.LogError("Invalid stream type, cannot set stream data. Please enter 'Single' or 'Multi'!");
            }

            triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);             // Get the indicies for a triangle and convert them to an int3
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]                                          // Optimize the 'SetVertex' function by inlineing it and making sure it is included more than once
        public void SetVertex(int index, Vertex vertex)
        {
            if (inputStreamType == StreamType.Single)                                               // Checks to see if the stream type is single or multi
            {
                Debug.Log("SINGLE: Setting the vertex data for the streams - INDEX: " + index.ToString());

                singleStream[index] = new Stream
                {
                    position = vertex.position,
                    normal = vertex.normal,
                    tangent = vertex.tangent,
                    texCoord0 = vertex.texCoord0
                };
            }
            else if (inputStreamType == StreamType.Multi)
            {
                Debug.Log("MULTI: Setting the vertex data for the streams - INDEX: " + index.ToString());

                stream0[index] = vertex.position;
                stream1[index] = vertex.normal;
                stream2[index] = vertex.tangent;
                stream3[index] = vertex.texCoord0;
            }
            else
            {
                Debug.LogError("Invalid stream type, cannot set stream vertex data. Please enter 'Single' or 'Multi'!");
            }
        }

        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;           // Set the triangles for the multistream buffer
    }
}