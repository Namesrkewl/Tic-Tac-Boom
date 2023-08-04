using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Type type;
    public enum Type {
        Player,
        AI
    }
    public State state;
    public enum State {
        Inactive,
        Idle,
        Playing,
        SelectingSkill,
        UsingSkill
    }
    public Faction faction;
    public enum Faction {
        Exiled,
        Pure
    }
    public Character character;
    public enum Character {
        Basilisk,
        Common,
        Dragon,
        Engineer,
        Fragmented,
        King,
        Knight,
        Nimble,
        Peasant,
        Rebel,
        Thief,
        Vagabond,
        Wizard,
        Wraith
    }

    public Sprite characterSprite, playerSprite, skin;
    public List<Talent> talents;
    public int maxMoves, remainingMoves;
    public PlayerObject playerObject;
    public static Talent activeTalent;

    public Player(Type _type) {
        type = _type;
        state = State.Inactive;
    }

    public void SetPlayerObject() {
        playerObject.name = "THE " + character.ToString().ToUpper();
        playerObject.characterSprite = characterSprite;
        playerObject.playerSprite = playerSprite;
    }


    //[field: SerializeField] public bool IsExiled { get; private set; }
}
