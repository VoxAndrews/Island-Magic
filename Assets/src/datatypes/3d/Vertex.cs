using Unity.Mathematics;                                            // Included to use float vectors

namespace DataType3D
{
    public struct Vertex                                            // Vertex structure
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
}