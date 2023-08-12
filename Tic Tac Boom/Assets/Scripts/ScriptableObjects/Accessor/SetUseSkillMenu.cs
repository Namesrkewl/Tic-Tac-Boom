using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUseSkillMenu : MonoBehaviour
{
    public Image skillSprite;
    public Button cancel;
    private bool firstEnable = true;

    private void OnEnable() {
        if (firstEnable != true) {
            if (PlayerManager.instance.player.state == Player.State.UsingSkill) {
                if (PlayerManager.instance.player.activeSkill != null) {
                    skillSprite.sprite = PlayerManager.instance.player.activeSkill.sprite;
                }
            } else if (PlayerManager.instance.enemy.state == Player.State.UsingSkill) {
                if (PlayerManager.instance.enemy.activeSkill != null) {
                    skillSprite.sprite = PlayerManager.instance.enemy.activeSkill.sprite;
                }
            }
            cancel.onClick.RemoveAllListeners();
            cancel.onClick.AddListener(PlayerManager.instance.CancelSkill);
        } else {
            firstEnable = false;
        }
    }
}
