using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BenchmarkLog : MonoBehaviour
{
    [SerializeField] TMP_Text benchmarkLogText;
    private Benchmark benchmark;
    private void Awake()
    {
        GameObject Utils = transform.Find("/[Utils]").gameObject;
        benchmark = Utils.GetComponent<Benchmark>();

        StartBenchmark();
    }

    private void StartBenchmark()
    {
        benchmark.SetBenchmark();
    }

    public void AddToBenchmarkLog(string log)
    {
        benchmarkLogText.text += log;
    }

}
