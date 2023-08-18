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
        ViewingSkills,
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

    public Sprite playerSprite, characterSprite, skin;
    public List<Talent> skills, passives, unlockedSkills, unlockedPassives, skillsPool, passivesPool;
    public int maxMoves, remainingMoves;
    public bool skillUsed;
    public PlayerObject playerObject;
    public Talent activeSkill;
    public int initialCooldown;

    public Player() {
        state = State.Inactive;
        InitializeTalents();
        
    }

    public Player(Type _type) {
        type = _type;
        state = State.Inactive;
        skills = new List<Talent>();
        passives = new List<Talent>();
        activeSkill = null;
        skillUsed = false;
    }

    public void SetPlayerObject() {
        playerObject.name = "THE " + character.ToString().ToUpper();
        playerObject.characterSprite = characterSprite;
        playerObject.playerSprite = playerSprite;
    }

    public void InitializeTalents() {
        skills = new List<Talent>();
        passives = new List<Talent>();
        unlockedSkills = new List<Talent>();
        unlockedPassives = new List<Talent>();
        skillsPool = new List<Talent>();
        passivesPool = new List<Talent>();
        activeSkill = null;
        skillUsed = false;
    }

    public void SetUnlockedTalents() {
        unlockedSkills = GameManager.instance.unlockedSkills;
        unlockedPassives = GameManager.instance.unlockedPassives;
    }

    public void SetTalentPools() {
        skillsPool.Clear();
        passivesPool.Clear();
        for (int i = unlockedSkills.Count - 1; i >= 0; i--) {
            if (unlockedSkills[i].canAddTalent) {
                skillsPool.Add(unlockedSkills[i]);
                Debug.Log($"{unlockedSkills[i].talentName} added to skillsPool");
                unlockedSkills.RemoveAt(i);
            }
        }
        for (int i = unlockedPassives.Count - 1; i >= 0; i--) {
            if (unlockedPassives[i].canAddTalent) {
                passivesPool.Add(unlockedPassives[i]);
                Debug.Log($"{unlockedPassives[i].talentName} added to passivesPool");
                unlockedPassives.RemoveAt(i);
            }
        }
    }

    public void Cooldown() {
        if (skills != null) {
            for (int i = 0; i < skills.Count; i++) {
                if (skills[i].cooldown > 0) {
                    skills[i].cooldown--;
                }
                skills[i].SetTalentObject();
            }
        }        
        if (passives != null) {
            for (int i = 0; i < passives.Count; i++) {
                if (passives[i].cooldown > 0) {
                    passives[i].cooldown--;
                }
            }
        }
        skillUsed = false;
    }

    public void UsedSkill() {
        for (int i = 0; i < skills.Count; i++) {
            if (activeSkill.talentName == skills[i].talentName) {
                skills[i].cooldown = skills[i].maxCooldown;
                skills[i].maxCooldown += skills[i].scaling;
                
            }
        }
        activeSkill = null;
        skillUsed = true;
    }


    //[field: SerializeField] public bool IsExiled { get; private set; }
}
