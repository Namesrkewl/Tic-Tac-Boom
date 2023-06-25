using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterStorySequence : TweenAnimation {
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject element;
    [SerializeField] private GameObject backgroundAudio;
    [SerializeField] private AudioClip chainsPulling;
    [SerializeField] private AudioClip loadingSound;
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject background;

    public void StartStory() {
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        if (!element.GetComponent<TweenAnimation>().IsMoving) {
            backgroundAudio = GameObject.Find("BackgroundAudio");
            backgroundAudio.GetComponent<AudioSource>().Stop();
            overlay.SetActive(true);
            LeanTween.alphaCanvas(view.GetComponent<CanvasGroup>(), 0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            LeanTween.move(background, new Vector3(0, 18f, 0), 2f);
            backgroundAudio.GetComponent<AudioSource>().PlayOneShot(chainsPulling);
            yield return new WaitForSeconds(2f);
            backgroundAudio.GetComponent<AudioSource>().Stop();
            backgroundAudio.GetComponent<AudioSource>().clip = loadingSound;
            backgroundAudio.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(2f);
            overlay.SetActive(false);
            sceneManager.GetComponent<LoadScene>().LoadStoryMode();
        }
    }
}
