using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLevelHandler : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;

    [SerializeField] private int baseEXP = 20;

    [SerializeField] TextMeshProUGUI experienceTMP;
    [SerializeField] GameObject levelUpCanvasObject;
    [SerializeField] TextMeshProUGUI healthMaxIncrease;
    [SerializeField] TextMeshProUGUI energyMaxIncrease;
    [SerializeField] ParticleSystem fireworksForLevelUp;

    Canvas experienceLevelCanvas;

    int experienceGainedThisBattle = 0;

    private void Awake()
    {
        experienceLevelCanvas = GetComponent<Canvas>();

        DisableExpComponents();
    }

    private void Start()
    {
        playerStatsManager = PlayerStatsManager.Instance;
    }

    public void AddExperienceFromEnemy(float multiplier, int playerLevel, int enemyLevel)
    {
        experienceGainedThisBattle += (int) Mathf.Ceil(baseEXP * enemyLevel * multiplier * enemyLevel / playerLevel);
    }

    public IEnumerator DisplayExperienceGained()
    {
        experienceTMP.enabled = true;

        experienceTMP.text = "EXP + " + experienceGainedThisBattle.ToString();

        yield return ExperienceGainedTweenAnimation();

        experienceTMP.enabled = false;
    }

    public IEnumerator CheckForLevelUp()
    {
        playerStatsManager.AddExperiencePoints(experienceGainedThisBattle);

        if (playerStatsManager.CheckLevelUp())
        {
            SetUpLevelUpScreen();

            levelUpCanvasObject.SetActive(true);
            fireworksForLevelUp.gameObject.SetActive(true);

            yield return new WaitForSeconds(4f);
        }
    }

    private void SetUpLevelUpScreen()
    {
        healthMaxIncrease.text = "+" + playerStatsManager.CalculateMaxStatIncrease(Stat.HEALTH) + " MAX INCREASE";
        energyMaxIncrease.text = "+" + playerStatsManager.CalculateMaxStatIncrease(Stat.ENERGY) + " MAX INCREASE";
    }

    private IEnumerator ExperienceGainedTweenAnimation()
    {
        experienceTMP.alpha = 1.0f;

        experienceTMP.transform.localScale = Vector3.zero;
        experienceTMP.transform.DOScale(Vector3.one, 1f);

        yield return new WaitForSeconds(1.5f);

        experienceTMP.DOFade(0, 1f);

        yield return new WaitForSeconds(1.5f);
    }

    public void EnableExpCanvas()
    {
        experienceLevelCanvas.enabled = true;
    }

    public void DisableExpComponents()
    {
        experienceLevelCanvas.enabled = false;

        experienceTMP.enabled = false;
        levelUpCanvasObject.SetActive(false);
        fireworksForLevelUp.gameObject.SetActive(false);
    }
}
