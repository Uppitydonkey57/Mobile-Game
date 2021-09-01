using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public enum Height { Low, Medium, High }

    public Height height;

    public Vector2 size = new Vector2(20, 10);

    public void RemoveChunk()
    {
        Destroy(gameObject);
    }


    private void OnEnable()
    {
        //PlayerController.PlayerDead += PlayerDead;
    }

    private void OnDisable()
    {
        //PlayerController.PlayerDead -= PlayerDead;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    void PlayerDead()
    {
        Debug.Log("Player Dead!");

        if (transform.position.x > Camera.main.transform.position.x + 50 || transform.position.x < Camera.main.transform.position.x - 50)
        {
            RemoveChunk();
        }
    }
}
