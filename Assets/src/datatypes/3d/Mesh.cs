using Unity.Mathematics;
using UnityEngine;

namespace DataType3D
{                                                                           // An interface for the mesh streams to set up vertex & index buffers (Declarations but no implementation)
    public interface IMeshStreams 
    {
        void Setup(Mesh.MeshData data, int vertexCount, int indexCount);    // Initilises the buffers

        void SetVertex(int index, Vertex data);                             // Copy a vertex into the buffers

        void SetTriangle(int index, int3 triangle);                         // Copy a set of indices into the buffers relevant to a triangle
    }                    
}