using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cmVirtualCamera;
    private float shakeTime;

    public static CinemachineCameraShake Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmBasicMultiChannelPerlin.m_AmplitudeGain = intensity; 

        shakeTime = time;
    }

    void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cmBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
