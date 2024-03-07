using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

namespace DataType3D.Generators                                                                     // A meshes-specific namespace for DataType3D
{
    /// <summary>
    /// Generates a square grid
    /// </summary>
    public struct SquareGrid : IMeshGenerator
    {
        /// <summary>
        /// The number of vertices in the grid mesh
        /// </summary>
        public int VertexCount => 4;

        /// <summary>
        /// The number of indices in the grid mesh
        /// </summary>
        public int IndexCount => 6;

        /// <summary>
        /// The length of the job needed to generate the grid
        /// </summary>
        public int JobLength => 1;

        /// <summary>
        /// Executes the job for the grid
        /// </summary>
        /// <param name="i">The index of the job</param>
        /// <param name="streams">The streams needed for the job</param>
        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams 
        {
            // Setting test vertices
            var vertex = new Vertex();

            // Setting vertices and adding them to the stream
            vertex.normal.z = -1.0f;
            vertex.tangent.xw = float2(1.0f, -1.0f);
            streams.SetVertex(0, vertex);

            vertex.position = right();
            vertex.texCoord0 = float2(1.0f, 0.0f);
            streams.SetVertex(1, vertex);

            vertex.position = up();
            vertex.texCoord0 = float2(0.0f, 1.0f);
            streams.SetVertex(2, vertex);

            vertex.position = float3(1.0f, 1.0f, 0.0f);
            vertex.texCoord0 = 1.0f;
            streams.SetVertex(3, vertex);

            streams.SetTriangle(0, int3(0, 2, 1));
            streams.SetTriangle(1, int3(1, 2, 3));
        }
    }
}
