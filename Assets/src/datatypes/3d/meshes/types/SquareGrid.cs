using System.Drawing;
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
        /// The resolution of the square grid
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// The number of vertices in the grid mesh
        /// </summary>
        public int VertexCount => 4 * (Resolution * Resolution);

        /// <summary>
        /// The number of indices in the grid mesh
        /// </summary>
        public int IndexCount => 6 * (Resolution * Resolution);

        /// <summary>
        /// The length of the job needed to generate the grid
        /// </summary>
        public int JobLength => Resolution;

        /// <summary>
        /// Executes the job for the grid
        /// </summary>
        /// <param name="z">The z-offset of the quad row</param>
        /// <param name="streams">The streams needed for the job</param>
        public void Execute<S>(int z, S streams) where S : struct, IMeshStreams 
        {
            int vi = 4 * Resolution * z, ti = 2 * Resolution * z;                                   // Determines the correct vertices and indices

            for (int x = 0; x < Resolution; x++, vi += 4, ti += 2)                                  // Loops through the grid to generate rows of quads with an x-offset (Increment Vertex Index offset by 4 and Triangle Index offset by 2)
            {
                var xCoordinates = float2(x, x + 1f) / Resolution - 0.5f;                           // Calculating the coordinates of the quads
                var zCoordinates = float2(z, z + 1f) / Resolution - 0.5f;

                var vertex = new Vertex();                                                          // Setting test vertices

                vertex.normal.y = 1.0f;                                                             // Setting vertices and adding them to the stream
                vertex.tangent.xw = float2(1.0f, -1.0f);                                            // (Adjusting using the calculated indices & coordinates)
                vertex.position.x = xCoordinates.x;
                vertex.position.z = zCoordinates.x;
                streams.SetVertex(vi + 0, vertex);

                vertex.position.x = xCoordinates.y;
                vertex.texCoord0 = float2(1.0f, 0.0f);
                streams.SetVertex(vi + 1, vertex);

                vertex.position.x = xCoordinates.x;
                vertex.position.z = zCoordinates.y;
                vertex.texCoord0 = float2(0.0f, 1.0f);
                streams.SetVertex(vi + 2, vertex);

                vertex.position.x = xCoordinates.y;
                vertex.texCoord0 = 1.0f;
                streams.SetVertex(vi + 3, vertex);

                streams.SetTriangle(ti + 0, vi + int3(0, 2, 1));
                streams.SetTriangle(ti + 1, vi + int3(1, 2, 3));
            }
        }

        public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(1f, 0f, 1f));                  // The implmentation of Bounds method
    }
}
