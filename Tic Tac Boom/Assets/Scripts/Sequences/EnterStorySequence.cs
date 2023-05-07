using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterStorySequence : TweenAnimation {
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject element;
    [SerializeField] private AudioSource storySounds;
    [SerializeField] private AudioClip chainsPulling;
    [SerializeField] private AudioClip loadingSound;
    [SerializeField] private GameObject sceneManager;

    public void StartStory() {
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        if (!element.GetComponent<TweenAnimation>().IsMoving) {
            LeanTween.moveLocal(view, new Vector3(0, 1920, 0), 1f);
            storySounds.PlayOneShot(chainsPulling);
            yield return new WaitForSeconds(1f);
            storySounds.clip = loadingSound;
            storySounds.loop = true;
            storySounds.Play();
        }
    }
}
