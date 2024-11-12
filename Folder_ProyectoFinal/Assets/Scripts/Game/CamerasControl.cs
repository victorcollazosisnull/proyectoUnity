using Cinemachine;
using UnityEngine;

public class CamerasControl : MonoBehaviour
{
    [Header("Lara Croft Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;

    private void OnEnable()
    {
        LaraCroftInputReader inputReader = GetComponent<LaraCroftInputReader>();
        inputReader.OnAimInput += CamarasAimInput;
    }

    private void OnDisable()
    {
        LaraCroftInputReader inputReader = GetComponent<LaraCroftInputReader>();
        inputReader.OnAimInput -= CamarasAimInput;
    }

    private void CamarasAimInput(bool isAiming)
    {
        if (isAiming)
        {
            mainCamera.Priority = 1;
            aimCamera.Priority = 2;
        }
        else
        {
            mainCamera.Priority = 2;
            aimCamera.Priority = 1;
        }
    }
}