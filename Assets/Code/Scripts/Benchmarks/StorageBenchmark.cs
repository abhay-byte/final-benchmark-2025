using UnityEngine;
using System.Diagnostics;
using System.IO;
using static Enums;
using System;

public class StorageBenchmark : MonoBehaviour
{

    [SerializeField] private Benchmark benchmark;
    private BenchmarkLog benchmarkLog;

    public string testFileName = "storage_test_file.txt";
    public int testDataSizeKB = 2097152; // Size of test data in kilobytes
    public int numIterations = 5;     // Number of iterations for benchmarking

    public int testDataSizeKB_4K = 4; // Size of test data for 4K in kilobytes
    public int numIterations_4K = 10000;

    private string testFilePath;

    private double totalReadPerSecond = 0;
    private double totalWritePerSecond = 0;

    public StorageBenchmarkData benchmarkData;

    private double readPerSecond;
    private double writePerSecond;
    private double readPerSecond4K;
    private double writePerSecond4K;


    private void Awake()
    {
        testFilePath = Path.Combine(Application.persistentDataPath, testFileName);
        Initialise();
        benchmarkLog.AddToBenchmarkLog("Storage Benchmark Began...\n");
    }

    private void Initialise()
    {
        benchmarkData = new StorageBenchmarkData();
        GameObject benchmarkLogGameObject = transform.Find("/scripts/benchmark_log").gameObject;
        benchmarkLog = benchmarkLogGameObject.GetComponent<BenchmarkLog>();
    }

    public void BeginBenchamrk()
    {

        BasicTest();

        FourKTest();

        SetStorageBenchmarkData();

        BeginNextBenchmark();
    }

    private void BasicTest()
    {
        for (int i = 0; i < numIterations; i++)
        {
            BenchmarkIterationBegin(testDataSizeKB);
        }

        readPerSecond = totalReadPerSecond / numIterations;
        writePerSecond = totalWritePerSecond / numIterations;

        UnityEngine.Debug.Log($"Storage Write Speed Test - Data Size: {testDataSizeKB} KB");
        UnityEngine.Debug.Log($"Write Speed: {readPerSecond:F2} MB/s");

        benchmarkLog.AddToBenchmarkLog($"Storage Write Speed Test - Data Size: {testDataSizeKB} KB\n");
        benchmarkLog.AddToBenchmarkLog($"Write Speed: {readPerSecond:F2} MB/s\n");

        UnityEngine.Debug.Log($"Storage Read Speed Test - Data Size: {testDataSizeKB} KB");
        UnityEngine.Debug.Log($"Read Speed: {writePerSecond:F2} MB/s");

        benchmarkLog.AddToBenchmarkLog($"Storage Read Speed Test - Data Size: {testDataSizeKB} KB\n");
        benchmarkLog.AddToBenchmarkLog($"Read Speed: {writePerSecond:F2} MB/s\n");
    }

    private void FourKTest()
    {
        for (int i = 0; i < numIterations; i++)
        {
            BenchmarkIterationBegin(testDataSizeKB_4K);
        }

        readPerSecond4K = totalReadPerSecond / numIterations;
        writePerSecond4K = totalWritePerSecond / numIterations;

        UnityEngine.Debug.Log($"Storage Write Speed Test - Data Size: {testDataSizeKB_4K} KB");
        UnityEngine.Debug.Log($"Write Speed: {readPerSecond4K:F2} MB/s");

        benchmarkLog.AddToBenchmarkLog($"Storage Write Speed Test 4K - Data Size: {testDataSizeKB_4K} KB\n");
        benchmarkLog.AddToBenchmarkLog($"Write Speed: {readPerSecond4K:F2} MB/s\n");

        UnityEngine.Debug.Log($"Storage Read Speed Test - Data Size: {testDataSizeKB_4K} KB");
        UnityEngine.Debug.Log($"Read Speed: {writePerSecond4K:F2} MB/s");

        benchmarkLog.AddToBenchmarkLog($"Storage Read Speed Test 4K - Data Size: {testDataSizeKB_4K} KB\n");
        benchmarkLog.AddToBenchmarkLog($"Read Speed: {writePerSecond4K:F2} MB/s\n");
    }


    private void BenchmarkIterationBegin(int testSize)
    {
        // Generate test data
        byte[] testData = new byte[testSize * 1024];
        new System.Random().NextBytes(testData);

        // Write test data to storage and measure time
        Stopwatch writeStopwatch = new Stopwatch();
        writeStopwatch.Start();

        using (FileStream fileStream = new FileStream(testFilePath, FileMode.Create, FileAccess.Write))
        {
            fileStream.Write(testData, 0, testData.Length);
        }

        writeStopwatch.Stop();
        double writeTimeMS = writeStopwatch.Elapsed.TotalMilliseconds;

        // Read test data from storage and measure time
        Stopwatch readStopwatch = new Stopwatch();
        readStopwatch.Start();

        using (FileStream fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[testData.Length];
            fileStream.Read(buffer, 0, buffer.Length);
        }

        readStopwatch.Stop();
        double readTimeMS = readStopwatch.Elapsed.TotalMilliseconds;

        // Delete the test file
        File.Delete(testFilePath);

        // Calculate write and read speeds in MB/s
        double writeSpeedMBPerS = (testSize / 1024.0) / (writeTimeMS / 1000.0);
        double readSpeedMBPerS = (testSize / 1024.0) / (readTimeMS / 1000.0);

        totalReadPerSecond += readSpeedMBPerS;
        totalWritePerSecond += writeSpeedMBPerS;

    }
    private void SetStorageBenchmarkData()
    {
        benchmark.SetStorageBenchmarkData(benchmarkData);

    }

    private void BeginNextBenchmark()
    {
        benchmark.SetBenchmark();
    }

}
public class StorageBenchmarkData
{
    public double writeSpeed;
    public double readSpeed;
    public double writeSpeed4K;
    public double readSpeed4K;

}
