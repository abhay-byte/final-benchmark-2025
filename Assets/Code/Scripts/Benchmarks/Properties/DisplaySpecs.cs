using UnityEngine;

public class DisplaySpecs : MonoBehaviour
{
    // Store the display specifications
    public int screenWidth;
    public int screenHeight;
    public float screenDPI;
    public Vector2 screenSizeInInches;
    public uint refreshRate;

    private void Start()
    {
        // Retrieve the current screen resolution
        Resolution currentResolution = Screen.currentResolution;
        screenWidth = currentResolution.width;
        screenHeight = currentResolution.height;
        refreshRate = currentResolution.refreshRateRatio.numerator;

        // Calculate the screen DPI
        screenDPI = Screen.dpi;

        // Calculate the screen size in inches
        float widthInInches = screenWidth / screenDPI;
        float heightInInches = screenHeight / screenDPI;
        screenSizeInInches = new Vector2(widthInInches, heightInInches);

        // Log the display specs
        Debug.Log("Screen Resolution: " + screenWidth + "x" + screenHeight);
        Debug.Log("Refresh Rate: " + refreshRate + "Hz");
        Debug.Log("Screen DPI: " + screenDPI);
        Debug.Log("Screen Size (Inches): " + screenSizeInInches.x + "x" + screenSizeInInches.y);
    }
}
