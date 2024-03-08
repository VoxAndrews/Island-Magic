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
                var xCoordinates = float2(x, x + 1.0f) / Resolution - 0.5f;                         // Calculating the coordinates of the quads
                var zCoordinates = float2(z, z + 1.0f) / Resolution - 0.5f;

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

        public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(1.0f, 0.0f, 1.0f));            // The implmentation of Bounds method
    }

    /// <summary>
    /// Generates a square grid with shared vertices
    /// </summary>
    public struct SharedSquareGrid : IMeshGenerator
    {
        /// <summary>
        /// The resolution of the square grid
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// The number of vertices in the grid mesh
        /// </summary>
        public int VertexCount => (Resolution + 1) * (Resolution + 1);

        /// <summary>
        /// The number of indices in the grid mesh
        /// </summary>
        public int IndexCount => 6 * (Resolution * Resolution);

        /// <summary>
        /// The length of the job needed to generate the grid
        /// </summary>
        public int JobLength => Resolution + 1;

        /// <summary>
        /// Executes the job for the grid
        /// </summary>
        /// <param name="z">The z-offset of the quad row</param>
        /// <param name="streams">The streams needed for the job</param>
        public void Execute<S>(int z, S streams) where S : struct, IMeshStreams
        {
            int vi = (Resolution + 1) * z, ti = 2 * Resolution * (z - 1);                           // Determine the index of the first vertex of the row

            var vertex = new Vertex();                                                              // Initialise a vertex with a constant normal and tangent and
            vertex.normal.y = 1.0f;                                                                 // make the UV continous across the entire grid
            vertex.tangent.xw = float2(1.0f, -1.0f);

            vertex.position.x = -0.5f;
            vertex.position.z = (float)z / Resolution - 0.5f;
            vertex.texCoord0.y = (float)z / Resolution;
            streams.SetVertex(vi, vertex);
            vi += 1;

            for (int x = 1; x <= Resolution; x++, vi++, ti += 2)                                    // Increment the vertex index and loop over all other vertices
            {                                                                                       // of the row
                vertex.position.x = (float)x / Resolution - 0.5f;
                vertex.texCoord0.x = (float)x / Resolution;
                streams.SetVertex(vi, vertex);

                if (z > 0)                                                                          // Set the triangles (Two at a time) only when z > 0 as there are
                {                                                                                   // no quads below it
                    streams.SetTriangle
                    (
                        ti + 0, vi + int3(-Resolution - 2, -1, -Resolution - 1)
                    );

                    streams.SetTriangle
                    (
                        ti + 1, vi + int3(-Resolution - 1, -1, 0)
                    );
                }
            }
        }

        public Bounds Bounds => new Bounds(Vector3.zero, new Vector3(1.0f, 0.0f, 1.0f));            // The implmentation of Bounds method
    }
}
