using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Weapon", menuName = "Custom Objects/Weapon", order = 1)]
[System.Serializable]
public class WeaponObject : ScriptableObject
{
    public string weaponName = "Untitled";
    public float damage = 1;
    public float shootDelay = 0.5f;
    public float reloadTime = 1f;
    public int clipSize = 6;
}
