using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    public GameObject logo;
    public GameObject overlay;
    public GameObject button;
    public GameObject chains;
    public GameObject menu;
    public AudioSource chainsSounds;
    public AudioClip chainsFalling;
    public AudioClip chainsPulling;
    public GameObject sceneManager;

    public void StartGame() {
        StartCoroutine(Play());
    }

    public IEnumerator Play() {
        button.SetActive(false);
        LeanTween.alpha(logo.GetComponent<Image>().rectTransform, 0, 0.5f);
        LeanTween.alphaText(overlay.transform.Find("Text").gameObject.GetComponent<Text>().rectTransform, 0, 0.5f);
        yield return new WaitForSeconds(1f);
        logo.SetActive(false);
        overlay.SetActive(false);
        LeanTween.moveLocal(chains, new Vector3(0, -960, 0), 1f).setEaseOutCubic();
        chainsSounds.PlayOneShot(chainsFalling);
        yield return new WaitForSeconds(1f);
        chainsSounds.Stop();
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocal(chains, new Vector3(0, 2880, 0), 1f);
        LeanTween.moveLocal(menu, new Vector3(0, 0, 0), 1f);
        chainsSounds.PlayOneShot(chainsPulling);
        yield return new WaitForSeconds(1f);
        chainsSounds.Stop();
        yield return new WaitForSeconds(1f);
        sceneManager.GetComponent<LoadScene>().LoadMain();
    }
}