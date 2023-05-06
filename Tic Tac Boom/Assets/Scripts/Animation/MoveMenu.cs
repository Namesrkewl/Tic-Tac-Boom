using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : MonoBehaviour
{
    public bool isMoving = false;

    public void MoveToHome() {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void MoveToBattle() {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(2160, 0, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void MoveToTalents() {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(-2160, 0, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void MoveToSkins() {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, -3840, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    public void MoveToShop() {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 3840, 0), 0.5f).setEaseOutElastic().setOnComplete(StopMove);
        }
    }

    void StopMove() {
        isMoving = false;
    }
}
