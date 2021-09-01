using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundLayer;
    private bool grounded;

    [SerializeField] private float initialMoveSpeed = 5f;
    private float moveSpeed;

    private Rigidbody2D rb;

    [SerializeField] private float initialDashSpeed = 0.3f;
    [SerializeField] private AnimationCurve speedCurve;
    private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private LayerMask grabLayers;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int grabRadiusPoints;
    private GameObject grabPoint;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform armTarget;
    private bool dashing;
    private float curvePoint;

    [SerializeField] private ParticleSystem dustParticle;
    private float emissionSpeed;
    private ParticleSystem.EmissionModule dustEmission;

    [SerializeField] private GameObject graphicsObject;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip grappleSound;
    private AudioSource source;

    public delegate void DeadAction();
    public static event DeadAction PlayerDead;

    private Touch touch;

    private GameMaster gm;

    private Animator animator;

    [SerializeField] private TextMeshProUGUI speedDebug;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();

        gm = FindObjectOfType<GameMaster>();

        moveSpeed = initialMoveSpeed;

        dashSpeed = initialDashSpeed;

        animator = GetComponent<Animator>();

        source = GetComponent<AudioSource>();

        dustEmission = dustParticle.emission;
        emissionSpeed = dustEmission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedDebug != null) speedDebug.text = curvePoint.ToString() + "\n" + speedCurve.Evaluate(curvePoint).ToString();

        grounded = Physics2D.OverlapCircle(groundPoint.position, 0.01f, groundLayer);

        animator.SetBool("Grounded", grounded);

        if (grounded)
        {
            if (curvePoint > 0)
            {
                curvePoint -= 0.0001f;
            }

            dustEmission.rateOverTime = emissionSpeed;
        } else
        {
            dustEmission.rateOverTime = 0;
        }

        if (!dashing)
        {
            rb.velocity = new Vector2(moveSpeed * (speedCurve.Evaluate(curvePoint) + 1), rb.velocity.y);

            if (curvePoint > 0)
            {
                curvePoint -= 0.00001f;
            }
        }

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            Collider2D grabPointCollider = Physics2D.OverlapCircle(touchPosition, 0.01f, grabLayers);

            if (touch.phase == TouchPhase.Began && grabPointCollider != null)
            {
                grabPoint = grabPointCollider.gameObject;
                StartCoroutine(Dash());
            }
            else if (touch.phase == TouchPhase.Began && grounded)
            {
                //Jump?
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructive"))
        {
            PlayerDead?.Invoke();
            rb.rotation = 0f;
            rb.drag = 0.6f;
            rb.constraints = RigidbodyConstraints2D.None;
            lineRenderer.enabled = false;
            dustEmission.enabled = false;
            FindObjectOfType<ScreenShake>().Shake(.08f, .08f);
            FindObjectOfType<ScreenFreeze>().Freeze(0.07f);
            source.PlayOneShot(deathSound);
            animator.SetTrigger("Dead");
            Destroy(this);
        }
    }

    IEnumerator Dash()
    {
        dashing = true;
        lineRenderer.enabled = true;
        animator.SetBool("Dashing", true);
        source.PlayOneShot(grappleSound);

        Vector2 tagetPosition = (Vector2)grabPoint.transform.position - rb.position;
        float angle = Mathf.Atan2(tagetPosition.y, tagetPosition.x);
        graphicsObject.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90f);

        Vector2 moveDirection = new Vector2(Mathf.Cos(angle) * Mathf.Rad2Deg, Mathf.Sin(angle) * Mathf.Rad2Deg);

        float dashWaitTime = dashTime;

        bool startWaiting = false;

        float grabPointRadius = grabPoint.GetComponent<CircleCollider2D>().radius * grabPoint.transform.localScale.x;

        while (dashWaitTime > 0)
        {
            lineRenderer.SetPosition(0, new Vector3(armTarget.position.x, armTarget.position.y, 0));
            lineRenderer.SetPosition(1, grabPoint.transform.position);

            curvePoint += 0.0002f;

            rb.velocity = moveDirection * (dashSpeed * (speedCurve.Evaluate(curvePoint) + 1));

            if (touch.phase == TouchPhase.Ended)
            {
                break;
            }

            //Add Score
            if (Physics2D.OverlapCircle(grabPoint.transform.position, grabPointRadius, playerLayer) && !startWaiting)
            {
                GrabPoint grabPointCheck = grabPoint.GetComponentInParent<GrabPoint>();
                if (!grabPointCheck.hasBeenGrabbed)
                {
                    gm.ChangeScore(grabRadiusPoints);
                    grabPointCheck.hasBeenGrabbed = true;
                }
                startWaiting = true;
            }

            if (startWaiting)
            {
                dashWaitTime -= Time.deltaTime;
            }

            yield return null;
        }

        float yVelocity = rb.velocity.y;

        rb.velocity = new Vector2(rb.velocity.x, yVelocity *= 0.5f);
        lineRenderer.enabled = false;

        graphicsObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        dashing = false;
        animator.SetBool("Dashing", false);
    }
}
