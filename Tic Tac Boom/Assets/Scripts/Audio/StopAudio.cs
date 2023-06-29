using UnityEngine;

public class StopAudio : MonoBehaviour
{
    [SerializeField] private GameObject source;
    // Start is called before the first frame update
    void Start()
    {
        Stop();
    }

    void Stop() {
        source.GetComponent<AudioSource>().Stop();
    }
}
