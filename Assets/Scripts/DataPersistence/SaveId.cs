using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SaveId : MonoBehaviour
{
    [SerializeField] string uniqueIdentifier = ""; //do NOT change to property

    public string GetUniqueIdentifier()
    {
        return uniqueIdentifier;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.IsPlaying(gameObject)) return;

        if (string.IsNullOrEmpty(gameObject.scene.path)) return; //don't generate an ID if in the prefab scene

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

        if (string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}