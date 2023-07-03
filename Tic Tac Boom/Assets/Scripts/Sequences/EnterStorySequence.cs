using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterStorySequence : TweenAnimation {
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject element;
    [SerializeField] private GameObject backgroundAudio;
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject shadows;


    public void StartStory() {
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        if (!element.GetComponent<TweenAnimation>().IsMoving) {
            backgroundAudio = GameObject.Find("BackgroundAudio");
            backgroundAudio.GetComponent<AudioSource>().Stop();
            overlay.SetActive(true);
            LeanTween.alphaCanvas(ui.GetComponent<CanvasGroup>(), 0, 0.1f);
            yield return new WaitForSeconds(0.1f);
            LeanTween.alpha(shadows, 0.8f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            sceneManager.GetComponent<LoadScene>().LoadStoryMode();
        }
    }
}
