using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchmarkScripts : MonoBehaviour
{
    [Header("Benchmark List")]

    [Header("CPU")]
    public CPUBenchmark cpuBenchmark;

    [Header("STORAGE")]
    public StorageBenchmark storageBenchmark;

    [Header("MEMORY")]
    public MemoryBenchmark memoryBenchmark;

}
