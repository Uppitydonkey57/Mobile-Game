
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    //Universal

    public WeaponType weaponType;

    public enum WeaponType { Projectile, Melee, Raycast }

    public float shootWait;
    bool canShoot = true;
    public string weaponName;

    public AudioSource audioSource;
    public AudioClip[] attackSounds;
    
    //Exclusive to melee weapons
    public Color weaponColor = Color.red;
    public Vector2 attackRange;
    public Vector2 attackOffset;

    //Exclusive to melee and raycast weapons
    public string hitTag;
    public float damage;

    //Exlusive to projectile weapons
    public bool multipleFirePoints;
    public Transform firePoint;
    public Transform[] firePoints;
    public bool randomPoint;

    public GameObject projectilePrefab;
    public float projectileSpeed;

    public bool useFireChance;
    public float fireChance;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    public void Attack()
    {
        if (canShoot)
        {
            if (attackSounds != null && attackSounds.Length > 0)
            {
                audioSource.PlayOneShot(attackSounds[UnityEngine.Random.Range(0, attackSounds.Length - 1)]);
            }

            switch (weaponType) {
                case WeaponType.Melee:
                    foreach (Collider2D collider in Physics2D.OverlapBoxAll((Vector2)transform.position + attackOffset, attackRange, 0))
                    {
                        Actor inRangeActor = collider.GetComponent<Actor>();

                        if (collider.GetComponent<Actor>() != null)
                        {
                            if (Array.Exists(inRangeActor.hitTags, element => element == hitTag))
                            {
                                inRangeActor.ChangeHealthKnockback(-damage, transform.right);
                            }
                        }
                    }
                    break;
                case WeaponType.Raycast:
                    break;

                case WeaponType.Projectile:
                    if (multipleFirePoints) {
                        if (!randomPoint)
                        {
                            foreach (Transform tempFirePoint in firePoints)
                            {
                                FireProjectile(tempFirePoint);
                            }
                        } else
                        {
                            FireProjectile(firePoints[UnityEngine.Random.Range(0, firePoints.Length - 1)]);
                        }
                    } else 
                    {
                        FireProjectile(firePoint);
                    }
                    
                    break;
            }
            StartCoroutine(ShootWait());
        }
    }

    void FireProjectile(Transform temporaryFirePoint)
    {
        float fireChanceNumber = Mathf.Round(UnityEngine.Random.Range(0, fireChance));

        if (!useFireChance || (useFireChance && fireChanceNumber == 0))
        {
            GameObject projectile = Instantiate(projectilePrefab, temporaryFirePoint.position, temporaryFirePoint.rotation);

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            projectileRb.AddForce(projectile.transform.up * projectileSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        if (weaponType == WeaponType.Melee)
        {
            Gizmos.color = weaponColor;

            Gizmos.DrawWireCube(transform.position + (Vector3)attackOffset, attackRange);
        }
    }

    IEnumerator ShootWait()
    {
        canShoot = false;

        yield return new WaitForSeconds(shootWait);

        canShoot = true;
    }
}