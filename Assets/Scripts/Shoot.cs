using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float waitChange = 0.2f;
    [SerializeField] private float waitMinimum = 1f;
    private float wait;

    [SerializeField] private float shootSpeed;

    [SerializeField] private GameObject projectilePrefab;

    private GameMaster gm;

    private int previousScoreMarker = 0;
    [SerializeField] private int scoreMarkAmount = 50;

    // Start is called before the first frame update
    void Start()
    {
        wait = waitTime;

        gm = FindObjectOfType<GameMaster>();

        StartCoroutine(ShootWait());
    }

    IEnumerator ShootWait()
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);

            if (gm.Score > previousScoreMarker + scoreMarkAmount && wait > waitMinimum)
            {
                wait -= waitChange;
                previousScoreMarker = gm.Score;
            }

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
