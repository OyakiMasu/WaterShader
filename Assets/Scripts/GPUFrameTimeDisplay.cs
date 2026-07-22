using UnityEngine;
using Unity.Profiling;
using TMPro; 

public class GPUFrameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    
    private ProfilerRecorder gpuFrameTimeRecorder;
    private ProfilerRecorder cpuFrameTimeRecorder;

    // Timer variables to slow down the UI updates
    private float updateTimer = 0f;
    private float updateInterval = 0.25f; // Updates 4 times per second

    void OnEnable()
    {
        // Hook into the native GPU and CPU Frame Time metrics
        gpuFrameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "GPU Frame Time");
        cpuFrameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Main Thread Frame Time");
    }

    void OnDisable()
    {
        // Always dispose of the recorders when disabled to prevent memory leaks
        if (gpuFrameTimeRecorder.Valid)
            gpuFrameTimeRecorder.Dispose();
            
        if (cpuFrameTimeRecorder.Valid)
            cpuFrameTimeRecorder.Dispose();
    }

    void Update()
    {
        // Advance the timer by the time elapsed since the last frame
        updateTimer += Time.deltaTime;

        // Only update the text if the timer exceeds our chosen interval
        if (updateTimer >= updateInterval)
        {
            // Reset the timer
            updateTimer = 0f;

            // Ensure we have a valid text component to write to
            if (statsText != null)
            {
                string displayText = "";

                if (cpuFrameTimeRecorder.Valid)
                {
                    float cpuTimeMs = cpuFrameTimeRecorder.LastValue / 1000000f;
                    displayText += $"CPU Time: {cpuTimeMs:F2} ms\n";
                }

                if (gpuFrameTimeRecorder.Valid)
                {
                    float gpuTimeMs = gpuFrameTimeRecorder.LastValue / 1000000f;
                    displayText += $"GPU Time: {gpuTimeMs:F2} ms";
                }

                statsText.text = displayText;
            }
        }
    }
}   