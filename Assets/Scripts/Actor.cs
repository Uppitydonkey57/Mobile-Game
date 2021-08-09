using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public string[] hitTags;

    ScreenShake screenShake;

    public bool shouldUseActivationRange;
    public bool deactivateAnimator;

    public MonoBehaviour[] behaviours;

    public Animator animator;

    [HideInInspector] public bool isActive;

    public float activationRange;

    public LayerMask activationLayer;

    public float shakeDelay;

    [Range(0f, 0.5f)] public float shakeDurationHit;
    [Range(0f, 0.5f)] public float shakeAmountHit;
    [Space]
    [Range(0f, 0.5f)] public float shakeDurationDead;
    [Range(0f, 0.5f)] public float shakeAmountDead;

    public float knockbackSpeed;

    public float healRate;

    //public bool shouldStun;

    Rigidbody2D rb;

    public Slider healthBar;

    public GameObject deathParticle;

    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip deathSound;

    public float hitWait;

    public bool DestroyOnDeath = true;
    public bool DestroyCanvas;
    public bool useDeathAnimBoolean = true;
    public bool useHitFlash = true;
    public Color hitFlashColor = Color.black;
    [Range(0f, 0.3f)] public float hitFlashDuration = 0.05f;

    List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    List<Color> spriteColors = new List<Color>();

    public bool isInvinsible;

    public bool useCorpseLayer;

    public bool debugHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        screenShake = FindObjectOfType<ScreenShake>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.SetFloat("Hp", maxHealth);
        }

        rb = GetComponent<Rigidbody2D>();

        if (shouldUseActivationRange)
        {
            if (behaviours != null)
            {
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour != null)
                        behaviour.enabled = false;
                }
            }

            if (deactivateAnimator)
                animator.enabled = false;
        }

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;

            healthBar.value = health;
        }

        if (GetComponents<SpriteRenderer>() != null)
        {
            foreach (SpriteRenderer rend in GetComponents<SpriteRenderer>())
            {
                sprites.Add(rend);
            }
        }

        if (GetComponentsInChildren<SpriteRenderer>() != null)
        {
            foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>())
            {
                sprites.Add(rend);
            }
        }

        if (sprites != null)
        {
            foreach (SpriteRenderer rend in sprites)
            {
                spriteColors.Add(rend.color);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, activationRange, activationLayer) != null)
        {
            Activate();
        }

        if (health + healRate < maxHealth)
        {
            ChangeHealth(healRate * Time.deltaTime);
        }

        if (debugHealth)
        {
            Debug.Log(gameObject.name + ": " + health);
        }
    }

    bool ParameterExists(string name)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == name)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void ChangeHealth(float amount)
    {
        if (!isInvinsible)
        {
            health += amount;

            if (animator != null && ParameterExists("Hp")) animator.SetFloat("Hp", health);

            if (!isActive) Activate();

            if (health + amount < health)
            {
                StartCoroutine(HitWait());

                if (useHitFlash && health > 0)
                {
                    StartCoroutine(HitFlash());
                    Debug.Log("HitFlash");
                }

                if (hitSounds != null && hitSounds.Length > 0)
                {
                    if (hitSounds.Length > 1)
                    {
                        audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length - 1)]);
                    } else
                    {
                        audioSource.PlayOneShot(hitSounds[0]);
                    }
                }

                if (animator != null)
                    animator.SetTrigger("Hit");

                Invoke(nameof(ShakeScreen), 0f);
            }

            if (healthBar != null)
                healthBar.value = health;

            if (health <= 0)
            {
                if (DestroyOnDeath)
                {
                    if ((hitSounds != null && hitSounds.Length > 0) || deathSound != null)
                    {
                        GameObject soundObject = new GameObject(gameObject.name + "'s Death Sound");
                        AudioSource objectSource = soundObject.AddComponent<AudioSource>();
                        objectSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
                        objectSource.volume = audioSource.volume;
                        objectSource.pitch = audioSource.pitch;

                        if (deathSound == null)
                        {
                            if (hitSounds.Length > 1)
                            {
                                objectSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length - 1)]);
                            }
                            else
                            {
                                objectSource.PlayOneShot(hitSounds[0]);
                            }
                        } else
                        {
                            objectSource.PlayOneShot(deathSound);
                        }

                        Destroy(soundObject, 10f);
                    }

                    if (deathParticle != null)
                    {
                        GameObject particle = Instantiate(deathParticle, transform.position, Quaternion.identity);
                        Destroy(particle, 10);
                    }

                    Destroy(gameObject);
                }
                else
                {
                    if (!useDeathAnimBoolean)
                    {
                        animator.SetTrigger("Dead");
                    } else
                    {
                        animator.SetBool("Dead", true);
                    }

                    if (useCorpseLayer)
                    {
                        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>())
                        {
                            rend.sortingLayerName = "Corpses";
                        }
                    }
                }

                if (debugHealth)
                {
                    Debug.Log(gameObject.name + ": " + health);
                }

                if (DestroyCanvas)
                {
                    Destroy(GetComponentInChildren<Canvas>().gameObject);
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<Actor>().enabled = false;
                }

                GetComponent<Actor>().enabled = false;
            }
        }
    }

    public virtual void ChangeHealthKnockback(float amount, Vector2 knockbackDirection)
    {
        ChangeHealth(amount);
        rb.AddForce(knockbackDirection * knockbackSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Activate()
    {
        if (shouldUseActivationRange)
        {
            if (behaviours != null)
            {
                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour != null)
                        behaviour.enabled = true;
                }
            }

            if (animator != null)
                animator.enabled = true;

            isActive = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (Array.Exists(hitTags, element => other.gameObject.CompareTag(element)))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();

            if (projectile != null)
            {
                ChangeHealthKnockback(-projectile.damage, projectile.transform.rotation.eulerAngles * -1);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Array.Exists(hitTags, element => other.gameObject.CompareTag(element)))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();

            if (projectile != null)
            {
                ChangeHealthKnockback(-projectile.damage, projectile.transform.rotation.eulerAngles * -1);
            }
        }
    }

    void ShakeScreen()
    {
        if (health <= 0)
        {
            screenShake.Shake(shakeDurationDead, shakeAmountDead);
        }
        else
        {
            screenShake.Shake(shakeDurationHit, shakeAmountHit);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        if (shouldUseActivationRange)
        {
            Gizmos.DrawWireSphere(transform.position, activationRange);
        }
    }

    IEnumerator HitFlash()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = hitFlashColor;
        }

        yield return new WaitForSeconds(hitFlashDuration);

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].color = spriteColors[i];
        }
    }

    IEnumerator HitWait()
    {
        if (hitWait > 0)
        {
            isInvinsible = true;

            yield return new WaitForSeconds(hitWait);

            isInvinsible = false;
        }
    }
}