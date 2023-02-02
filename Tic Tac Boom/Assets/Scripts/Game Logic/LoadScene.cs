using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LoadScene : MonoBehaviour {
    public static LoadScene instance;
    private int currentSceneIndex = 0;
    private void Start() {
        if (GameObject.Find("SceneHistory") != null) {
            SceneHistory.instance.sceneHistory.Add(SceneManager.GetActiveScene().name);
        }
        if (instance != null) {
            Destroy(gameObject);
        }
    }
    private void Update() {
        if (GameObject.Find("SceneHistory") != null) {
            currentSceneIndex = SceneHistory.instance.sceneHistory.Count;
        }
    }

    public void LoadMain() {
        SceneManager.LoadScene("Main");
    }
    public void LoadSinglePlayer() {
        SceneManager.LoadScene("SinglePlayer");
    }
    public void LoadStoryMode() {
        GameManager.instance.gridSize = 3;
        SceneManager.LoadScene("StoryMode");
    }
    public void LoadPlayerVSAI() {
        if (!GameObject.Find("Label").GetComponent<Text>().text.Any(Char.IsLetter)) {
            GameManager.instance.gridSize = Convert.ToInt32(GameObject.Find("Label").GetComponent<Text>().text);
            SceneManager.LoadScene("PlayerVSAI");
        } else {
            Debug.Log("Pick a Grid Size");
        }
    }
    public void LoadLocalPVP() {
        if (!GameObject.Find("Label").GetComponent<Text>().text.Any(Char.IsLetter)) {
            GameManager.instance.gridSize = Convert.ToInt32(GameObject.Find("Label").GetComponent<Text>().text);
            SceneManager.LoadScene("PlayerVSAI");
        } else {
            Debug.Log("Pick a Grid Size");
        }
    }
    public void LoadCustomAI() {
        SceneManager.LoadScene("CustomAI");
    }
    public void LoadCustomLocal() {
        SceneManager.LoadScene("CustomLocal");
    }
    public void LoadSkins() {
        SceneManager.LoadScene("Skins");
    }
    public void LoadShop() {
        SceneManager.LoadScene("Shop");
    }
    public void LoadCredits() {
        SceneManager.LoadScene("Credits");
    }
    public void LoadLastScene() {
        if (currentSceneIndex > 1) {
            SceneManager.LoadScene(SceneHistory.instance.sceneHistory[currentSceneIndex - 2]);
            Debug.Log("Last scene loaded");
        } else {
            Debug.Log("No last scene found. Returning to Main Menu");
            SceneManager.LoadScene("Main");
        }
    }
}
