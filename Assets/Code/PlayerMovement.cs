using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float friction;
    public float turnRate;
    public float maxDegrees;
    Vector2 v;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity += v * speed;
        rb.linearVelocity = rb.linearVelocity * friction;
    }

    public void OnMove(Vector2 value)
    {
        v = value;
    }
}
