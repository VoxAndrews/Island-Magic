using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

namespace DataType3D.Meshes                                                                         // A meshes-specific namespace for DataType3D
{
    public struct SquareGrid : IMeshGenerator 
    {
        public uint VertexCount => 0;

        public uint IndexCount => 0;

        public uint JobLength => 0;

        public void Execute<S>(uint i, S streams) where S : struct, IMeshStreams { }
    }
}
