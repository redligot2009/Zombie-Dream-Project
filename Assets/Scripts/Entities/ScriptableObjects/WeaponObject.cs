using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Weapon", menuName = "Custom Objects/Weapon", order = 1)]
[System.Serializable]
public class WeaponObject : ScriptableObject
{
    public enum TriggerType { MANUAL, AUTOMATIC }
    public string weaponName = "Untitled";
    public float damage = 1;
    public float shootDelay = 0.5f;
    public float reloadTime = 1f;
    public int clipSize = 6;
    public float recoilVelocity = 0f;
    public float cameraShakeIntensity = 0f;
    public TriggerType triggerType;
    public AudioClip shotSound;
    public bool loopShot = false;
}
