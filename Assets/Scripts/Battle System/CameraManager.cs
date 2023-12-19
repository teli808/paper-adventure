using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject defaultBattleViewCamera;
    [SerializeField] GameObject targetEnemyAttackCamera;
    [SerializeField] GameObject targetPlayerAttackCamera;
    [SerializeField] GameObject zoomOnEnemyDialogueCamera;

    [SerializeField] Camera mainCamera;
    [SerializeField] Camera playerUICamera;

    GameObject currentCamera;

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one CameraManager in the scene");
        }

        Instance = this;

        currentCamera = defaultBattleViewCamera;
    }

    private void LateUpdate()
    {
        playerUICamera.fieldOfView = mainCamera.fieldOfView;
    }

    public void DefaultViewCamera()
    {
        DeactivateCurrentCamera();
        defaultBattleViewCamera.SetActive(true);
        currentCamera = defaultBattleViewCamera;
    }

    public void ToggleAttackZoomCamera(UnitType unitTypeOfTarget)
    {
        if (unitTypeOfTarget == UnitType.ENEMY)
        {
            SetTargetEnemyAttackCamera();
        }
        else if (unitTypeOfTarget == UnitType.PLAYER)
        {
            SetTargetPlayerAttackCamera();
        }
    }

    public void SetTargetEnemyAttackCamera()
    {
        DeactivateCurrentCamera();
        targetEnemyAttackCamera.SetActive(true);
        currentCamera = targetEnemyAttackCamera;
    }

    public void SetTargetPlayerAttackCamera()
    {
        DeactivateCurrentCamera();
        targetPlayerAttackCamera.SetActive(true);
        currentCamera = targetPlayerAttackCamera;
    }

    public void SetZoomOnEnemyDialogueCamera()
    {
        DeactivateCurrentCamera();
        zoomOnEnemyDialogueCamera.SetActive(true);
        currentCamera = zoomOnEnemyDialogueCamera;
    }

    private void DeactivateCurrentCamera()
    {
        currentCamera.SetActive(false);
    }
}
