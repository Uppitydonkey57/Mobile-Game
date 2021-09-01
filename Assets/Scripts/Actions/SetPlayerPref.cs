using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPref : Action
{
    [SerializeField] private string prefName;
    [SerializeField] private int value;

    public override void PerformAction()
    {
        PlayerPrefs.SetInt(prefName, value);
    }
}
