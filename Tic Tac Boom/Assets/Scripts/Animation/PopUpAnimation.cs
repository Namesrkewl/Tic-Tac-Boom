using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnimation : MonoBehaviour
{
    public GameObject menu;
    // isMoving is used to determine if the button can be pressed or not
    public bool isMoving = false;

    // Animation for opening the menu
    public void Open()
    {
        if (!isMoving) {
            isMoving = true;
            // Sets the element to active
            Activate();
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
        }
    }

    // Animation for closing the menu
    public void Close()
    {
        if (!isMoving) {
            isMoving = true;
            LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.2f).setEaseOutExpo().setOnComplete(Deactivate);
        }
    }

    // Deactivate element
    void Deactivate()
    {
        gameObject.SetActive(false);
        isMoving = false;
        menu.SetActive(false);
    }

    // Activate element
    void Activate()
    {
        menu.SetActive(true);
        gameObject.SetActive(true);
        isMoving = false;
    }
}
