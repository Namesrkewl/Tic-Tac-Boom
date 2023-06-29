using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetMenu : MonoBehaviour
{
    public GameObject skill, image, description, cooldown, activeImage;

    public void SetSkillMenu() {
        skill.GetComponent<TextMeshProUGUI>().text = gameObject.name;
        image.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        description.GetComponent<TextMeshProUGUI>().text = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
        if (gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text != null) {
            cooldown.GetComponent<TextMeshProUGUI>().text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        } else {
            cooldown.GetComponent<TextMeshProUGUI>().text = "USE";
        }
        activeImage.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
    }
}
