using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMenuAnimation : MonoBehaviour
{
    // Element is the object that we will be performing animations on
    public GameObject element;
    // inUse is used to determine if the button can be pressed or not
    private bool inUse = false;

    // Function used to call our set menu function; can only be called while the previous animation is not in use
    public void CallMenu()
    {
        if (!inUse)
        {
            StartCoroutine(SetMenu());
        }
    }

    // Sets our menu based on whether or not element is active in Unity
    public IEnumerator SetMenu()
    {
        inUse = true;
        if (!element.activeSelf)
        {
            OnOpen();
            yield return new WaitForSeconds(0.5f);
            inUse = false;
        }
        else
        {
            OnClose();
            // Timer for how long the button will be disabled after the animation is called
            yield return new WaitForSeconds(0);
            inUse = false;
        }
    }

    // Animation for opening the menu
    public void OnOpen()
    {
        // Sets the element to active
        Activate();
        // Sets the default position that the element will start at while animating
        //element.transform.localPosition = new Vector3(0, 0, 0);
        element.transform.localScale = new Vector3(0, 0, 0);
        // Sets the animation that will occur
        LeanTween.scale(element, new Vector3(1, 1, 1), 0.5f).setEaseOutElastic();
    }

    // Animation for closing the menu
    public void OnClose()
    {
        // Sets the animation that will occur, deactivates the element at the end 
        //LeanTween.moveLocal(element, new Vector3(-1500, 0, 0), 0.2f).setOnComplete(Deactivate);
        LeanTween.scale(element, new Vector3(0, 0, 0), 0.5f).setEaseOutExpo().setOnComplete(Deactivate);
    }

    // Destroys the element (currently not necessary)
    void DestroyMe()
    {
        Destroy(element);
    }

    // Deactivate element
    void Deactivate()
    {
        element.SetActive(false);
    }

    // Activate element
    void Activate()
    {
        element.SetActive(true);
    }
}
