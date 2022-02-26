using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cmVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin;
    private float shakeTime;

    public static CinemachineCameraShake Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cmBasicMultiChannelPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void shakeCamera(float intensity, float time)
    {
        // accesses multichannelperlin on virtual camera that controls shaking.
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
                cmBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
