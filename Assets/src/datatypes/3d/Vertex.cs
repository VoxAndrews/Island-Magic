using Unity.Mathematics;                                            // Included to use float vectors

namespace DataType3D
{
    public struct Vertex                                            // Vertex structure (Similar to how 'appdata_tan' is defined)
    {
        public float3 position;

        public float4 tangent;

        public float3 normal;

        public float2 texCoord0;
    }
}