using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnAnyKey : MonoBehaviour
{
    public Action[] actions;

    public bool doOnce;

    bool hasPerformed;

    Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if ((hasPerformed && !doOnce) || !hasPerformed)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    foreach (Action action in actions) action.PerformAction();
                    hasPerformed = true;
                }
            }
        }
    }
}
