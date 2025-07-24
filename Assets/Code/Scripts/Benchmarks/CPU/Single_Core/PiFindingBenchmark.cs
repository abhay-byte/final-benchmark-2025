using UnityEngine;
using System.Diagnostics;

public class PiFindingBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public int totalPoints = 100000000; // Number of random points to generate
    private int pointsInsideCircle = 0;
    private Stopwatch stopwatch = new Stopwatch();


    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginPiBenchmark()
    {
        UnityEngine.Debug.Log($"Estimating Pi with {totalPoints} points...");

        stopwatch.Start();
        EstimatePi();
        stopwatch.Stop();

        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        UnityEngine.Debug.Log($"Estimated Pi: {EstimatePiValue()}");
        UnityEngine.Debug.Log($"Execution Time: {elapsedTime} ms");

        SetPiBenchmarkResult(elapsedTime/1000);
        cpuBenchmark.BeginBenchamrk();
    }

    private void EstimatePi()
    {
        for (int i = 0; i < totalPoints; i++)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);

            if (x * x + y * y <= 1)
            {
                pointsInsideCircle++;
            }
        }
    }

    private double EstimatePiValue()
    {
        return 4.0 * pointsInsideCircle / totalPoints;
    }

    private void SetPiBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.piFindingTIme = totalTimeElapsed;
    }
}
