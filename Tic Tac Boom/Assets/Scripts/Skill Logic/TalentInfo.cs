using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentInfo : MonoBehaviour
{
    [field: SerializeField] public int Cooldown { get; private set;}
    [field: SerializeField] public int Scaling { get; private set;}
    [field: SerializeField] public int Index { get; private set;}

    private void Start() {
        SetTalentInfo();
    }
    public void SetTalentInfo() {
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += $"\nCooldown: {Cooldown} | {Scaling}";
    }
    public void SelectSkill () {
        GameObject skills = GameObject.Find("SkillMenu").transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        Instantiate(GameManager.instance.talentChoices.talents[Index], skills.transform).name = GameManager.instance.talentChoices.talents[Index].name;
    }
}
