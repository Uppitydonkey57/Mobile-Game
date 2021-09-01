using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : Action
{
    [Header("Make sure to switch gm scene switch to normal scene switch")]

    public string sceneName;

    public bool useBuildOrder;

    GameMaster gm;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    public override void PerformAction()
    {
        Time.timeScale = 1f;

        if (!useBuildOrder)
        {
            gm.LoadScene(sceneName);
        }
    }
}
