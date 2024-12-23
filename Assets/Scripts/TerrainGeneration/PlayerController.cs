using TerrainGeneration;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Vector3 startPosition;

    private Rigidbody rb;

    private bool isGrounded = true;

    public Vector2Int CurrentPosition
    {
        get
        {
            return new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 move = moveDirection.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + move);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
