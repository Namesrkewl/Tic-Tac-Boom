using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public Camera backgroundCamera, gridCamera, overlayCamera;

    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "StoryMode") {
            backgroundCamera = GameObject.Find("BackgroundCamera").GetComponent<Camera>();
            gridCamera = GameObject.Find("GridCamera").GetComponent<Camera>();
            overlayCamera = GameObject.Find("OverlayCamera").GetComponent<Camera>();
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public IEnumerator FollowGrid(int size) {
        Vector3 newCameraPosition;
        if (size % 2 == 0) {
            GameObject midBotLeftTile = GridManager.instance.Tiles[(size / 2) - 1][(size / 2) - 1];
            GameObject midTopRightTile = GridManager.instance.Tiles[(size / 2)][(size / 2)];
            newCameraPosition = midBotLeftTile.transform.position + ((midTopRightTile.transform.position - midBotLeftTile.transform.position) / 2);
        } else {
            GameObject centerTile = GridManager.instance.Tiles[(size / 2)][(size / 2)];
            newCameraPosition = centerTile.transform.position;
        }
        newCameraPosition -= new Vector3(0, 0, 4f + (2.5f * size));
        if (GridManager.instance.state == GridManager.State.Tracking) {
            LeanTween.move(gridCamera.gameObject, newCameraPosition, 1f).setEaseOutCubic();
        } else {
            gridCamera.transform.position = newCameraPosition;
            GridManager.instance.state = GridManager.State.Idle;
        }
        yield return null;
    }
}
