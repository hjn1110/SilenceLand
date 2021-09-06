using UnityEngine;
using UnityEngine.AI;

public class Rotate_Instantly : MonoBehaviour
{
    public bool showAhead;
    public bool showPath;

    private NavMeshAgent agent;
    private Vector3 nextWaypoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);

                if (agent.path.corners.Length == 1)
                {
                    nextWaypoint = agent.path.corners[0];
                }
            }
        }
        */

        if (agent.hasPath)
        {
            if (nextWaypoint != agent.path.corners[1])
            {
                RotateToPoint(agent.path.corners[1]);
                nextWaypoint = agent.path.corners[1];
            }
        }
    }

    private void RotateToPoint(Vector3 targetPoint)
    {
        Vector3 targetVector = targetPoint - transform.position;
        float angleDifference = Vector2.SignedAngle(transform.up, targetVector);
        transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + angleDifference);
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (showPath && agent.hasPath)
            {
                for (int i = 0; i + 1 < agent.path.corners.Length; i++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(agent.path.corners[i + 1], 0.03f);
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                }
            }

            if (showAhead)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.up * 0.5f);
            }
        }
    }
}
