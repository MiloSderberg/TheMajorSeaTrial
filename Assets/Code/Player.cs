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

    [Header("Health")]
    public int health;
    int currentHealth;
    public float iFrames;
    float iTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = acceleration / (1 - friction);
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity += v * acceleration;
        Rotate();

        rb.linearVelocity = rb.linearVelocity * friction;

        iTime -= Time.deltaTime;
        print(currentHealth);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Hurt(collision);
    }

    void Hurt(Collider2D collision)
    {
        Damage dmg = collision.GetComponent<Damage>();
        if (dmg != null)
        {
            if (dmg.hurtPlayer && iTime <= 0)
            {
                currentHealth -= dmg.damageAmmount;
                iTime = iFrames;
                if (dmg.isOnHostile)
                {
                    collision.gameObject.GetComponent<Hostile>().Die();
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
        }
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
