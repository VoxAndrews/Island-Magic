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
        public int VertexCount => 0;

        /// <summary>
        /// The number of triangles in the grid mesh
        /// </summary>
        public int IndexCount => 0;

        /// <summary>
        /// The length of the job needed to generate the grid
        /// </summary>
        public int JobLength => 0;

        /// <summary>
        /// Executes the job for the grid
        /// </summary>
        /// <param name="i">The index of the job</param>
        /// <param name="streams">The streams needed for the job</param>
        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams { }
    }
}
