using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentChoices : MonoBehaviour
{
    public List<GameObject> talentChoices;
    public List<GameObject> talents;
    public List<int> talentsTaken;
    public List<int> talentsShowing;
    public List<int> talentsLocked;
    public void GenerateSkills(int amount) {
        int talent;
        bool isTalentLocked, isTalentTaken, isTalentShowing;
        GameObject choices = GameObject.Find("LevelClearMenu").transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        while (amount > 0 && (talentsLocked.Count + talentsTaken.Count + talentsShowing.Count) < talentChoices.Count) {
            do {
                talent = Random.Range(0, 6);
                isTalentLocked = false;
                isTalentTaken = false;
                isTalentShowing = false;
                if (!isTalentLocked) {
                    for (int i = 0; i < talentsLocked.Count; i++) {
                        if (talent == talentsLocked[i]) {
                            isTalentLocked = true;
                            break;
                        }
                    }
                }
                for (int i = 0; i < talentsTaken.Count; i++) {
                    if (talent == talentsTaken[i]) {
                        isTalentTaken = true;
                        break;
                    }
                }
                if (!isTalentTaken) {
                    for (int i = 0; i < talentsShowing.Count; i++) {
                        if (talent == talentsShowing[i]) {
                            isTalentShowing = true;
                            break;
                        }
                    }
                }

            } while (isTalentLocked || isTalentTaken || isTalentShowing);
            talentsShowing.Add(talent);
            Instantiate(talentChoices[talent], choices.transform).name = talentChoices[talent].name;
            amount--;
        }
    }

    public void ClearTalentChoices() {
        GameObject choices = GameObject.Find("LevelClearMenu").transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        for (int i = 0; i < choices.transform.childCount; i++) {
            Destroy(choices.transform.GetChild(i).gameObject);
        }
        talentsShowing.Clear();
    }
}
