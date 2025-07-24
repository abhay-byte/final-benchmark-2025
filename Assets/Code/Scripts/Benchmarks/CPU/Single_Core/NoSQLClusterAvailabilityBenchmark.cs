using UnityEngine;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class NoSQLClusterAvailabilityBenchmark : MonoBehaviour
{
    public int simulationCount = 100000000;   // Number of simulations
    public int clusterNodes = 500;         // Number of nodes in the cluster
    public float nodeFailureRate = 0.05f; // Probability of a node failure (5% failure rate)
    public float networkFailureRate = 0.1f; // Probability of network failure (10% failure rate)

    private Stopwatch stopwatch = new Stopwatch();
    private CPUBenchmark cpuBenchmark;

    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginSimulationBenchmark()
    {
        UnityEngine.Debug.Log($"Running {simulationCount} simulations...");

        stopwatch.Start();

        for (int i = 0; i < simulationCount; i++)
        {
            SimulateClusterAvailability();
        }

        stopwatch.Stop();

        TimeSpan elapsedTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Total Execution Time: {elapsedTime.TotalSeconds:F2} seconds");

        SetSimulationBenchmarkResult(elapsedTime.TotalSeconds);
        cpuBenchmark.BeginBenchamrk();
    }

    private void SimulateClusterAvailability()
    {
        for (int node = 0; node < clusterNodes; node++)
        {
            if (Random.Range(0f, 1f) < nodeFailureRate)
            {
                // Node failure occurred
                return;
            }

            if (Random.Range(0f, 1f) < networkFailureRate)
            {
                // Network failure occurred
                return;
            }
        }
    }

    private void SetSimulationBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.simulationTime = totalTimeElapsed;
    }
}
