using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameplateAnimation : MonoBehaviour
{
    public GameObject element;

    // isShowing is used to determine if the button can be pressed or not
    private bool isMoving = false;

    // Animation for opening the menu
    public void Show()
    {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 510, 0), 0.2f).setOnComplete(Deactivate);
        }
    }


    // Animation for closing the menu
    public void Hide()
    {
        if (!isMoving) {
            isMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(965, 510, 0), 0.2f).setOnComplete(Activate);
        }
    }

    // Deactivate element
    void Deactivate()
    {
        element.SetActive(false);
        isMoving = false;
    }

    // Activate element
    void Activate()
    {
        element.SetActive(true);
        isMoving = false;
    }
}
