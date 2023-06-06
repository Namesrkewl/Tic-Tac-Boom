using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsAnimation : TweenAnimation {
    [SerializeField] private GameObject element;
    [SerializeField] private GameObject cancel;

    // Animation for opening the menu
    public void Show() {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), 0.2f).setOnComplete(Deactivate);
        }
    }


    // Animation for closing the menu
    public void Hide() {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, -3840, 0), 0.2f).setOnComplete(Activate);
        }
    }

    public void CancelBombUse() {
        GameManager.instance.talents.usingSmallBomb = false;
        GameManager.instance.talents.usingCrossBomb = false;
        GameManager.instance.talents.usingXBomb = false;
        GameManager.instance.talents.usingMine = false;
        GameManager.instance.bombInUse = false;
        cancel.SetActive(false);
    }

    // Deactivate element
    void Deactivate() {
        element.SetActive(false);
        IsMoving = false;
    }

    // Activate element
    void Activate() {
        element.SetActive(true);
        IsMoving = false;
    }
}
