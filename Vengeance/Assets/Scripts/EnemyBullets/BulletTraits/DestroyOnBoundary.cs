using UnityEngine;
using System.Collections;

public class DestroyOnBoundary : MonoBehaviour
{
    float BoundaryRadius = -0.1f;

    // Update is called once per frame
    void FixedUpdate()
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
