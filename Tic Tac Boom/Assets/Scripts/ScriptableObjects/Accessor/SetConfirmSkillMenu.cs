using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetConfirmSkillMenu : MonoBehaviour
{
    public Image skillSprite;
    new public TextMeshProUGUI name;
    public TextMeshProUGUI description, cooldown;
    public TalentObject talentObject;

    private void Update() {
        if (talentObject != null) {
            if (skillSprite.sprite != talentObject.sprite) {
                skillSprite.sprite = talentObject.sprite;
            }
            if (name.text != talentObject.name) {
                name.text = talentObject.name;
            }
            if (description.text != talentObject.description) {
                description.text = talentObject.description;
            }
            if (cooldown.text != talentObject.cooldown.ToString() + " TURNS" && talentObject.cooldown != 0) {
                cooldown.text = talentObject.cooldown.ToString() + " TURNS";
            } else if (cooldown.text != "USE" && talentObject.cooldown == 0) {
                cooldown.text = "USE";
            }
        }
    }

    public void SetMenu(GameObject skill) {
        if (PlayerManager.instance.player.state == Player.State.Playing) {
            for (int i = 0; i < PlayerManager.instance.player.talents.Count; i++) {
                if (skill.name == PlayerManager.instance.player.talents[i].talentName.ToString()) {
                    talentObject = PlayerManager.instance.player.talents[i].talentObject;
                }
            }
        } else if (PlayerManager.instance.enemy.state != Player.State.Playing) {
            for (int i = 0; i < PlayerManager.instance.enemy.talents.Count; i++) {
                if (skill.name == PlayerManager.instance.enemy.talents[i].talentName.ToString()) {
                    talentObject = PlayerManager.instance.enemy.talents[i].talentObject;
                }
            }
        }
    }
}
