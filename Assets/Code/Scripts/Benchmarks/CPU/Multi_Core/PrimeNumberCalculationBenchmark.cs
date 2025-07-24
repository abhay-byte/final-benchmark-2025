using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

public class PrimeNumberCalculationBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public int rangeStart = 1;
    public int rangeEnd = 10000000;
    public int numIterations = 100; // Number of iterations for benchmarking
    private double totalTimeElapsed = 0;

    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginPrimeNumberBenchmark()
    {
        totalTimeElapsed = 0;
        for (int i = 0; i < numIterations; i++)
        {
            BenchmarkPrimeCalculation();
        }
        UnityEngine.Debug.Log($"Total Time Took: {totalTimeElapsed}");

        SetPrimeNumberBenchmarkResult(totalTimeElapsed/1000);
        cpuBenchmark.BeginBenchamrk();
    }

    private void BenchmarkPrimeCalculation()
    {
        List<int> primeNumbers = new List<int>();

        int threadCount = Environment.ProcessorCount;
        int numbersPerThread = (rangeEnd - rangeStart) / threadCount;
        Task[] tasks = new Task[threadCount];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start(); // Start measuring time

        for (int i = 0; i < threadCount; i++)
        {
            int start = rangeStart + i * numbersPerThread;
            int end = i == threadCount - 1 ? rangeEnd : start + numbersPerThread;
            tasks[i] = Task.Factory.StartNew(() => CalculatePrimes(start, end, primeNumbers));
        }

        Task.WaitAll(tasks);

        stopwatch.Stop(); // Stop measuring time

        int totalPrimes = primeNumbers.Count;

        UnityEngine.Debug.Log($"Prime Number Calculation Benchmark - Range: {rangeStart}-{rangeEnd}");
        UnityEngine.Debug.Log($"Total Prime Numbers Found: {totalPrimes}");
        UnityEngine.Debug.Log($"Time Took: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        totalTimeElapsed += stopwatch.Elapsed.TotalMilliseconds;

    }

    private void CalculatePrimes(int start, int end, List<int> primeNumbers)
    {
        for (int i = start; i < end; i++)
        {
            if (IsPrime(i))
            {
                lock (primeNumbers) // Ensure thread-safe access to the shared list
                {
                    primeNumbers.Add(i);
                }
            }
        }
    }

    private bool IsPrime(int n)
    {
        if (n <= 1)
            return false;
        if (n <= 3)
            return true;
        if (n % 2 == 0 || n % 3 == 0)
            return false;

        for (int i = 5; i * i <= n; i += 6)
        {
            if (n % i == 0 || n % (i + 2) == 0)
                return false;
        }
        return true;
    }
    private void SetPrimeNumberBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.primeNumberCalculationTime = totalTimeElapsed;
    }
}
