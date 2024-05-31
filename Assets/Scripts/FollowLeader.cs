
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader : MonoBehaviour
{
    public GameObject leader; // Asegúrate de asignar esto en el Inspector
    public float maxSpeed = 3f;
    public float leaderSideDistance = 2.5f;
    public float separationDistance = 2.5f;
    public float alignmentStrength = 1.5f;
    public float cohesionStrength = 0.5f;
    public float separationStrength = 0.5f;
    public float rotationStrength = 0.5f;
    public float evadeStrength = 0.5f;
    public float followerDistanceInLine = 2f; // Nueva variable pública
    private Rigidbody2D leaderRb;

    private Rigidbody2D rb;
    public static List<FollowLeader> followers = new List<FollowLeader>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the follower.");
            return;
        }

        if (!followers.Contains(this))
        {
            followers.Add(this);
        }

        leaderRb = leader.GetComponent<Rigidbody2D>();

        // Restringir la rotación del Rigidbody2D
        rb.freezeRotation = true;
    }

    private void OnDestroy()
    {
        if (followers.Contains(this))
        {
            followers.Remove(this);
        }
    }

    private void FixedUpdate()
    {
        if (leader == null)
        {
            Debug.LogError("Leader is not assigned.");
            return;
        }

        if (leaderRb == null)
        {
            Debug.LogError("Leader does not have a Rigidbody2D component.");
            return;
        }

        if (leaderRb.velocity.sqrMagnitude > 0.01f)
        {
            Vector2 sidePoint = CalculateSidePoint(leaderRb);
            Vector2 arrivalForce = Arrive(sidePoint);
            Vector2 separationForce = Separate();
            Vector2 alignmentForce = Align(leaderRb);
            Vector2 cohesionForce = Cohere(leaderRb);
            Vector2 evadeForce = EvadePath();

            Vector2 totalForce = arrivalForce + separationForce + alignmentForce + cohesionForce + evadeForce;

            // Limitar la fuerza total para que no supere la velocidad máxima
            totalForce = Vector2.ClampMagnitude(totalForce, maxSpeed);

            // Aplicar la fuerza al personaje
            rb.AddForce(totalForce);

            // Calcular la velocidad del personaje para determinar si está caminando
            float movementSpeed = totalForce.magnitude;

            // Actualizar la animación de caminar (si es necesario)
            // animator.SetFloat("Speed", movementSpeed);

            // Depuración
            Debug.Log("Separation Force: " + separationForce);
            Debug.Log("Arrival Force: " + arrivalForce);
            Debug.Log("Alignment Force: " + alignmentForce);
            Debug.Log("Cohesion Force: " + cohesionForce);
            Debug.Log("Evade Force: " + evadeForce);
            Debug.Log("Total Force: " + totalForce);
        }
        else
        {
            // El líder no se está moviendo, no aplicar fuerzas
            rb.velocity = Vector2.zero;
        }
    }

    private Vector2 CalculateSidePoint(Rigidbody2D leaderRb)
    {
        Vector2 leaderDirection = leaderRb.transform.right; // Utilizar la dirección hacia donde apunta el líder
        int index = followers.IndexOf(this);
        Vector2 sidePoint = (Vector2)leaderRb.position + leaderDirection * (index - followers.Count / 2 + 0.5f) * followerDistanceInLine;
        return sidePoint;
    }

    private Vector2 Arrive(Vector2 target)
    {
        Vector2 desiredVelocity = (target - (Vector2)transform.position).normalized * maxSpeed;
        Vector2 steering = desiredVelocity - rb.velocity;
        return steering;
    }

    private Vector2 Separate()
    {
        Vector2 separationForce = Vector2.zero;
        int index = followers.IndexOf(this);

        for (int i = 0; i < followers.Count; i++)
        {
            if (i != index)
            {
                FollowLeader follower = followers[i];
                Vector2 separationDirection = (transform.position - follower.transform.position).normalized;
                float distance = Vector2.Distance(transform.position, follower.transform.position);

                // Separación entre seguidores
                if (distance < separationDistance)
                {
                    separationForce += separationDirection / Mathf.Max(distance, 0.001f);
                }

                // Mantener distancia en fila lateral
                float desiredDistance = Mathf.Abs(index - i) * followerDistanceInLine;
                if (distance > desiredDistance)
                {
                    if (i < index)
                    {
                        separationForce -= separationDirection * (distance - desiredDistance);
                    }
                    else
                    {
                        separationForce += separationDirection * (distance - desiredDistance);
                    }
                }

                // Alineación horizontal
                Vector2 alignmentDirection = Vector2.right; // Dirección horizontal (puedes ajustar según sea necesario)
                separationForce += (alignmentDirection - separationDirection) * alignmentStrength;
            }
        }

        return separationForce * separationStrength;
    }

    private Vector2 Align(Rigidbody2D leaderRb)
    {
        Vector2 leaderVelocity = leaderRb.velocity;
        Vector2 desiredDirection = leaderVelocity.normalized;
        Vector2 currentDirection = rb.velocity.normalized;
        return (desiredDirection - currentDirection) * alignmentStrength;
    }

    private Vector2 Cohere(Rigidbody2D leaderRb)
    {
        Vector2 cohesionForce = ((Vector2)leaderRb.position - rb.position).normalized * cohesionStrength;
        return cohesionForce;
    }

    private Vector2 EvadePath()
    {
        // Implementar la lógica de evasión de obstáculos
        Vector2 evadeForce = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, separationDistance);
        if (hit.collider != null)
        {
            evadeForce = (Vector2)transform.position - hit.point;
        }
        return evadeForce * evadeStrength;
    }

    private void MaintainFormation()
    {
        if (leader == null || leader.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }

        Rigidbody2D leaderRb = leader.GetComponent<Rigidbody2D>();

        if (leaderRb.velocity.sqrMagnitude <= 0.01f)
        {
            // El líder no se está moviendo, mantener la formación
            int index = followers.IndexOf(this);
            Vector2 leaderVelocity = leaderRb.velocity.normalized;
            Vector2 perpendicularDirection = new Vector2(-leaderVelocity.y, leaderVelocity.x); // Dirección perpendicular a la velocidad del líder
            Vector2 desiredPosition = (Vector2)leaderRb.position + perpendicularDirection * (index - followers.Count / 2 + 0.5f) * followerDistanceInLine;
            rb.MovePosition(Vector2.Lerp(rb.position, desiredPosition, Time.deltaTime * 2f));
        }
    }
}

