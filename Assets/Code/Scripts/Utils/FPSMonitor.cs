using System;
using UnityEngine;

public class FPSMonitor : MonoBehaviour
{
    public double measurementDuration; // Duration for FPS measurement in seconds
    public bool isMonitoring = false;

    private float startTime;
    private int frameCount;
    private int highestFPS;
    private int lowestFPS;
    private double totalFPS;

    public FPS_Stats benchmarkData;

    private void Start()
    {
        ResetMeasurement();
        benchmarkData = new FPS_Stats();
    }

    private void ResetMeasurement()
    {
        frameCount = 0;
        startTime = Time.realtimeSinceStartup;
        highestFPS = 0;
        lowestFPS = int.MaxValue;
        totalFPS = 0;
    }

    private void Update()
    {
        if (isMonitoring)
        {
            frameCount++;

            if (Time.realtimeSinceStartup - startTime >= measurementDuration)
            {
                CalculateFPS();

                // Reset the measurement for the next interval
                StopMonitoring();
                SetBenchmarkData();
            }
        }
    }

    private void CalculateFPS()
    {
        double currentFPS = frameCount / measurementDuration;
        totalFPS += currentFPS;
        highestFPS = Mathf.Max(highestFPS, (int)currentFPS);
        lowestFPS = Mathf.Min(lowestFPS, (int)currentFPS);

        Debug.Log($"Average FPS: {totalFPS / frameCount:F2}");
        Debug.Log($"Highest FPS: {highestFPS}");
        Debug.Log($"Lowest FPS: {lowestFPS}");
        Debug.Log($"FPS in the last {measurementDuration} seconds: {currentFPS:F2}");


    }

    public void StartMonitoring()
    {
        isMonitoring = true;
        ResetMeasurement();
    }

    public void StopMonitoring()
    {
        isMonitoring = false;
    }

    private void SetBenchmarkData()
    {
        benchmarkData.stability = (lowestFPS / highestFPS) * 100;
        benchmarkData.averageFrameRate = (float)(totalFPS / frameCount);
        benchmarkData.noOfFrames = frameCount;
        benchmarkData.pointOnePercentage = lowestFPS;

    }
}
