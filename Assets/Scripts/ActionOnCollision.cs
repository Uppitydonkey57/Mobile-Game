using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnCollision : MonoBehaviour
{
    public Action[] actions;

    public Vector2 size;

    public LayerMask layer;

    public bool doOnce;

    bool hasPerformed;

    public Color collisionColor = Color.white;

    // Update is called once per frame
    void Update()
    {
        if ((hasPerformed && !doOnce) || !hasPerformed)
            if (Physics2D.OverlapBox(transform.position, size, 0, layer))
            {
                foreach (Action action in actions) action.PerformAction();
                hasPerformed = true;
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = collisionColor;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
