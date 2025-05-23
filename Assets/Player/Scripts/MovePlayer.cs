using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 direction = (cameraForward * v + cameraRight * h).normalized;
        Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }

    void Update()
    {
        OrientToMouse();
    }

    private void OrientToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 lookTarget = hit.point;
            lookTarget.y = transform.position.y;

            Vector3 direction = (lookTarget - transform.position).normalized;
            if (direction != Vector3.zero && (lookTarget - transform.position).magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
            }
        }
    }
}
