    
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoveTrigger : MonoBehaviour
{
    PlayerManager playerManager;

    private void Start() {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Trigger player movement at the current game object when mouse is clicked
    private void OnMouseUpAsButton() {
        if ((SceneManager.GetActiveScene().name == "StoryMode") && playerManager.player.state == Player.State.Playing) {
            EventManager.StartPlayerMoveEvent(gameObject);
        } else if (!(SceneManager.GetActiveScene().name == "StoryMode")) {
            EventManager.StartPlayerMoveEvent(gameObject);
        } //else {
            //EventManager.StartPlayerMoveEvent(gameObject);
       // } THIS IS FOR DEBUGGING STORY MODE AI
    }
}
