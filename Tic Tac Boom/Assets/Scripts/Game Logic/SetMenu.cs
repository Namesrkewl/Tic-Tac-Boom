using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetMenu : MonoBehaviour
{
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
