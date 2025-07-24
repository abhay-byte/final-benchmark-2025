using UnityEngine;
using System;
using System.Diagnostics;

public class DijkstrasPearlsBenchmark : MonoBehaviour
{
    private CPUBenchmark cpuBenchmark;

    public int gridSize = 350; // Adjust the grid size as needed
    private int[,] grid;
    private int[,] distance;
    private bool[,] visited;
    private int[] dx = { 0, 1, 1 };
    private int[] dy = { 1, 0, 1 };


    private void Awake()
    {
        Utils utility = GetComponent<Utils>();
        cpuBenchmark = utility.GetCPUBenchmarkScript();
    }
    public void BeginPearlsBenchmark()
    {
        InitializeGrid();
        BenchmarkDijkstrasAlgorithm();
    }

    private void InitializeGrid()
    {
        // Create a 1000x1000 grid with random pearl values (for demonstration purposes)
        grid = new int[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = UnityEngine.Random.Range(1, 10);
            }
        }

        distance = new int[gridSize, gridSize];
        visited = new bool[gridSize, gridSize];
    }

    private void BenchmarkDijkstrasAlgorithm()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        int shortestPathSum = FindShortestPath();
        stopwatch.Stop();

        TimeSpan elapsedTime = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"Shortest Path Sum: {shortestPathSum}");
        UnityEngine.Debug.Log($"Execution Time: {elapsedTime.TotalMilliseconds} ms");

        SetPearlsBenchmarkResult(elapsedTime.TotalMilliseconds/1000);
        cpuBenchmark.BeginBenchamrk();

    }

    private int FindShortestPath()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                distance[i, j] = int.MaxValue;
            }
        }

        distance[0, 0] = grid[0, 0];

        for (int count = 0; count < gridSize * gridSize - 1; count++)
        {
            int minDistance = int.MaxValue;
            int minI = -1, minJ = -1;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (!visited[i, j] && distance[i, j] < minDistance)
                    {
                        minDistance = distance[i, j];
                        minI = i;
                        minJ = j;
                    }
                }
            }

            visited[minI, minJ] = true;

            for (int k = 0; k < 3; k++)
            {
                int ni = minI + dx[k];
                int nj = minJ + dy[k];

                if (IsValid(ni, nj) && !visited[ni, nj] && distance[ni, nj] > distance[minI, minJ] + grid[ni, nj])
                {
                    distance[ni, nj] = distance[minI, minJ] + grid[ni, nj];
                }
            }
        }

        return distance[gridSize - 1, gridSize - 1];
    }

    private bool IsValid(int i, int j)
    {
        return i >= 0 && i < gridSize && j >= 0 && j < gridSize;
    }

    private void SetPearlsBenchmarkResult(double totalTimeElapsed)
    {
        CPUBenchmarkData benchmarkData = cpuBenchmark.benchmarkData;
        benchmarkData.dijkstraPearlsTime = totalTimeElapsed;
    }
}
