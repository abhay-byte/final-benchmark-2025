using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

public class ParallelSortingBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public int arraySize = 1000000;  // Size of the array to be sorted
    public int numIterations = 10;
    private double totalTimeElapsed = 0;// Number of iterations for benchmarking


    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginSortingBenchmark()
    {
        for (int i = 0; i < numIterations; i++)
        {
            BenchmarkParallelSorting();
        }
        UnityEngine.Debug.Log($"Total Time Took: {totalTimeElapsed}");

        SetSortingBenchmarkResult(totalTimeElapsed/1000);

        cpuBenchmark.BeginBenchamrk();
    }

    private void BenchmarkParallelSorting()
    {
        // Generate a random array to be sorted
        int[] numbersToSort = GenerateRandomArray(arraySize);

        int threadCount = SystemInfo.processorCount;
        int elementsPerThread = arraySize / threadCount;

        Task[] tasks = new Task[threadCount];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start(); // Start measuring time

        for (int i = 0; i < threadCount; i++)
        {
            int startIndex = i * elementsPerThread;
            int endIndex = i == threadCount - 1 ? arraySize - 1 : startIndex + elementsPerThread - 1;

            tasks[i] = Task.Factory.StartNew(() => ParallelQuickSort(numbersToSort, startIndex, endIndex));
        }

        Task.WaitAll(tasks);

        stopwatch.Stop(); // Stop measuring time

        // Optionally, you can use the sorted array for further processing or validation

        UnityEngine.Debug.Log($"Parallel Sorting Benchmark - Array Size: {arraySize}");
        UnityEngine.Debug.Log($"Time Taken: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        UnityEngine.Debug.Log("Parallel sorting completed.");
        totalTimeElapsed += stopwatch.Elapsed.TotalMilliseconds;
}

    private int[] GenerateRandomArray(int size)
    {
        int[] array = new int[size];
        System.Random random = new System.Random();

        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next();
        }

        return array;
    }

    private void ParallelQuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right);
            Parallel.Invoke(
                () => ParallelQuickSort(arr, left, pivotIndex - 1),
                () => ParallelQuickSort(arr, pivotIndex + 1, right)
            );
        }
    }

    private int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        int swap = arr[i + 1];
        arr[i + 1] = arr[right];
        arr[right] = swap;

        return i + 1;
    }

    private void SetSortingBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.sortingTIme = totalTimeElapsed;
    }

}
