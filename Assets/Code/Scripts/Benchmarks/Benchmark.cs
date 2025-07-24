using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.Advertisements;
using static Enums;
public class Benchmark : MonoBehaviour
{
    private LevelLoader loadLevel;

    private BenchmarkScripts benchmarkScripts;

    private BenchmarkData userBenchmarkData;

    private BenchmarkPoints benchmarkPoints;

    private CPUBenchmarkData cpuData;
    private GPUBenchmarkData gpuData;
    private StorageBenchmarkData storageData;
    private UXBenchmarkData uxData;
    private MemoryBenchmarkData memorydata;

    private GameObject benchmarkRunning;

    private Enums.benchmarks currentBenchmark;
    private Enums.GPU currentGPUBenchmark;
    private Enums.UX currentUXBenchmark;

    [SerializeField] private bool skipCPUBenchmark = false;
    [SerializeField] private bool skipGPUBenchmark = false;
    [SerializeField] private bool skipStorageBenchmark = false;
    [SerializeField] private bool skipMemoryBenchmark = false;

    void Awake()
    {
        benchmarkScripts = GetComponent<BenchmarkScripts>();
        ResetBenchmarkState();
    }

    private void ResetBenchmarkState()
    {
        gpuData = new GPUBenchmarkData();
        currentBenchmark = benchmarks.NOT_BEGAN;
        currentGPUBenchmark = GPU.NOT_BEGAN;
    }

    private void Start()
    {
        loadLevel = GetComponent<LevelLoader>();
        benchmarkScripts = GetComponent<BenchmarkScripts>();
    }

    public void SetBenchmark()
    {
        if (currentBenchmark == benchmarks.NOT_BEGAN)
        {

            if(skipCPUBenchmark)
            {
                currentBenchmark = benchmarks.CPU;
                SetBenchmark();
            }
            else
            {
                BeginCPUBenchmark();
            }

        }
        else if (currentBenchmark == benchmarks.CPU)
        {
            if(skipStorageBenchmark)
            {
                currentBenchmark = benchmarks.STORAGE;
                SetBenchmark();

            }
            else
            {
                BeginStorageBenchmark();
            }
        }
        else if (currentBenchmark == benchmarks.STORAGE)
        {
            if (skipMemoryBenchmark)
            {
                currentBenchmark = benchmarks.MEMORY;
                SetBenchmark();

            }
            else
            {
                BeginMemoryBenchmark();
            }

        }
        else if (GPU.TEMPLE != currentGPUBenchmark)
        {
            if(skipGPUBenchmark)
            {
                ShowBenchmarkResult();

            }
            else
            {
                SetGPUBenchmark();
            }
        }
        else
        {
            ShowBenchmarkResult();
        }

    }

    private void BeginCPUBenchmark()
    {
        currentBenchmark = benchmarks.CPU;

        benchmarkRunning = Instantiate(benchmarkScripts.cpuBenchmark.gameObject);
        benchmarkRunning.SetActive(true);
        CPUBenchmark benchmark = benchmarkRunning.GetComponent<CPUBenchmark>();
        benchmark.BeginBenchamrk();
    }

    private void BeginStorageBenchmark()
    {
        currentBenchmark = benchmarks.STORAGE;

        benchmarkRunning = Instantiate(benchmarkScripts.storageBenchmark.gameObject);
        benchmarkRunning.SetActive(true);
        StorageBenchmark benchmark = benchmarkRunning.GetComponent<StorageBenchmark>();
        benchmark.BeginBenchamrk();
    }

    private void BeginMemoryBenchmark()
    {
        currentBenchmark = benchmarks.MEMORY;

        benchmarkRunning = Instantiate(benchmarkScripts.memoryBenchmark.gameObject);
        benchmarkRunning.SetActive(true);
        MemoryBenchmark benchmark = benchmarkRunning.GetComponent<MemoryBenchmark>();
        benchmark.BeginBenchmark();
    }
    private void SetGPUBenchmark()
    {
        currentBenchmark = benchmarks.GPU;
        UnityEngine.AsyncOperation asyncOperation = null;

        if (currentGPUBenchmark == GPU.NOT_BEGAN)
        {
            currentGPUBenchmark = GPU.SCHOOL;
            asyncOperation = loadLevel.ChangeScene("SchoolGPU");
            Destroy(benchmarkRunning);
        }    
        else if (currentGPUBenchmark == GPU.SCHOOL)
        {
            currentGPUBenchmark = GPU.TEMPLE;
            asyncOperation = loadLevel.ChangeScene("TempleGPU");
        }
        StartCoroutine(SearchForGPUObject());
    }

    IEnumerator SearchForGPUObject()
    {
        yield return new WaitForSeconds(5f);
        GameObject benchmarkGameObject = transform.Find("/GPUBenchmark").gameObject;
        GPUBenchmark benchmark = benchmarkGameObject.GetComponent<GPUBenchmark>();
        benchmark.BeginBenchmark();

    }
    private void ShowBenchmarkResult()
    {
        ResetBenchmarkState();
        loadLevel.ChangeScene("BenchmarkLog");
    }

    public void SetCpuBenchmarkData(CPUBenchmarkData data)
    {
        cpuData = data;
    }

    public void SetStorageBenchmarkData(StorageBenchmarkData data)
    {
        storageData = data;
    }
    public void SetStorageMemoryData(MemoryBenchmarkData data)
    {
        memorydata = data;
    }

    public void SetSchoolGPUBenchmarkData(FPS_Stats data)
    {
        gpuData.schoolGPU = data;
        SetBenchmark();

    }

    public void SetTempleGPUBenchmarkData(FPS_Stats data)
    {
        gpuData.templeGPU = data;
        SetBenchmark();
    }
}

public class BenchmarkData
{
    public BenchmarkPoints benchmarkPoints;

    public CPUBenchmarkData cpuData;
    public GPUBenchmarkData gpuData;
    public StorageBenchmarkData storageData;
    public UXBenchmarkData uxData;
    public MemoryBenchmarkData ramData;
}

public class BenchmarkPoints
{
    public float cpuPoints;
    public float gpuPoints;
    public float storagePoints;
    public float ramPoints;
    public float uxPoints;

}

public class CPUBenchmarkData
{
    // General CPU Benchmark
    public CompressionBenchmarkData compressionBenchmarkdata;
    public CryptographicBenchmarkData cryptographicBenchmarkData;
    public double imageProcessingTime;
    public double meshProcessingTime;

    // Single Core Intensive CPU Benchmark
    public double dijkstraPearlsTime;
    public double simulationTime;
    public double piFindingTIme;

    // Multi-Core Intensive CPU Benchmark
    public double matrixMultiplicationTime;
    public double sortingTIme;
    public double primeNumberCalculationTime;

}

public class GPUBenchmarkData
{
    public FPS_Stats schoolGPU;
    public FPS_Stats templeGPU;
}

public class UXBenchmarkData
{
    public FPS_Stats appDemo;
    public FPS_Stats websiteDemo;
}

public class FPS_Stats
{
    public float averageFrameRate;
    public float noOfFrames;
    public float pointOnePercentage;
    public float stability;
}

