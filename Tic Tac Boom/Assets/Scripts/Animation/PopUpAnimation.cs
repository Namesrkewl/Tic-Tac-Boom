using UnityEngine;

public class PopUpAnimation : TweenAnimation
{
    [SerializeField] private GameObject menu;

    // Animation for opening the menu
    public void Open()
    {
        if (!IsMoving) {
            IsMoving = true;
            // Sets the element to active
            Activate();
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
        }
    }

    // Animation for closing the menu
    public void Close()
    {
        if (!IsMoving) {
            IsMoving = true;
            LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.2f).setEaseOutExpo().setOnComplete(Deactivate);
        }
    }

    // Deactivate element
    void Deactivate()
    {
        gameObject.SetActive(false);
        IsMoving = false;
        menu.SetActive(false);
    }

    // Activate element
    void Activate()
    {
        menu.SetActive(true);
        gameObject.SetActive(true);
        IsMoving = false;
    }
}
