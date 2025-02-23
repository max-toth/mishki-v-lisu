using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 0.1f;

    private float horizontal;
    private float vertical;

    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction.x, direction.y) * speed;
    }
}
