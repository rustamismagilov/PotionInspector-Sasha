using System.Collections;
using UnityEngine;


public class CustomerMovement : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("walk");
    private static readonly int Down = Animator.StringToHash("down");
    private static readonly int Waiting = Animator.StringToHash("waiting");
    private static readonly int Up = Animator.StringToHash("up");
    [SerializeField] private Transform[] approachPath;
    [SerializeField] private Transform[] departPath;
    [SerializeField] private float approachDuration = 5f;
    [SerializeField] private float departDuration = 7f;
    private Animator animator;

    private bool helpingCustomer = false;

    private Coroutine currentMoveRoutine;

    public bool HelpingCustomer()
    {
        return helpingCustomer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void CustomerReset()
    {
        if (!helpingCustomer) //reset customer only if they have already departed the scene
        {
            gameObject.SetActive(true);

            if (approachPath != null && approachPath.Length > 0)
            {
                transform.position = approachPath[0].position;
            }
        }
    }
    public void CustomerArrive()
    {
        //call customer to booth
        if (helpingCustomer) return;

        if (currentMoveRoutine != null)
        {
            StopCoroutine(currentMoveRoutine);
        }

        animator.SetTrigger(Walk);

        currentMoveRoutine = StartCoroutine(FollowPath(approachPath, approachDuration, ApproachCallback, OnApproachComplete));
    }

    public void CustomerDepart()
    {
        if (!helpingCustomer)
        {
            Debug.Log("Customer hasn't been served yet or not available.");
            return;
        }

        if (currentMoveRoutine != null)
            StopCoroutine(currentMoveRoutine);

        animator.SetBool(Waiting, false);
        animator.SetTrigger(Walk);

        currentMoveRoutine = StartCoroutine(FollowPath(departPath, departDuration, DepartCallback, OnDepartComplete));
    }


    IEnumerator FollowPath(Transform[] path, float duration, System.Action<int> onWaypoint, System.Action onComplete)
    {
        float totalDist = 0f;
        for (int i = 0; i < path.Length - 1; i++)
        {
            totalDist += Vector3.Distance(path[i].position, path[i + 1].position);
        }

        for (int i = 0; i < path.Length - 1; i++)
        {
            Vector3 start = path[i].position;
            Vector3 end = path[i + 1].position;
            float dist = Vector3.Distance(start, end);
            float segmentDuration = duration * (dist / totalDist);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / segmentDuration;
                transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * dist / segmentDuration);
                yield return null;
            }

            onWaypoint?.Invoke(i);  // notify reaching waypoint
        }

        onComplete?.Invoke();
    }

    void ApproachCallback(int waypointIndex)
    {
        switch (waypointIndex)
        {
            case 0:
                Debug.Log("Walking up");
                animator.SetTrigger(Up);
                break;
            case 1:
                Debug.Log("Reset up");
                animator.ResetTrigger(Up);
                Debug.Log("Set waiting");
                animator.SetBool(Waiting, true);
                break;
        }
    }

    private void OnApproachComplete()
    {
        helpingCustomer = true;
    }

    void DepartCallback(int waypointIndex)
    {
        switch (waypointIndex)
        {
            case 1:
                Debug.Log("Walking down");
                animator.SetTrigger(Down);
                break;
            case 2:
                Debug.Log("Just walking");
                animator.SetTrigger(Walk);
                break;
        }
    }


    private void OnDepartComplete()
    {
        helpingCustomer = false;
        gameObject.SetActive(false);
        Invoke(nameof(CustomerReset), 2f);
    }
}
