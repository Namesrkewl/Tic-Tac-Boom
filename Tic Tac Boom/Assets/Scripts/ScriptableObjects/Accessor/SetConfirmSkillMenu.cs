using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetConfirmSkillMenu : MonoBehaviour
{
    public Image skillSprite;
    new public TextMeshProUGUI name;
    public TextMeshProUGUI description, cooldown;
    public Button cancel, use;
    public TalentObject talentObject;

    private void OnEnable() {
        if (talentObject != null) {
            skillSprite.sprite = talentObject.talent.sprite;
            name.text = talentObject.talent.name;
            description.text = talentObject.talent.description;
            if (PlayerManager.instance.player.state == Player.State.SelectingSkill) {
                if (PlayerManager.instance.player.skillUsed) {
                    cooldown.text = "SKILL USED";
                } else if (talentObject.talent.cooldown == 0) {
                    cooldown.text = "USE";
                } else {
                    cooldown.text = talentObject.talent.cooldown.ToString() + " TURNS";
                }
            } else if (PlayerManager.instance.enemy.state == Player.State.SelectingSkill) {
                if (PlayerManager.instance.player.skillUsed) {
                    cooldown.text = "SKILL USED";
                } else if (talentObject.talent.cooldown == 0) {
                    cooldown.text = "USE";
                } else {
                    cooldown.text = talentObject.talent.cooldown.ToString() + " TURNS";
                }
            }
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(PlayerManager.instance.CancelSkill);
            use.onClick.RemoveAllListeners();
            use.onClick.AddListener(delegate{PlayerManager.instance.ConfirmSkill(talentObject.talent);});
        }
    }
}
