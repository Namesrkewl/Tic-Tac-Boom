using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerMoveTrigger : MonoBehaviour, IPointerClickHandler
{
    PlayerManager playerManager;

    private void Start() {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Trigger player movement at the current game object when mouse is clicked
    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("Clicked!");
        if ((SceneManager.GetActiveScene().name == "StoryMode") && (playerManager.player.state == Player.State.Playing || playerManager.player.state == Player.State.UsingSkill) ) {
            EventManager.StartPlayerMoveEvent(gameObject);
        } else if (!(SceneManager.GetActiveScene().name == "StoryMode")) {
            EventManager.StartPlayerMoveEvent(gameObject);
        } //else {
            //EventManager.StartPlayerMoveEvent(gameObject);
       // } THIS IS FOR DEBUGGING STORY MODE AI
    }
}
