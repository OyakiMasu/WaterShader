using UnityEngine;
using Unity.Profiling;
using TMPro; 

public class GPUFrameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsText;
    
    private ProfilerRecorder gpuFrameTimeRecorder;
    private ProfilerRecorder cpuFrameTimeRecorder;

    private float updateTimer = 0f;
    private float updateInterval = 0.25f; // Updates 4 times per second

    void OnEnable()
    {
        gpuFrameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "GPU Frame Time");
        cpuFrameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Main Thread Frame Time");
    }

    void OnDisable()
    {
        if (gpuFrameTimeRecorder.Valid)
            gpuFrameTimeRecorder.Dispose();
            
        if (cpuFrameTimeRecorder.Valid)
            cpuFrameTimeRecorder.Dispose();
    }

    void Update()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;

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