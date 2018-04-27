using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemObject))]
public class ItemEditor : Editor
{
    public SerializedProperty
        itemType,
        pickUpType,
        healthRestore,
        associatedWeapon,
        objectSprite;
    private void OnEnable()
    {
        pickUpType = serializedObject.FindProperty("pickUpType");
        itemType = serializedObject.FindProperty("itemType");
        healthRestore = serializedObject.FindProperty("healthRestore");
        associatedWeapon = serializedObject.FindProperty("associatedWeapon");
        objectSprite = serializedObject.FindProperty("objectSprite");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ItemObject.ItemType itype = (ItemObject.ItemType)itemType.enumValueIndex;
        EditorGUILayout.ObjectField(objectSprite, typeof(Sprite));
        EditorGUILayout.PropertyField(pickUpType);
        EditorGUILayout.PropertyField(itemType);
        switch (itype)
        {
            case ItemObject.ItemType.HEALTH_PICKUP:
                int restoreVal = EditorGUILayout.IntField(new GUIContent("Health Restore", "Amount of health to restore"), healthRestore.intValue);
                healthRestore.intValue = restoreVal;
                break;
            case ItemObject.ItemType.ITEM_PICKUP:
                EditorGUILayout.ObjectField(associatedWeapon);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
