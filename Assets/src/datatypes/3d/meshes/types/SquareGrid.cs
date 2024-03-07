using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

namespace DataType3D                                                                        // A meshes-specific namespace for DataType3D
{
    public struct SquareGrid : IMeshGenerator
    {
        public int VertexCount => 0;

        public int IndexCount => 0;

        public int JobLength => 0;

        public void Execute<S>(int i, S streams) where S : struct, IMeshStreams { }
    }
}
