using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private enum OverworldLevels {
        TutorialLv1,
        TutorialLv2,
    }

    [SerializeField] OverworldLevels nameOfPortalLevel;

    [SerializeField] OverworldLevels connectedLevel;

    [SerializeField] Transform spawnPoint;

    SceneSwitcher sceneSwitcher;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        sceneSwitcher = GameObject.FindWithTag("SceneSwitcher").GetComponent<SceneSwitcher>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TransitionNextScene());
    }

    private IEnumerator TransitionNextScene()
    {
        EventManager.Instance.ChangePlayerState(PlayerState.DISABLED);

        yield return sceneSwitcher.TransitionOverworldToOverworld(connectedLevel.ToString());

        SpawnPlayerAtOtherPortal();

        Destroy(gameObject);
    }

    private void SpawnPlayerAtOtherPortal()
    {
        GameObject[] portalList = GameObject.FindGameObjectsWithTag("Portal");

        foreach (GameObject portal in portalList)
        {
            print(portal.GetComponent<LevelPortal>().nameOfPortalLevel);

            if (portal.GetComponent<LevelPortal>().nameOfPortalLevel == connectedLevel)
            {
                EventManager.Instance.SpawnPlayerAtPosition(portal.GetComponent<LevelPortal>().spawnPoint);
            }
        }
    }
}
