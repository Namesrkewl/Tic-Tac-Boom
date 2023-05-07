using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimation : MonoBehaviour
{
    public GameObject tic;
    public GameObject tac;
    public GameObject title;
    public GameObject button;
    public GameObject backgroundAudio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    public IEnumerator Intro() {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Tic());
        title.SetActive(true);
        tac.SetActive(false);
        tic.SetActive(false);
        yield return new WaitForSeconds(1f);
        backgroundAudio.SetActive(true);
        button.SetActive(true);
        LeanTween.alphaText(button.transform.Find("Text").gameObject.GetComponent<Text>().rectTransform, 1, 0.5f);
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
