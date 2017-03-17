using UnityEngine;
using System.Collections;

public class BounceOnBoundary : MonoBehaviour
{
    float BoundaryRadius = 0.0f;
    int x = 0;
    float angle = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.y + BoundaryRadius >= Camera.main.orthographicSize)
        {
            Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, 0f - transform.rotation.z));
            Destroy(gameObject);
        }
        if (pos.y - BoundaryRadius <= -Camera.main.orthographicSize)
        {
            Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, 180f - transform.rotation.z));
            Destroy(gameObject);
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + BoundaryRadius >= widthOrtho)
        {
            Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, 225f - transform.rotation.z));
            Destroy(gameObject);
        }
        if (pos.x - BoundaryRadius <= -widthOrtho)
        {
            Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, 135f - transform.rotation.z));
            Destroy(gameObject);
        }
    }
}
