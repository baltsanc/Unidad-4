
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float impulseForce = 3f;

    public bool ignoreNextCollision;
    private Vector3 startPosition;

    [HideInInspector]
    public int perfectPass;

    public float superSpeed = 8;
    private bool isSuperSpeedActive;
    public int perfectPassCount = 3;

    private Renderer ballRenderer;
    private Color originalColor;
    public Color superSpeedColor = Color.red;

    private TrailRenderer ballTrail;

    private void Start()
    {
        startPosition = transform.position;

        ballRenderer = GetComponent<Renderer>();
        originalColor = ballRenderer.material.color;

        ballTrail = GetComponent<TrailRenderer>();

        ballTrail.emitting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;

        if (isSuperSpeedActive && !collision.transform.GetComponent<GoalController>())
        {
            Destroy(collision.transform.parent.gameObject, 0.2f);
        }
        else
        {
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();

            if (deathPart)
                GameManager.singleton.RestartLevel();
        }

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke("AllowNextCollision", 0.2f);

        perfectPass = 0;

        isSuperSpeedActive = false;

        ballRenderer.material.color = originalColor;

        ballTrail.emitting = false;
        ballTrail.Clear();
    }

    private void Update()
    {
        if (perfectPass >= perfectPassCount && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;

            ballRenderer.material.color = superSpeedColor;

            ballTrail.Clear();
            ballTrail.emitting = true;

            rb.AddForce(Vector3.down * superSpeed, ForceMode.Impulse);
        }
    }

    private void AllowNextCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPosition;

        ballRenderer.material.color = originalColor;

        ballTrail.emitting = false;
        ballTrail.Clear();

        isSuperSpeedActive = false;
    }
}
