using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : StoryModeAI
{
    public override GameObject AIMove() {
        if (FirstMove) {
            FirstMove = false;
            MoveCount = GameManager.instance.opponentMoveMax;
        }
        if (MoveCount % 2 == 1) {
            MoveCount--;
            return base.AIMove();
        } else {
            int gridX, gridY;
            do {
                int size = GameManager.instance.gridSize;
                gridX = Random.Range(0, (size));
                gridY = Random.Range(0, (size));
                MoveCount--;
            }
            while (GameObject.Find($"{gridX},{gridY}").transform.GetChild(0).gameObject.activeSelf);

            if (MoveCount <= 0) {
                FirstMove = true;
                MoveCount = GameManager.instance.opponentMoveMax;
            }

            return GameObject.Find($"{gridX},{gridY}");
        }
    }

    public override void SetAI() {
        if (gameObject != GameManager.instance.gameObject) {
            if (GameManager.instance.gameObject.GetComponent<DragonAI>()) {
                Destroy(GameManager.instance.gameObject.GetComponent<DragonAI>());
                GameManager.instance.gameObject.AddComponent<DragonAI>();
                // Sets Logic
                GameManager.instance.GetComponent<DragonAI>().BlockedValue = BlockedValue;
                GameManager.instance.GetComponent<DragonAI>().PlayerValue = PlayerValue;
                GameManager.instance.GetComponent<DragonAI>().BaseValue = BaseValue;
                GameManager.instance.GetComponent<DragonAI>().OpponentValue = OpponentValue;
                GameManager.instance.GetComponent<DragonAI>().ImpendingVictoryValue = ImpendingVictoryValue;
                GameManager.instance.GetComponent<DragonAI>().ImpendingDoomValue = ImpendingDoomValue;
                GameManager.instance.GetComponent<DragonAI>().VictoryValue = gameObject.GetComponent<DragonAI>().VictoryValue;
                // Sets Move Count
                GameManager.instance.opponentMoveMax = OpponentMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = StartingGridSize;
            } else if (GameManager.instance.gameObject.GetComponent<StoryModeAI>()){
                Destroy(GameManager.instance.gameObject.GetComponent<StoryModeAI>());
                GameManager.instance.gameObject.AddComponent<DragonAI>();
                // Sets Logic
                GameManager.instance.GetComponent<DragonAI>().BlockedValue = BlockedValue;
                GameManager.instance.GetComponent<DragonAI>().PlayerValue = PlayerValue;
                GameManager.instance.GetComponent<DragonAI>().BaseValue = BaseValue;
                GameManager.instance.GetComponent<DragonAI>().OpponentValue = OpponentValue;
                GameManager.instance.GetComponent<DragonAI>().ImpendingVictoryValue = ImpendingVictoryValue;
                GameManager.instance.GetComponent<DragonAI>().ImpendingDoomValue = ImpendingDoomValue;
                GameManager.instance.GetComponent<DragonAI>().VictoryValue = gameObject.GetComponent<DragonAI>().VictoryValue;
                // Sets Move Count
                GameManager.instance.opponentMoveMax = OpponentMoveMax;
                // Sets Starting Grid Size
                GameManager.instance.newGridSize = StartingGridSize;
            }
        }
    }
}
