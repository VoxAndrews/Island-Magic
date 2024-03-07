using Unity.Mathematics;

namespace DataType3D                                                                                // Create a namespace for 3D data types
{
    /// <summary>
    /// Vertex structure (Similar to how 'appdata_tan' is defined inn the Unity Documentation)
    /// </summary>
    public struct Vertex
    {
        /// <summary>
        /// The position of the vertex
        /// </summary>
        public float3 position;

        /// <summary>
        /// The normal of the vertex
        /// </summary>
        public float3 normal;

        /// <summary>
        /// The tangent of the vertex
        /// </summary>
        public float4 tangent;

        /// <summary>
        /// The texture coordinate of the vertex
        /// </summary>
        public float2 texCoord0;
    }
}