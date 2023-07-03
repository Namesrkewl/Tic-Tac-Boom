using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentInfo : MonoBehaviour
{
    [field: SerializeField] public int Cooldown { get; private set;}
    [field: SerializeField] public int Scaling { get; private set;}

    private void Start() {
        SetTalentInfo();
    }
    public void SetTalentInfo() {
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += $"\nCooldown: {Cooldown} | {Scaling}";
    }
}
