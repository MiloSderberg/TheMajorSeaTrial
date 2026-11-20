using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    float maxSpeed;
    public float acceleration;
    public float friction;
    public float maxRot;
    float rot;
    Vector2 v;
    Rigidbody2D rb;

    [Header("Attacks")]
    public Weapons[] weapons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = acceleration / (1 - friction);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity += v * acceleration;
        Rotate();

        rb.linearVelocity = rb.linearVelocity * friction;
    }

    void Rotate()
    {
        float normalized = rb.linearVelocityY / maxSpeed;
        rot = maxRot * normalized;

        transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
    }

    public void OnMove(InputValue value)
    {
        v = value.Get<Vector2>().normalized;
    }
}

[Serializable]
public class Weapons
{

}
