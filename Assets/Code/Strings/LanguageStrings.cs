using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageStrings: MonoBehaviour 
{
    private Enums.language currentLanguage;
    private string[] strings;

    public string startBenchmarkLabel = "";
    private string fileName;

    private void Start() 
    {

        GetSrings();
    }

    private void GetSrings()
    {
        GetCurrentLanguage();
        if (currentLanguage == Enums.language.ENGLISH)
        {
            fileName = "english.txt";
            string filePath = Path.Combine(Application.dataPath, fileName);
            //strings = File.ReadAllLines("D:/FinalBenchmark2/Assets/Code/Strings/english.txt");

            startBenchmarkLabel = "START";
        }
    }

    private void GetCurrentLanguage()
    {
        currentLanguage = Enums.language.ENGLISH;
    }
}
