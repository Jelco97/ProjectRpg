using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(HeightGround))]
//public class CustomHeight : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.PrefixLabel(position, label);
//
//        Rect newPosition = position;
//        newPosition.y += 18f;
//
//        SerializedProperty groundData = property.FindPropertyRelative("HeightGroundData");
//
//        for (int i = 0; i < 10; i++)
//        {
//            SerializedProperty row = groundData.GetArrayElementAtIndex(i).FindPropertyRelative("Row");
//
//            newPosition.height = 20;
//            newPosition.width = 30;
//
//            if (row.arraySize != 10)
//                row.arraySize = 10;
//
//            newPosition.x = (position.width - (10 * (newPosition.width + 2))) * .5f;
//
//            for (int j = 0; j < 10; j++)
//            {
//                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j), GUIContent.none);
//                newPosition.x += newPosition.width + 2f;
//            }
//
//            newPosition.y += newPosition.height + 2f;
//        }
//    }
//
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return 11 * 22;
//    }
//}
