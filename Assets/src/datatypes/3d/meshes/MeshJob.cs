using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DataType3D
{
    /// <summary>
    /// Represents a job for mesh generation (Burst compilation and compile synchronously
    /// to save time and memory)
    /// </summary>
    /// <typeparam name="G">The type of the mesh generator used by the job.</typeparam>
    /// <typeparam name="S">The type of the mesh streams used by the job.</typeparam>
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<G, S> : IJobFor
        where G : struct, IMeshGenerator
        where S : struct, IMeshStreams
    {
        /// <summary>
        /// The mesh generator object used by the job.
        /// </summary>
        G generator;

        /// <summary>
        /// The mesh streams object used by the job (Write only)
        /// </summary>
        [WriteOnly]
        S streams;

        /// <summary>
        /// Executes the job
        /// </summary>
        /// <param name="i">The index of the job</param>
        /// <param name="streams">The streams needed for the job</param>
        public void Execute(int i) => generator.Execute(i, streams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meshData">The data to generate the mesh</param>
        /// <param name="dependency">The dependency for the job</param>
        /// <param name="type">The type of the streams</param>
        /// <returns>The job handle (The unique identifier for the job)</returns>
        public static JobHandle ScheduleParallel(Mesh.MeshData meshData, JobHandle dependency, StreamType type)
        {
            var job = new MeshJob<G, S>();

            job.streams.Setup(meshData, job.generator.VertexCount, job.generator.IndexCount, type);

            return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
        }
    }
}
