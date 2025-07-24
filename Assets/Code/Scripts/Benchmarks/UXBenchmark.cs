using UnityEngine;
[RequireComponent(typeof(FPSMonitor), typeof(Animator))]
public class UXBenchmark : MonoBehaviour
{
    public FPSMonitor fpsMonitor; // Reference to the FPSMonitor script
    public Animator animator;     // Reference to the Animator component

    private void Start()
    {
        if (fpsMonitor == null || animator == null)
        {
            Debug.LogError("Please assign references to the FPSMonitor and Animator in the Inspector.");
            enabled = false; // Disable the script if references are missing
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBenchmark();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopBenchmark();
        }
    }

    private void StartBenchmark()
    {
        // Start the FPS monitoring
        fpsMonitor.StartMonitoring();

        // Trigger the "StartRecording" animation
        animator.SetTrigger("StartRecording");
    }

    private void StopBenchmark()
    {
        // Stop the FPS monitoring
        fpsMonitor.StopMonitoring();

        // Trigger the "StopRecording" animation
        animator.SetTrigger("StopRecording");
    }
}
