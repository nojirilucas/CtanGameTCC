using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraSystemManager : MonoBehaviour
{
    public static CameraSystemManager Instance { get; private set; }

    public bool HasCameraAccess { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        HasCameraAccess = false;
    }

    public void GrantCameraAccess()
    {
        if (!HasCameraAccess)
        {
            HasCameraAccess = true;
            Debug.Log("Acesso ao sistema de câmeras concedido!");
        }
    }
}
