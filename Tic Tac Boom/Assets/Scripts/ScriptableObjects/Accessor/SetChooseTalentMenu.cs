using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SetChooseTalentMenu : MonoBehaviour
{
    public Image skillSprite;
    new public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public Button cancel, select;
    public TalentObject talentObject;

    private void OnEnable() {
        if (talentObject != null) {
            skillSprite.sprite = talentObject.talent.sprite;
            name.text = talentObject.talent.name;
            description.text = talentObject.talent.description;
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(PlayerManager.instance.CloseTalentChoice);
            select.onClick.RemoveAllListeners();
            select.onClick.AddListener(delegate { PlayerManager.instance.ConfirmTalentChoice(talentObject.talent); });
        }
    }
}
