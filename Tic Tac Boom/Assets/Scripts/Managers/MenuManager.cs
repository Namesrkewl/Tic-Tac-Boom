using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject skillMenu, confirmSkillMenu, useSkillMenu, skills, UI, HUD, nextFight, grid, turnDisplay, playerVictoryMenu, enemyVictoryMenu, levelClearMenu, skillChoices, passiveChoices, chooseTalentMenu;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "StoryMode") {
            skillMenu = GameObject.Find("SkillMenu");
            confirmSkillMenu = GameObject.Find("ConfirmSkillMenu");
            useSkillMenu = GameObject.Find("UseSkillMenu");
            skills = GameObject.Find("Skills");
            UI = GameObject.Find("UI");
            HUD = GameObject.Find("HUD");
            nextFight = GameObject.Find("NextFight");
            grid = GameObject.Find("Grid");
            turnDisplay = GameObject.Find("TurnDisplay");
            playerVictoryMenu = GameObject.Find("PlayerVictoryMenu");
            enemyVictoryMenu = GameObject.Find("EnemyVictoryMenu");
            levelClearMenu = GameObject.Find("LevelClearMenu");
            skillChoices = GameObject.Find("SkillChoices");
            passiveChoices = GameObject.Find("PassiveChoices");
            chooseTalentMenu = GameObject.Find("ChooseTalentMenu");
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
