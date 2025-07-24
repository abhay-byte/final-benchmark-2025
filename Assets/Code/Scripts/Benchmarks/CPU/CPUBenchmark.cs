using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBenchmark : MonoBehaviour
{
    [SerializeField] private Benchmark benchmark;
    private BenchmarkLog benchmarkLog;

    [Header("CPU Benchmark List")]
    [SerializeField] private CompressionBenchmark compressionBenchmark;
    [SerializeField] private CryptographicBenchmark cryptographicBenchmark;
    [SerializeField] private ParallelImageProcessingBenchmark imageProcessingBenchmark;
    [SerializeField] private ParallelMeshProcessingBenchmark meshProcessingBenchmark;

    [SerializeField] private DijkstrasPearlsBenchmark dijkstraPearlsBenchmark;
    [SerializeField] private NoSQLClusterAvailabilityBenchmark simulationBenchmark;
    [SerializeField] private PiFindingBenchmark piFindingBenchmark;

    [SerializeField] private MatrixMultiplicationBenchmark matrixMultiplicationBenchmark;
    [SerializeField] private ParallelSortingBenchmark sortingBenchmark;
    [SerializeField] private PrimeNumberCalculationBenchmark primeNumberCalculationBenchmark;

    public CPUBenchmarkData benchmarkData;

    private Enums.CPU currentCPUBenchmark;
    private GameObject benchmarkRunning;

    private void Awake()
    {
        Initialise();
        benchmarkLog.AddToBenchmarkLog("CPU Benchmark Began...\n");
    }

    private void Initialise()
    {
        benchmarkData = new CPUBenchmarkData();
        currentCPUBenchmark = Enums.CPU.NOT_BEGAN;
        GameObject benchmarkLogGameObject = transform.Find("/scripts/benchmark_log").gameObject;
        benchmarkLog = benchmarkLogGameObject.GetComponent<BenchmarkLog>();
    }

    public void BeginBenchamrk()
    {
        if (currentCPUBenchmark == Enums.CPU.NOT_BEGAN)
            StartCoroutine(BeginCompressionBenchmark());

        else if (currentCPUBenchmark == Enums.CPU.COMPRESSION)
        {
            ShowCompressionBenchmarkResult();
            StartCoroutine(BeginCryptographicBenchmark());
        }

        else if (currentCPUBenchmark == Enums.CPU.CRYPTOGRAPHIC)
        {
            CryptographicBenchmarkData data = benchmarkData.cryptographicBenchmarkData;
            benchmarkLog.AddToBenchmarkLog($"3DES/CTR Decryption Benchmark: {data.decryptionTime} sec\n");
            benchmarkLog.AddToBenchmarkLog($"3DES/CTR Encryption Benchmark: {data.encryptionTime} sec\n");
            StartCoroutine(BeginImageProcessingBenchmark());
        }
            

        else if (currentCPUBenchmark == Enums.CPU.IMAGE)
        {
            benchmarkLog.AddToBenchmarkLog($"Parallel Image Processing Benchmark: {benchmarkData.imageProcessingTime} sec\n");
            StartCoroutine(BeginMeshProcessingBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.MESH)
        {
            benchmarkLog.AddToBenchmarkLog($"Parallel Mesh Processing Benchmark: {benchmarkData.meshProcessingTime} sec\n");
            StartCoroutine(BeginPearlsBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.PEARLS)
        {
            benchmarkLog.AddToBenchmarkLog($"Pearls Benchmark: {benchmarkData.dijkstraPearlsTime} sec\n");
            StartCoroutine(BeginSimulationBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.SIMULATION)
        {
            benchmarkLog.AddToBenchmarkLog($"Simulation Benchmark: {benchmarkData.simulationTime} sec\n");
            StartCoroutine(BeginPiBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.PI)
        {
            benchmarkLog.AddToBenchmarkLog($"PI Benchmark: {benchmarkData.piFindingTIme} sec\n");
            StartCoroutine(BeginMatrixMultiplicationBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.MATRIX)
        {
            benchmarkLog.AddToBenchmarkLog($"Matrix Multiplication Benchmark: {benchmarkData.matrixMultiplicationTime} sec\n");
            StartCoroutine(BeginSortingBenchmark());
        }


        else if (currentCPUBenchmark == Enums.CPU.SORTING)
        {
            benchmarkLog.AddToBenchmarkLog($"Sorting Benchmark: {benchmarkData.sortingTIme} sec\n");
            StartCoroutine(BeginPrimeNumberBenchmark());
        }

        else
        {
            benchmarkLog.AddToBenchmarkLog($"Prime Number Benchmark: {benchmarkData.primeNumberCalculationTime} sec\n");
            SetCPUBenchmarkData();
            benchmark.SetBenchmark();
        }

    }

    private IEnumerator BeginCompressionBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nCompression Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        currentCPUBenchmark = Enums.CPU.COMPRESSION;
        CreateGameObject(compressionBenchmark.gameObject);
        CompressionBenchmark BenchmarkScript = benchmarkRunning.GetComponent<CompressionBenchmark>();
        BenchmarkScript.CompressionBeginBenchmark();
    }

    private void ShowCompressionBenchmarkResult()
    {
        CompressionBenchmarkData data = benchmarkData.compressionBenchmarkdata;
        benchmarkLog.AddToBenchmarkLog($"Compression Time: {data.compressionTime} sec\n");
        benchmarkLog.AddToBenchmarkLog($"Base64 Encoding Time: {data.base64EncodingTime} sec\n");
        benchmarkLog.AddToBenchmarkLog($"Base64 Decoding Time: {data.base64DecordingTime} sec\n");
        benchmarkLog.AddToBenchmarkLog($"Decompression Time: {data.decompressionTime} sec\n");
    }

    private IEnumerator BeginCryptographicBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nCryptographic Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.CRYPTOGRAPHIC,cryptographicBenchmark.gameObject);
        CryptographicBenchmark BenchmarkScript = benchmarkRunning.GetComponent<CryptographicBenchmark>();
        BenchmarkScript.BeginCryptographicBenchmark();
    }

    private IEnumerator BeginImageProcessingBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nImage Processing Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.IMAGE,imageProcessingBenchmark.gameObject);
        ParallelImageProcessingBenchmark BenchmarkScript = benchmarkRunning.GetComponent<ParallelImageProcessingBenchmark>();
        BenchmarkScript.BeginImageProcessingBenchmark();
    }

    private IEnumerator BeginMeshProcessingBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nMesh Processing Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.MESH,meshProcessingBenchmark.gameObject);
        ParallelMeshProcessingBenchmark BenchmarkScript = benchmarkRunning.GetComponent<ParallelMeshProcessingBenchmark>();
        BenchmarkScript.BeginMeshProcessingBenchmark();
    }

    private IEnumerator BeginPearlsBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nPearls Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.PEARLS, dijkstraPearlsBenchmark.gameObject);
        DijkstrasPearlsBenchmark BenchmarkScript = benchmarkRunning.GetComponent<DijkstrasPearlsBenchmark>();
        BenchmarkScript.BeginPearlsBenchmark();
    }
    private IEnumerator BeginSimulationBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nSimulation Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.SIMULATION,simulationBenchmark.gameObject);
        NoSQLClusterAvailabilityBenchmark BenchmarkScript = benchmarkRunning.GetComponent<NoSQLClusterAvailabilityBenchmark>();
        BenchmarkScript.BeginSimulationBenchmark();
    }
    private IEnumerator BeginPiBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nPi Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.PI,piFindingBenchmark.gameObject);
        PiFindingBenchmark BenchmarkScript = benchmarkRunning.GetComponent<PiFindingBenchmark>();
        BenchmarkScript.BeginPiBenchmark();
    }

    private IEnumerator BeginMatrixMultiplicationBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nMatrix Multiplication Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.MATRIX,matrixMultiplicationBenchmark.gameObject);
        MatrixMultiplicationBenchmark BenchmarkScript = benchmarkRunning.GetComponent<MatrixMultiplicationBenchmark>();
        BenchmarkScript.BeginMatrixMultiplicationBenchmark();
    }
    private IEnumerator BeginSortingBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nSorting Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.SORTING,sortingBenchmark.gameObject);
        ParallelSortingBenchmark BenchmarkScript = benchmarkRunning.GetComponent<ParallelSortingBenchmark>();
        BenchmarkScript.BeginSortingBenchmark();
    }
    private IEnumerator BeginPrimeNumberBenchmark()
    {
        benchmarkLog.AddToBenchmarkLog("\nPrime Number Benchmark Began...\n");
        yield return new WaitForSeconds(3f);
        ChangeBenchmark(Enums.CPU.PRIME_NUMBER,primeNumberCalculationBenchmark.gameObject);
        PrimeNumberCalculationBenchmark BenchmarkScript = benchmarkRunning.GetComponent<PrimeNumberCalculationBenchmark>();
        BenchmarkScript.BeginPrimeNumberBenchmark();
    }

    private void SetCPUBenchmarkData()
    {
        benchmark.SetCpuBenchmarkData(benchmarkData);
        currentCPUBenchmark = Enums.CPU.NOT_BEGAN;
    }
    private void ChangeBenchmark(Enums.CPU type, GameObject gameObject)
    {
        CreateGameObject(gameObject);
        currentCPUBenchmark = type;
        DestroyGameObject();
    }
    private void CreateGameObject(GameObject gameObject)
    {
        benchmarkRunning = Instantiate(gameObject);
        benchmarkRunning.SetActive(true);
    }

    private void DestroyGameObject()
    {
        Destroy(benchmarkRunning);
    }
}
