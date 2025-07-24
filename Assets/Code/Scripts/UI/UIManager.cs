using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Set the UI Objects from scene.")]
    [SerializeField] private UIObjects uiObjects;

    private LanguageStrings languageStrings;


    private void Start()
    {
        StartCoroutine(SetStringToUI());
    }

    private IEnumerator SetStringToUI()
    {
        yield return new WaitForSeconds(0.05f);

        GetLanguageStrings();

        uiObjects.UpdateStartButtonText(languageStrings.startBenchmarkLabel);

        uiObjects.SetBenchmarkButtonListener();
    }

    private void GetLanguageStrings()
    {
        GameObject languageStringsGameObject = transform.Find("/[Utils]").gameObject;
        languageStrings = languageStringsGameObject.GetComponent<LanguageStrings>();

    }


}
