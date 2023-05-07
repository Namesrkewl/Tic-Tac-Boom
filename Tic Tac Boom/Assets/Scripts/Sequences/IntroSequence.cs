using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    public GameObject tic;
    public GameObject tac;
    public GameObject logo;
    public GameObject overlay;
    public GameObject button;
    public GameObject backgroundAudio;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    public IEnumerator Intro() {
        background.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        LeanTween.alpha(background.GetComponent<Image>().rectTransform, 0.5f, 2f);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Tic());
        logo.SetActive(true);
        background.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        tac.SetActive(false);
        tic.SetActive(false);
        yield return new WaitForSeconds(1f);
        backgroundAudio.SetActive(true);
        overlay.SetActive(true);
        LeanTween.alphaText(overlay.transform.Find("Text").gameObject.GetComponent<Text>().rectTransform, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        button.SetActive(true);
    }

    public IEnumerator Tic() {
        tic.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Tac());
    }

    public IEnumerator Tac() {
        tac.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }
}
