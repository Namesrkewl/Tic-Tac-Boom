using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetText : MonoBehaviour
{
    private void Start() {
        SetStageText();
    }
    void SetStageText() {
        gameObject.GetComponent<TextMeshProUGUI>().text = $"LEVEL {GameManager.instance.stage}";
    }
}
