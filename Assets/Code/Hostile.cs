using UnityEngine;

public class Hostile : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    public float speed;

    [Header("Patterns")]
    public bool zigzag;
    public float zigzagSpeed;
    public float zigzagRate;
    float zigzagRand;

    public bool backForth;
    public float backForthRate;
    float backForthRand;

    public bool spin;
    public float spinSpeed;

    [Header("Attacks")]
    public GameObject bullet;
    public float bulletVelocity;
    public Transform fireFrom;
    public float fireRate;
    float fireTimer;
    public float rotOffset;

    public bool volley;
    public float volleyAmmount;
    public float volleySize;

    [Header("\"Difficulty\"")]
    public bool ultraInstinct;
    public bool doom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zigzagRand = Random.Range(0f, 1f);
        backForthRand = Random.Range(0f, 1f);
        fireRate = fireRate + Random.Range(0f, .1f * fireRate);
        if (spin == false) spinSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
        if (bullet != null)
        {
            Attack();
        }
    }

    void Move()
    {
        if (backForth)
        {
            BackForth();
        }
        else
        {
            rb.linearVelocityX = -speed;
        }
        if (zigzag)
        {
            ZigZag();
        }
        if (spin)
        {
            transform.rotation = Quaternion.AngleAxis(Time.time * spinSpeed, Vector3.forward);
        }
    }
    void ZigZag()
    {
        float oscillate = Mathf.Sin((Time.time * zigzagRate) + zigzagRand);
        rb.linearVelocityY = zigzagSpeed * oscillate;
    }
    void BackForth()
    {
        float oscillate = Mathf.Sin((Time.time * backForthRate) + backForthRand);
        rb.linearVelocityX = -speed * Mathf.Abs(oscillate);
    }

    void Attack()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            if (volley)
            {
                int s = (int)volleyAmmount / 2;
                for (int i = 1; i < volleyAmmount + 1; i++)
                {
                    bool isEven = false;
                    if (volleyAmmount % 2 == 0)
                    {
                        isEven = true;
                    }

                    float rot = RotCalc(i, s, isEven);
                    FireBullet(rot);
                    print($"{i}, {rot}");
                }
            }
            else
            {
                FireBullet(0);
            }
        }
    }
    float RotCalc(int i, int s, bool isEven)
    {
        float rot = 0;
        if (i <= s)
        {
            rot = ((volleySize / volleyAmmount) / 2) * i;
        }
        else if (i == s + 1 && isEven == false)
        {
            rot = 0;
        }
        else
        {
            rot = isEven ? -((volleySize / volleyAmmount) / 2) * (i - s)
                : -((volleySize / volleyAmmount) / 2) * (i - s - 1);
        }
        return rot;
    }

    void FireBullet(float rot)
    {
        GameObject spawnedBullet = Instantiate(bullet, fireFrom.position, Quaternion.AngleAxis(rot + rotOffset + (Time.time * spinSpeed), transform.forward));
        Vector2 dir = Quaternion.AngleAxis(rot, Vector3.forward) * -transform.right;
        spawnedBullet.GetComponent<Rigidbody2D>().linearVelocity = dir * bulletVelocity;
        fireTimer = fireRate;
    }
}
