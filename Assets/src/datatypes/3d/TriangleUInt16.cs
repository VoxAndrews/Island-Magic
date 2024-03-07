using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace DataType3D
{
    /// <summary>
    /// Represents the indicies of a triangle as ushort values to save on memory
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUInt16
    {
        /// <summary>
        /// The first indice of the triangle
        /// </summary>
        public ushort a;

        /// <summary>
        /// The second indice of the triangle
        /// </summary>
        public ushort b;

        /// <summary>
        /// The third indice of the triangle
        /// </summary>
        public ushort c;

        /// <summary>
        /// Implicitly converts an int3 to a TriangleUInt16 (Will convert the int3 to ushort values
        /// when the context of the operation is appropriate)
        /// </summary>
        /// <param name="t"></param>
        public static implicit operator TriangleUInt16(int3 t) => new TriangleUInt16
        {
            a = (ushort)t.x,
            b = (ushort)t.y,
            c = (ushort)t.z
        };
    }
}