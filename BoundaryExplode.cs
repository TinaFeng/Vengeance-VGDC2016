using UnityEngine;
using System.Collections;

public class BoundaryExplode : MonoBehaviour
{
    public GameObject bullet;
    float BoundaryRadius = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.y + BoundaryRadius > Camera.main.orthographicSize)
        {
            Destroy(gameObject);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 180));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 135));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 225));
        }
        if (pos.y - BoundaryRadius < -Camera.main.orthographicSize)
        {
            Destroy(gameObject);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 45));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 315));
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + BoundaryRadius > widthOrtho)
        {
            Destroy(gameObject);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 45));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 135));
        }
        if (pos.x - BoundaryRadius < -widthOrtho)
        {
            Destroy(gameObject);
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 270));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 315));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 225));
        }
    }
}
