using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private GameObject company;
    [SerializeField] private GameObject tic;
    [SerializeField] private GameObject tac;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject backgroundAudio;
    [SerializeField] private GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    private IEnumerator Intro() {
        background.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        LeanTween.alpha(background, 0.5f, 4f);
        //LeanTween.alphaText(company.GetComponent<TextMeshProUGUI>().rectTransform, 1, 2f);
        yield return new WaitForSeconds(2f);
        //LeanTween.alphaText(company.GetComponent<TextMeshProUGUI>().rectTransform, 0, 1f);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(Tic());
        logo.SetActive(true);
        background.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        tac.SetActive(false);
        tic.SetActive(false);
        yield return new WaitForSeconds(1f);
        backgroundAudio.SetActive(true);
        overlay.SetActive(true);
        LeanTween.alphaCanvas(overlay.GetComponent<CanvasGroup>(), 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        button.SetActive(true);
    }

    private IEnumerator Tic() {
        tic.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Tac());
    }

    private IEnumerator Tac() {
        tac.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }
}
