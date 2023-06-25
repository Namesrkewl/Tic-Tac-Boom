using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject chains;
    [SerializeField] private GameObject menu;
    [SerializeField] private AudioSource chainsSounds;
    [SerializeField] private AudioClip chainsFalling;
    [SerializeField] private AudioClip chainsPulling;
    [SerializeField] private GameObject sceneManager;

    public void StartGame() {
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        button.SetActive(false);
        LeanTween.alpha(logo.GetComponent<Image>().rectTransform, 0, 0.5f);
        LeanTween.alphaText(overlay.transform.Find("Text").gameObject.GetComponent<Text>().rectTransform, 0, 0.5f);
        yield return new WaitForSeconds(1f);
        logo.SetActive(false);
        overlay.SetActive(false);
        LeanTween.move(chains, new Vector3(0, 0, 0), 1f).setEaseOutCubic();
        chainsSounds.PlayOneShot(chainsFalling);
        yield return new WaitForSeconds(1f);
        chainsSounds.Stop();
        yield return new WaitForSeconds(0.5f);
        LeanTween.move(menu, new Vector3(0, 28.5f, 0), 1f);
        chainsSounds.PlayOneShot(chainsPulling);
        yield return new WaitForSeconds(1f);
        chainsSounds.Stop();
        yield return new WaitForSeconds(0.5f);
        sceneManager.GetComponent<LoadScene>().LoadMain();
    }
}