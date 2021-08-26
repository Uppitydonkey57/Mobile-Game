using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float waitTime;

    [SerializeField] private float shootSpeed;

    [SerializeField] private GameObject projectilePrefab;

    private GameMaster gm;

    private int previousScoreMarker;
    [SerializeField] private int scoreMarkAmount = 50;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();

        StartCoroutine(ShootWait());
    }

    IEnumerator ShootWait()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (gm.Score > 100)
            {
                Vector2 position = transform.position;

                GameObject projectile = Instantiate(projectilePrefab, new Vector2(transform.position.x, Random.Range(position.y - 5, position.y + 5)), Quaternion.identity);

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

                projectileRb.AddForce(transform.up * shootSpeed, ForceMode2D.Impulse);
            }
        }
    }
}
