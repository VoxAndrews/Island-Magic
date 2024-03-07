using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataType3D
{
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
        int VertexCount { get; }

        /// <summary>
        /// The amount of indices in the generated mesh
        /// </summary>
        /// <returns>The amount of indices in the mesh</returns>
        int IndexCount { get; }

        /// <summary>
        /// The length of the mesh generation job
        /// </summary>
        /// <returns>The length of the job</returns>
        int JobLength { get; }
    }
}
