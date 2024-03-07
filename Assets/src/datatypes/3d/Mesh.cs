using Unity.Mathematics;
using UnityEngine;

namespace DataType3D.Meshes                                                                         // Create a meshes-specific namespace for DataType3D
{
    /// <summary>
    /// Represents the type of stream to be used for the mesh. Single-stream is 
    /// better for less detailed meshes that do not require multiple streams for 
    /// parallel processing and uses less memory. Multi-stream is better for more
    /// detailed meshes that require multiple streams for parallel processing and 
    /// uses more memory to process the data.
    /// </summary>
    public enum StreamType
    {
        /// <summary>
        /// Single-stream storage of mesh data (PTNXPTNXPTNXPTNX)
        /// </summary>
        Single,

        /// <summary>
        /// Multi-stream storage of mesh data (PPPPTTTTNNNNXXXX)
        /// </summary>
        Multi
    }

    /// <summary>
    /// An interface for the mesh streams to set up vertex & index buffers
    /// </summary>
    public interface IMeshStreams
    {
        /// <summary>
        /// A method to initilise the stream buffers
        /// </summary>
        /// <param name="data">The data of the mesh</param>
        /// <param name="vertexCount">The amount of vertexes in the mesh</param>
        /// <param name="indexCount">The amount of indices in the mesh.</param>
        /// <param name="type">The format the data is stored in</param>
        void Setup(Mesh.MeshData data, int vertexCount, int indexCount, StreamType type);

        /// <summary>
        /// A method to set the vertex data
        /// </summary>
        /// <param name="index">The index of the vertex</param>
        /// <param name="data">The vertex data</param>
        void SetVertex(int index, Vertex data);

        /// <summary>
        /// A method to set the triangle data
        /// </summary>
        /// <param name="index">The index of the triangle</param>
        /// <param name="data">The triangle indices</param>
        void SetTriangle(int index, int3 triangle);
    }

    /// <summary>
    /// An interface to generate a mesh from the data streams provided
    /// </summary>
    public interface IMeshGenerator
    {
        /// <summary>
        /// A method to execute the mesh generation code
        /// </summary>
        /// <param name="i">An index parameter (Implementation specific)</param>
        /// <param name="streams">The streams used for storage of the data</param>
        void Execute<S>(int i, S streams) where S : struct, IMeshStreams;

        /// <summary>
        /// The amount of vertexes in the generated mesh
        /// </summary>
        /// <returns>The amount of vertexes in the mesh</returns>
        uint VertexCount { get; }

        /// <summary>
        /// The amount of indices in the generated mesh
        /// </summary>
        /// <returns>The amount of indices in the mesh</returns>
        uint IndexCount { get; }

        /// <summary>
        /// The length of the mesh generation job
        /// </summary>
        /// <returns>The length of the job</returns>
        uint JobLength { get; }
    }
}