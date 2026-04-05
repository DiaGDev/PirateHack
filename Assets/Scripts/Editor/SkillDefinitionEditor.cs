using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillDefinition))]
public class SkillDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillType"));

        SkillDefinition def = (SkillDefinition)target;

        switch (def.skillType)
        {
            case SkillDefinition.SkillType.Explode:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tokens"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Explode Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("explosionParticles"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("blockerLayer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pulsateDuration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldown"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
