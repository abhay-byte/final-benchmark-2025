using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine.UI;

[RequireComponent(typeof(Texture2D))]
public class ParallelImageProcessingBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public Texture2D inputImage;
    public int numIterations = 250; // Number of iterations for benchmarking
    private double totalTimeElapsed = 0;

    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginImageProcessingBenchmark()
    {
        totalTimeElapsed = 0;
        for (int i = 0; i < numIterations; i++)
        {
            totalTimeElapsed += BenchmarkParallelImageProcessing();
        }

        UnityEngine.Debug.Log($"Total Time Took: {totalTimeElapsed}");
        SetCryptographicBenchmarkResult();
        cpuBenchmark.BeginBenchamrk();
    }

    private double BenchmarkParallelImageProcessing()
    {
        if (inputImage == null)
        {
            UnityEngine.Debug.LogError("Input image is not assigned.");
            return 0;
        }

        int width = inputImage.width;
        int height = inputImage.height;
        Color[] inputPixels = inputImage.GetPixels();
        Color[] outputPixels = new Color[inputPixels.Length];

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start(); // Start measuring time

        int threadCount = SystemInfo.processorCount;
        int rowsPerThread = height / threadCount;

        Task[] tasks = new Task[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int startRow = i * rowsPerThread;
            int endRow = i == threadCount - 1 ? height : startRow + rowsPerThread;
            tasks[i] = Task.Factory.StartNew(() => ApplyFilter(inputPixels, outputPixels, width, height, startRow, endRow));
        }

        Task.WaitAll(tasks);

        stopwatch.Stop(); // Stop measuring time

        Texture2D outputImage = new Texture2D(width, height);
        outputImage.SetPixels(outputPixels);
        outputImage.Apply();

        // Optionally, you can display or save the output image

        UnityEngine.Debug.Log("Parallel Image Processing Benchmark");
        UnityEngine.Debug.Log($"Image size: {width}x{height}");
        UnityEngine.Debug.Log($"Time Took: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");

        return stopwatch.Elapsed.TotalMilliseconds/1000;

    }

    private void ApplyFilter(Color[] inputPixels, Color[] outputPixels, int width, int height, int startRow, int endRow)
    {
        // Apply a sample filter (e.g., grayscale conversion)
        for (int y = startRow; y < endRow; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                Color pixel = inputPixels[index];
                float grayscaleValue = (pixel.r + pixel.g + pixel.b) / 3.0f;
                outputPixels[index] = new Color(grayscaleValue, grayscaleValue, grayscaleValue, pixel.a);
            }
        }
    }
    private void SetCryptographicBenchmarkResult()
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.imageProcessingTime = totalTimeElapsed;
    }
}
