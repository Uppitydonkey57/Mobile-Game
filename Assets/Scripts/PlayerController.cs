using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool dashing;
    private float curvePoint;

    private Touch touch;

    private GameMaster gm;

    /*REMOVE LATER*/
    public TextMeshProUGUI speedDebug;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();

        gm = FindObjectOfType<GameMaster>();

        moveSpeed = initialMoveSpeed;

        dashSpeed = initialDashSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        speedDebug.text = curvePoint.ToString() + "\n" + speedCurve.Evaluate(curvePoint).ToString();

        grounded = Physics2D.OverlapCircle(groundPoint.position, 0.01f, groundLayer);

        if (grounded)
        {
            if (curvePoint > 0)
            {
                curvePoint -= 0.0001f;
            }
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
                Debug.Log("Grab");
            }
            else if (touch.phase == TouchPhase.Began && grounded)
            {
                //Jump?
                Debug.Log("Jump");
            }
        }
    }

    //FIX THIS LATER THIS IS SOME REALLY AWFUL CODE!!
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructive"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Dash()
    {
        dashing = true;
        lineRenderer.enabled = true;

        //Maybe don't do this?
        Vector2 tagetPosition = (Vector2)grabPoint.transform.position - rb.position;
        float angle = Mathf.Atan2(tagetPosition.y, tagetPosition.x); // * Mathf.Rad2Deg - 90f
        //rb.rotation = angle;

        Vector2 moveDirection = new Vector2(Mathf.Cos(angle) * Mathf.Rad2Deg, Mathf.Sin(angle) * Mathf.Rad2Deg);

        float dashWaitTime = dashTime;

        bool startWaiting = false;

        float grabPointRadius = grabPoint.GetComponent<CircleCollider2D>().radius * grabPoint.transform.localScale.x;

        while (dashWaitTime > 0)
        {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
            lineRenderer.SetPosition(1, grabPoint.transform.position);

            //dashSpeed += 0.0002f;

            curvePoint += 0.0002f;

            rb.velocity = moveDirection * (dashSpeed * (speedCurve.Evaluate(curvePoint) + 1));

            if (touch.phase == TouchPhase.Ended)
            {
                break;
            }

            if (Physics2D.OverlapCircle(grabPoint.transform.position, grabPointRadius, playerLayer) && !startWaiting)
            {
                Debug.Log("Entered Grab Radius!");
                gm.ChangeScore(grabRadiusPoints);
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
        //moveSpeed = Mathf.Abs(rb.velocity.x);
        dashing = false;
    }
}
