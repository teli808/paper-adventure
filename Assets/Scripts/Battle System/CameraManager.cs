using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject defaultBattleViewCamera;
    [SerializeField] GameObject zoomOnPlayerTeamCamera;
    [SerializeField] GameObject zoomOnEnemyTeamCamera;
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

    public void SetAttackZoomOnCamera(Unit targetToFocus)
    {
        DeactivateCurrentCamera();

        if (targetToFocus == BattleSlotManager.Instance.GetPlayerUnit())
        {
            zoomOnPlayerTeamCamera.SetActive(true);
            currentCamera = zoomOnPlayerTeamCamera;
        }
        else
        {
            zoomOnEnemyTeamCamera.SetActive(true);
            currentCamera = zoomOnEnemyTeamCamera;
        }
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
