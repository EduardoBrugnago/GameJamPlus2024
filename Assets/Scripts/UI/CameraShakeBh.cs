using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeBh : MonoBehaviour
{
    public static CameraShakeBh instance;
    [SerializeField] private float globalShakeForce = 1.0f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource source)
    {

        source.GenerateImpulseWithForce(globalShakeForce);
    }
}
