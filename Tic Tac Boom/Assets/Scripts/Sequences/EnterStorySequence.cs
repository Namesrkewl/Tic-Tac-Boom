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

    public void StartStory() {
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        if (!element.GetComponent<TweenAnimation>().IsMoving) {
            backgroundAudio = GameObject.Find("BackgroundAudio");
            backgroundAudio.GetComponent<AudioSource>().Stop();
            overlay.SetActive(true);
            LeanTween.moveLocal(view, new Vector3(0, 1920, 0), 2f);
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
