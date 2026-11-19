
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

    // CAMBIO DE COLOR
    private Renderer ballRenderer;
    private Color originalColor;
    public Color superSpeedColor = Color.red;

    // ESTELA DE LA BOLA
    private TrailRenderer ballTrail;

    private void Start()
    {
        startPosition = transform.position;

        ballRenderer = GetComponent<Renderer>();
        originalColor = ballRenderer.material.color;

        // Obtener el Trail Renderer
        ballTrail = GetComponent<TrailRenderer>();

        // Mantener la estela desactivada al inicio
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

        // Desactivar supervelocidad
        isSuperSpeedActive = false;

        // Recuperar color original
        ballRenderer.material.color = originalColor;

        // Desactivar estela
        ballTrail.emitting = false;
        ballTrail.Clear();
    }

    private void Update()
    {
        if (perfectPass >= perfectPassCount && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;

            // Cambiar color a rojo
            ballRenderer.material.color = superSpeedColor;

            // Activar estela
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

        // Restaurar color y estela
        ballRenderer.material.color = originalColor;

        ballTrail.emitting = false;
        ballTrail.Clear();

        isSuperSpeedActive = false;
    }
}
