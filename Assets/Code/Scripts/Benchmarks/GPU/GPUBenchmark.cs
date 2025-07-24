using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using static Enums;

public class GPUBenchmark : MonoBehaviour
{
    public FPSMonitor fpsCounter; // Reference to the FPSCounter script
    public PlayableDirector benchamrkTimeline;

    private Benchmark benchmark;

    public GPU gpuScene;

    public void Start()
    {
        GameObject benchmarkObject = transform.Find("/[Utils]").gameObject;
        benchmark = benchmarkObject.GetComponent<Benchmark>(); 
    }

    public void BeginBenchmark()
    {

        fpsCounter.StartMonitoring();
        fpsCounter.measurementDuration = benchamrkTimeline.duration;

        benchamrkTimeline.Play();
        benchamrkTimeline.stopped += OnPlayableDirectorStopped;


    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (benchamrkTimeline == aDirector)
        {
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            benchmark.SetSchoolGPUBenchmarkData(fpsCounter.benchmarkData);
        }

    }
}

