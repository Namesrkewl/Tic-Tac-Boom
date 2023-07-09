using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetMenu : MonoBehaviour
{
    public void SetConfirmSkillMenu() {
        GameObject confirmSkillMenu = GameObject.Find("ConfirmSkillMenu");
        confirmSkillMenu.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        confirmSkillMenu.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        confirmSkillMenu.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
        if (gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text != null) {
            confirmSkillMenu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        } else {
            confirmSkillMenu.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "USE";
        }
        GameObject.Find("ActiveSkill").transform.GetChild(0).GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
    }
    public void SetChooseTalentMenu(GameObject skill) {
        GameObject chooseTalentMenu = GameObject.Find("ChooseTalentMenu");
        chooseTalentMenu.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        chooseTalentMenu.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        chooseTalentMenu.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
        chooseTalentMenu.transform.GetChild(1).GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        chooseTalentMenu.transform.GetChild(1).GetChild(3).GetComponent<Button>().onClick.AddListener(skill.GetComponent<TalentInfo>().SelectSkill);
        chooseTalentMenu.transform.localPosition = Vector3.zero;
        chooseTalentMenu.transform.GetChild(1).localScale = Vector3.zero;
        LeanTween.scale(chooseTalentMenu.transform.GetChild(1).gameObject, Vector3.one, 0.5f).setEaseOutElastic();
    }
}
