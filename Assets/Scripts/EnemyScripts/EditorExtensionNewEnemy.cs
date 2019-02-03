using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAvatar))]
public class EditorExtensionNewEnemy : Editor {

    EnemyAvatar m_script;
    SerializedProperty m_projectile;
    SerializedProperty m_speedOfProjectile;
    SerializedProperty m_shootingPoint;

    SerializedProperty torusScaleSpeed;
    SerializedProperty torusMaxScaleTimes;
    SerializedProperty specialSkillRange;
    SerializedProperty specialSkillCooldown;

    //GameObject m_projectile;
    void OnEnable()
    {
        m_script = (EnemyAvatar)target;
        
        // Setup the SerializedProperties.
        m_projectile = serializedObject.FindProperty("projectile"); // find property projectile of script "NewEnemy"
        m_speedOfProjectile = serializedObject.FindProperty("projectileSpeed"); // find property projectile of script "NewEnemy"
        m_shootingPoint = serializedObject.FindProperty("shootingPoint");

        torusScaleSpeed = serializedObject.FindProperty("torusScaleSpeed"); 
        torusMaxScaleTimes = serializedObject.FindProperty("torusMaxScaleTimes");
        specialSkillRange = serializedObject.FindProperty("specialSkillRange");
        specialSkillCooldown = serializedObject.FindProperty("specialSkillCooldown");

    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI(); // tell to dislpay the basic gui, because we override the method
        m_script = (EnemyAvatar)target; // set the script we want to affect
        if (m_script.m_enemyType == EnemyTypes.EnemyRange)
        {

            EditorGUILayout.PropertyField(m_projectile, new GUIContent("Select projectile")); // display property for projectile
            EditorGUILayout.PropertyField(m_speedOfProjectile, new GUIContent("Select speed")); // display property for speed
            EditorGUILayout.PropertyField(m_shootingPoint, new GUIContent("Shooting point")); // display property for speed
            m_speedOfProjectile.serializedObject.ApplyModifiedProperties();
            m_projectile.serializedObject.ApplyModifiedProperties();

        }
        else if (m_script.m_enemyType == EnemyTypes.EnemyGolem) {
            EditorGUILayout.PropertyField(torusScaleSpeed, new GUIContent("Scale speed"));
            EditorGUILayout.PropertyField(torusMaxScaleTimes, new GUIContent("Special skill max scale"));
            EditorGUILayout.PropertyField(specialSkillRange, new GUIContent("Special skill trigger range"));
            EditorGUILayout.PropertyField(specialSkillCooldown, new GUIContent("Special skill Cooldown"));

            specialSkillRange.serializedObject.ApplyModifiedProperties();
            torusScaleSpeed.serializedObject.ApplyModifiedProperties();
            torusMaxScaleTimes.serializedObject.ApplyModifiedProperties();
            specialSkillCooldown.serializedObject.ApplyModifiedProperties();
        }
        
    }
    

}
