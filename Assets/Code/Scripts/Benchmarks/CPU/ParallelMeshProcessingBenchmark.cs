using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
[RequireComponent(typeof(Mesh))]
public class ParallelMeshProcessingBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public int numVertices = 10000;   // Number of vertices in the mesh
    public int numIterations = 5;     // Number of iterations for benchmarking

    private Mesh mesh;
    private Vector3[] vertices;
    private double totalTimeElapsed = 0;

    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginMeshProcessingBenchmark()
    {
        totalTimeElapsed = 0;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[numVertices];

        for (int i = 1; i <= numIterations; i++)
        {
            StartCoroutine(BenchmarkParallelMeshProcessing(i));
        }
    }

    private IEnumerator BenchmarkParallelMeshProcessing(int val)
    {
        // Initialize mesh vertices with random positions
        for (int i = 0; i < numVertices; i++)
        {
            vertices[i] = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        }

        mesh.vertices = vertices; // Apply initial vertex positions to the mesh

        int numThreads = SystemInfo.processorCount; // Get the number of available processors
        int verticesPerThread = numVertices / numThreads;
        Task[] tasks = new Task[numThreads];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start(); // Start measuring time

        for (int i = 0; i < numThreads; i++)
        {
            int startIndex = i * verticesPerThread;
            int endIndex = i == numThreads - 1 ? numVertices : startIndex + verticesPerThread;

            tasks[i] = Task.Factory.StartNew(() => ProcessVertices(startIndex, endIndex));
        }

        stopwatch.Stop(); // Stop measuring time

        // Schedule mesh update on the main thread
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        UnityEngine.Debug.Log($"Parallel Mesh Processing Benchmark - Vertices: {numVertices}");
        UnityEngine.Debug.Log($"Time Took: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        totalTimeElapsed += stopwatch.Elapsed.TotalMilliseconds;

        SetMeshProccesingBenchmarkResult(stopwatch.Elapsed.TotalMilliseconds);
        cpuBenchmark.BeginBenchamrk();
        yield return null;

    }

    private void ProcessVertices(int startIndex, int endIndex)
    {
        // Simulate a complex calculation on mesh vertices (e.g., deformation, manipulation, etc.)
        for (int i = startIndex; i < endIndex; i++)
        {
            // Example: Deform the vertices by scaling them
            vertices[i] *= UnityEngine.Random.Range(0.8f, 1.2f);
        }
    }

    private void SetMeshProccesingBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.meshProcessingTime = totalTimeElapsed/1000;
    }
}
