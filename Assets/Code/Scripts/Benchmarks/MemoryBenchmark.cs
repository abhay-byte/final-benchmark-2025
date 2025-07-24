using UnityEngine;
using System.Diagnostics;
using Unity.Mathematics;

public class MemoryBenchmark : MonoBehaviour
{
    [SerializeField] private Benchmark benchmark;
    private BenchmarkLog benchmarkLog;

    private MemoryBenchmarkData benchmarkData;

    public int memoryBlockSizeMB = 1024; // Size of memory block to allocate in megabytes
    public int numIterations = 1;

    private float totalAllocationTime;
    private float totalDeAllocationTime;

    private byte[] memoryBlock;

    private void Awake()
    {
        Initialise();
        benchmarkLog.AddToBenchmarkLog("Memory Benchmark Began...\n");
    }

    private void Initialise()
    {
        benchmarkData = new MemoryBenchmarkData();
        GameObject benchmarkLogGameObject = transform.Find("/scripts/benchmark_log").gameObject;
        benchmarkLog = benchmarkLogGameObject.GetComponent<BenchmarkLog>();
    }

    public void BeginBenchmark()
    {
        memoryBlock = new byte[memoryBlockSizeMB * 1024 * 1024]; // Allocate memory block

        for (int i = 0; i < numIterations; i++)
        {
            BenchmarkRamSpeed();
        }

        // Deallocate memory block
        memoryBlock = null;
        System.GC.Collect();

        SetBenchmarkData();
        BeginNextBenchmark();  
    }

    private void BenchmarkRamSpeed()
    {
        // Measure memory allocation time
        Stopwatch allocationStopwatch = new Stopwatch();
        allocationStopwatch.Start();

        byte[] allocatedMemory = new byte[memoryBlockSizeMB * 1024 * 1024];

        allocationStopwatch.Stop();
        double allocationTimeMS = allocationStopwatch.Elapsed.TotalMilliseconds;

        // Measure memory deallocation time
        Stopwatch deallocationStopwatch = new Stopwatch();
        deallocationStopwatch.Start();

        allocatedMemory = null;
        System.GC.Collect();

        deallocationStopwatch.Stop();
        double deallocationTimeMS = deallocationStopwatch.Elapsed.TotalMilliseconds;

        // Display results
        UnityEngine.Debug.Log($"RAM Speed Test - Memory Block Size: {memoryBlockSizeMB} MB");
        UnityEngine.Debug.Log($"Memory Allocation Time: {allocationTimeMS} ms");
        UnityEngine.Debug.Log($"Memory Deallocation Time: {deallocationTimeMS} ms");

        benchmarkLog.AddToBenchmarkLog($"RAM Speed Test - Memory Block Size: {memoryBlockSizeMB} MB\n");
        benchmarkLog.AddToBenchmarkLog($"Memory Allocation Time: {allocationTimeMS} ms\n");
        benchmarkLog.AddToBenchmarkLog($"Memory Deallocation Time: {deallocationTimeMS} ms\n");
    }

    private void SetBenchmarkData()
    {
        benchmark.SetStorageMemoryData(benchmarkData);
    }

    private void BeginNextBenchmark()
    {
        benchmark.SetBenchmark();
    }

}

public class MemoryBenchmarkData
{
    public float memoryAllocationTime;
    public float memoryDeallocationTime;
}
