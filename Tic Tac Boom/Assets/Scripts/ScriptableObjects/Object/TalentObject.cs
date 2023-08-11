using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerObject")]
public class TalentObject : ScriptableObject {
    public Sprite sprite;
    public new string name;
    public string description;
    public int cooldown;
}
