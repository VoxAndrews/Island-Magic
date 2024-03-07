using Unity.Mathematics;
using UnityEngine;

namespace DataType3D
{                                                                                           // An interface for the mesh streams to set up vertex & index buffers (Declarations but no implementation)
    public enum StreamType
    {
        Single,
        Multi
    }

    public interface IMeshStreams 
    {
        void Setup(Mesh.MeshData data, int vertexCount, int indexCount, StreamType type);   // Initilises the buffers

        void SetVertex(int index, Vertex data);                                             // Copy a vertex into the buffers

        void SetTriangle(int index, int3 triangle);                                         // Copy a set of indices into the buffers relevant to a triangle
    }                    
}