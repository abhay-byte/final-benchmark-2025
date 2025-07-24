using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    [SerializeField] private CPUBenchmark cpuBenchmark;
    public CPUBenchmark GetCPUBenchmarkScript()
    {
        return cpuBenchmark;
    }
}
