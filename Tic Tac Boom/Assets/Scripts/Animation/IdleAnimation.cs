using UnityEngine;

public class IdleAnimation : TweenAnimation
{
    void Start()
    {
        Expand();
    }

    public void Expand() {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1), 1f).setOnComplete(Contract);
    }

    public void Contract() {
        gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 1);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 1f).setOnComplete(Expand);
    }
}
