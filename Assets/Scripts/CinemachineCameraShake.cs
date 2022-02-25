using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraShake : MonoBehaviour
{
    public static CinemachineCameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cmVirtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cmBasicMultiChannelPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }
}
