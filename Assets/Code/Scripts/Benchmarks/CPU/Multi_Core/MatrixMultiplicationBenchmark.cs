using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class MatrixMultiplicationBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    private int matrixSize = 2048; // Adjust the matrix size as needed
    private int numThreads;


    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }

    public void BeginMatrixMultiplicationBenchmark()
    {
        numThreads = SystemInfo.processorCount;

        UnityEngine.Debug.Log($"Matrix Size: {matrixSize}x{matrixSize}");
        UnityEngine.Debug.Log($"Number of Threads: {numThreads}");

        // Generate random matrices
        double[,] matrixA = GenerateRandomMatrix(matrixSize);
        double[,] matrixB = GenerateRandomMatrix(matrixSize);

        // Initialize the result matrix
        double[,] resultMatrix = new double[matrixSize, matrixSize];

        Stopwatch stopwatch = new Stopwatch();

        // Perform parallel matrix multiplication
        stopwatch.Start();
        Parallel.For(0, matrixSize, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, i =>
        {
            for (int j = 0; j < matrixSize; j++)
            {
                for (int k = 0; k < matrixSize; k++)
                {
                    resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        });
        stopwatch.Stop();

        // Measure execution time
        TimeSpan elapsedTime = stopwatch.Elapsed;
        double seconds = elapsedTime.TotalSeconds;

        UnityEngine.Debug.Log($"Execution Time: {seconds:F2} seconds");

        SetMatrixMultiplicationBenchmarkResult(seconds);

        cpuBenchmark.BeginBenchamrk();

        // Optionally, you can print or save the result matrix
        // PrintMatrix(resultMatrix);
    }

    private double[,] GenerateRandomMatrix(int size)
    {
        System.Random rand = new System.Random();
        double[,] matrix = new double[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j] = rand.NextDouble();
            }
        }

        return matrix;
    }

    private void PrintMatrix(double[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                UnityEngine.Debug.Log(matrix[i, j] + " ");
            }
            UnityEngine.Debug.Log("");
        }
    }
    private void SetMatrixMultiplicationBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.matrixMultiplicationTime = totalTimeElapsed;
    }
}
