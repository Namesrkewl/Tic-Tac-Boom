using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsAnimation : TweenAnimation {
    [SerializeField] private GameObject menu, element, veil;

    public void Update() {
        if (PlayerManager.instance.player.state == Player.State.Playing || PlayerManager.instance.player.state == Player.State.ViewingSkills) {
            if (!menu.activeSelf) {
                menu.SetActive(true);
            }
        } else {
            if (menu.activeSelf) {
                menu.SetActive(false);
            }
        }
    }

    // Animation for opening the menu
    public void Show() {
        if (!IsMoving) {
            PlayerManager.instance.player.state = Player.State.ViewingSkills;
            IsMoving = true;
            Deactivate();
            veil.SetActive(true);
            LeanTween.moveLocal(menu, new Vector3(0, -560, 0), 0.2f);
        }
    }


    // Animation for closing the menu
    public void Hide() {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(menu, new Vector3(0, -1400, 0), 0.2f).setOnComplete(Activate);
            veil.SetActive(false);
        }
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
        PlayerManager.instance.player.state = Player.State.Playing;
    }
}
