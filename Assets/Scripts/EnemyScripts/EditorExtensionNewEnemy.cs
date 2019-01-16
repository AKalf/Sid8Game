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
    SerializedProperty  m_buffRange;
    SerializedProperty m_buffAmount;
	SerializedProperty m_buffCooldown; 
    //GameObject m_projectile;
    void OnEnable()
    {
        m_script = (EnemyAvatar)target;
        
        // Setup the SerializedProperties.
        m_projectile = serializedObject.FindProperty("projectile"); // find property projectile of script "NewEnemy"
        m_speedOfProjectile = serializedObject.FindProperty("projectileSpeed"); // find property projectile of script "NewEnemy"
        m_shootingPoint = serializedObject.FindProperty("shootingPoint");
        m_buffRange = serializedObject.FindProperty("m_buffRange"); // find property the buffrange property of script "NewEnemy"
        m_buffAmount = serializedObject.FindProperty("m_buffAmount");
		m_buffCooldown = serializedObject.FindProperty("m_buffCooldown");

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
        else if (m_script.m_enemyType == EnemyTypes.EnemyLeader) {
            EditorGUILayout.PropertyField(m_buffRange, new GUIContent("Select buff range"));
            EditorGUILayout.PropertyField(m_buffAmount, new GUIContent("Select buff amount"));
			EditorGUILayout.PropertyField(m_buffCooldown, new GUIContent("Select buff's cooldown"));
            m_buffRange.serializedObject.ApplyModifiedProperties();
            m_buffAmount.serializedObject.ApplyModifiedProperties();
			m_buffCooldown.serializedObject.ApplyModifiedProperties ();
        }
        
    }
    

}
