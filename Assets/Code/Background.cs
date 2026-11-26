using UnityEngine;

public class Background : MonoBehaviour
{
    float width;
    public GameObject[] background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width = background[0].transform.lossyScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject b in background)
        {
            if (b.transform.position.x <= -19.12263f)
            {
                b.transform.position = new Vector2(19.12263f, transform.position.y);
            }
        }
    }
}