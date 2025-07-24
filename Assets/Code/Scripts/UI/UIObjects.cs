using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIObjects : MonoBehaviour
{
    
    private LevelLoader levelLoader;
    [Header("UI Elements")] [Space(10)]

    [Tooltip("Add TMP_Text Elements from UI.")]
    [SerializeField] private TMP_Text[] TextObjects;

    [Tooltip("Add Button Elements from UI.")]
    [SerializeField] private Button[] ButtonObjects;

    private Dictionary<string, int> ButtonNames = new Dictionary<string, int>()
    {
        {"start_benchmark_button",0},
    };

    private Dictionary<string, int> TextNames = new Dictionary<string, int>()
    {
        {"start_benchmark_text",0},
    };

    public void UpdateStartButtonText(string text)
    {
        TextObjects[TextNames["start_benchmark_text"]].text = text;
    }

    public void SetBenchmarkButtonListener()
    {
        ButtonObjects[ButtonNames["start_benchmark_button"]].onClick.AddListener(LoadBenchmarkLogScene);
    }

    private void LoadBenchmarkLogScene()
    {
        GetLevelLoader();
        levelLoader.ChangeScene("BenchmarkLog");
    }

    private void GetLevelLoader()
    {
        GameObject LevelLoaderGameObject = transform.Find("/[Utils]").gameObject;
        levelLoader = LevelLoaderGameObject.GetComponent<LevelLoader>();
    }
}
