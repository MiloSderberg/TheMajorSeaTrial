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
    Camera cam;
    float a;
    InputAction attack;

    [Header("Health")]
    public GameObject[] healths;
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
        cam = Camera.main;
        attack = GetComponent<PlayerInput>().actions["Attack"];
    }

    // Update is called once per frame
    void Update()
    {
        Timers();
        rb.linearVelocity += v * acceleration;
        Rotate();

        rb.linearVelocity = rb.linearVelocity * friction;
        print(currentHealth);

        if (weapons != null)
        {
            RotateTurret();
        }

        if (attack.IsPressed())
        {
            FireWeapon();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Hurt(collision);
        HealthBar();
    }

    void HealthBar()
    {
        for (int i = 0; i < healths.Length; i++)
        {
            if (i > currentHealth - 1)
            {
                healths[i].SetActive(false);
            }
            else
            {
                healths[i].SetActive(true);
            }
        }
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

    void RotateTurret()
    {
        foreach (Weapons w in weapons)
        {
            Vector2 screenPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 rot = (screenPos - (Vector2)w.weapon.transform.position).normalized;
            w.weapon.transform.up = Vector2.Lerp(w.weapon.transform.up, rot, w.responsiveness * Time.deltaTime);
            w.weapon.transform.position = w.weaponPos.position;
        }
    }

    void FireWeapon()
    {
        foreach (Weapons w in weapons)
        {
            if (w.fireTimer <= 0)
            {
                GameObject spawnedBullet = Instantiate(w.bullet, w.fireFrom.transform.position, w.weapon.transform.rotation);
                spawnedBullet.GetComponent<Rigidbody2D>().linearVelocity = w.weapon.transform.up * w.bulletVelocity;
                w.fireTimer = w.fireRate;
            }
        }
    }

    void Timers()
    {
        if (weapons != null)
        {
            foreach (Weapons w in weapons)
            {
                w.fireTimer -= Time.deltaTime;
            }
        }
        iTime -= Time.deltaTime;
    }
}

[Serializable]
public class Weapons
{
    public GameObject weapon;
    public Transform weaponPos;
    public Transform fireFrom;
    public GameObject bullet;
    public float fireRate;
    [HideInInspector] public float fireTimer;
    public float responsiveness;
    public float bulletVelocity;
}
