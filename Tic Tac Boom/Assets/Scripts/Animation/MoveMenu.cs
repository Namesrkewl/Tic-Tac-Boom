using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : TweenAnimation {
    public void MoveToHome() {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void MoveToBattle() {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(2160, 0, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void Show() {
        StartCoroutine(ShowMenu());
    }

    private IEnumerator ShowMenu() {
        StartCoroutine(MoveCurtainsIn());
        yield return new WaitForSeconds(0.8f);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
        yield return StartCoroutine(MoveCurtainsOut());
    }

    public void Hide() {
        StartCoroutine(HideMenu());
    }

    private IEnumerator HideMenu() {
        StartCoroutine(MoveCurtainsIn());
        yield return new WaitForSeconds(0.8f);
        gameObject.transform.localPosition = new Vector3(0, 3840, 0);
        yield return StartCoroutine(MoveCurtainsOut());
    }

    void StopMove() {
        IsMoving = false;
    }

    private IEnumerator MoveCurtainsIn() {
        // Get the objects and set them all active
        GameObject curtains = GameObject.Find("Curtains");
        GameObject center = curtains.transform.GetChild(0).gameObject;
        center.SetActive(true);
        GameObject left = curtains.transform.GetChild(1).gameObject;
        left.SetActive(true);
        GameObject right = curtains.transform.GetChild(2).gameObject;
        right.SetActive(true);

        // Left and Right curtains in frame
        LeanTween.moveLocal(left, new Vector3(0, 0, 0), 0.2f).setEaseInCubic();
        LeanTween.moveLocal(right, new Vector3(0, 0, 0), 0.2f).setEaseInCubic();
        yield return new WaitForSeconds(0.4f);

        // Center curtain moves down
        LeanTween.moveLocal(center, new Vector3(0, 0, 0), 0.2f).setEaseInCubic();
    }

    private IEnumerator MoveCurtainsOut() {
        // Get the objects and set them all active
        GameObject curtains = GameObject.Find("Curtains");
        GameObject center = curtains.transform.GetChild(0).gameObject;
        GameObject left = curtains.transform.GetChild(1).gameObject;
        GameObject right = curtains.transform.GetChild(2).gameObject;

        // Center curtain moves up
        LeanTween.moveLocal(center, new Vector3(0, 1920, 0), 0.2f).setEaseInCubic();
        yield return new WaitForSeconds(0.4f);

        // Left and Right curtains move out of frame
        LeanTween.moveLocal(left, new Vector3(-500, 0, 0), 0.2f).setEaseInCubic();
        LeanTween.moveLocal(right, new Vector3(500, 0, 0), 0.2f).setEaseInCubic();
        yield return new WaitForSeconds(0.2f);

        // Deactivate all the objects
        left.SetActive(false);
        right.SetActive(false);
        center.SetActive(false);
    }
    public void QuickShow() {
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }
    public void QuickHide() {
        gameObject.transform.localPosition = new Vector3(0, 3840, 0);
    }
}
