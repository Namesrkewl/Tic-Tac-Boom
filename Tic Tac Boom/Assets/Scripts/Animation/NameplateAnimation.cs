using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameplateAnimation : TweenAnimation
{
    [SerializeField] private GameObject element;

    // Animation for opening the menu
    public void Show()
    {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(0, 510, 0), 0.2f).setOnComplete(Deactivate);
        }
    }


    // Animation for closing the menu
    public void Hide()
    {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.moveLocal(gameObject, new Vector3(965, 510, 0), 0.2f).setOnComplete(Activate);
        }
    }

    // Deactivate element
    void Deactivate()
    {
        element.SetActive(false);
        IsMoving = false;
    }

    // Activate element
    void Activate()
    {
        element.SetActive(true);
        IsMoving = false;
    }
}
