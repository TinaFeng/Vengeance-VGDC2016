using UnityEngine;
using System.Collections;

public class BoundaryDestroy : MonoBehaviour
{
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
        }
        if (pos.y - BoundaryRadius < -Camera.main.orthographicSize)
        {
            Destroy(gameObject);
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + BoundaryRadius > widthOrtho)
        {
            Destroy(gameObject);
        }
        if (pos.x - BoundaryRadius < -widthOrtho)
        {
            Destroy(gameObject);
        }
    }
}
