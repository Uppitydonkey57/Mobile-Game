#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Weapon weapon = (Weapon)target;

        #region Showing Universal Parameters
        ShowParameter("weaponName");
        ShowParameter("weaponType");
        ShowParameter("shootWait");
        ShowParameter("audioSource");
        ShowParameter("attackSounds");
        #endregion

        switch (weapon.weaponType) 
        {
            case Weapon.WeaponType.Melee:
                ShowParameter("weaponColor");
                ShowParameter("attackRange");
                ShowParameter("attackOffset");
                ShowParameter("hitTag");
                ShowParameter("damage");
                break;

            case Weapon.WeaponType.Projectile:
                ShowParameter("multipleFirePoints");
                if (weapon.multipleFirePoints)
                {
                    ShowParameter("firePoints");
                    ShowParameter("randomPoint");
                }
                else
                    ShowParameter("firePoint");
                ShowParameter("projectilePrefab");
                ShowParameter("projectileSpeed");
                ShowParameter("useFireChance");
                if (weapon.useFireChance)
                    ShowParameter("fireChance");
                break;

            case Weapon.WeaponType.Raycast:
                ShowParameter("hitTag");
                ShowParameter("damage");
                break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    void ShowParameter(string variable)  
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(variable));
    }
}
#endif