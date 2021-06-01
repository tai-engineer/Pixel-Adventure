using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Brick))]
public class BrickEditor : Editor
{
    SerializedProperty bounceSpeed;
    SerializedProperty canSpawn, coinPool, noOfCoin, disabledSprite;

    void OnEnable()
    {
        bounceSpeed = serializedObject.FindProperty("_bounceSpeed");
        canSpawn = serializedObject.FindProperty("_canSpawn");
        coinPool = serializedObject.FindProperty("_coinPool");
        noOfCoin = serializedObject.FindProperty("_noOfCoin");
        disabledSprite = serializedObject.FindProperty("_disabledSprite");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(disabledSprite);
        EditorGUILayout.PropertyField(bounceSpeed);

        EditorGUILayout.BeginHorizontal();
        canSpawn.isExpanded = EditorGUILayout.Foldout(canSpawn.isExpanded, new GUIContent("Has prefab to spawn"));
        EditorGUILayout.EndHorizontal();

        if(canSpawn.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(coinPool);
            EditorGUILayout.PropertyField(noOfCoin);
            EditorGUI.indentLevel--;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
