using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DataType3D
{
    /// <summary>
    /// Represents a job for mesh generation.
    /// </summary>
    /// <typeparam name="G">The type of the mesh generator used by the job.</typeparam>
    /// <typeparam name="S">The type of the mesh streams used by the job.</typeparam>
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]            // Enable burst compilation and compile synchronously (Saves time and memory)
    public struct MeshJob<G, S> : IJobFor
        where G : struct, IMeshGenerator
        where S : struct, IMeshStreams
    {
        G generator;

        [WriteOnly]                                                                                 // Write only (Only writing at this point, don't need to read)
        S streams;

        public void Execute(int i) => generator.Execute(i, streams);

        public static JobHandle ScheduleParallel(Mesh.MeshData meshData, JobHandle dependency, StreamType type)
        {
            var job = new MeshJob<G, S>();

            job.streams.Setup(meshData, job.generator.VertexCount, job.generator.IndexCount, type);

            return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
        }
    }
}
