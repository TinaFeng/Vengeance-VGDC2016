using UnityEngine;
using System.Collections;

public class BounceOnBoundary : MonoBehaviour
{
    float BoundaryRadius = 0.0f;
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
            angle = transform.rotation.z + 180f;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            transform.rotation = rot;
        }
        if (pos.y - BoundaryRadius <= -Camera.main.orthographicSize)
        {
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + BoundaryRadius >= widthOrtho)
        {
            angle = transform.rotation.z + 180f; ;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            transform.rotation = rot;
        }
        if (pos.x - BoundaryRadius <= -widthOrtho)
        {
            angle = transform.rotation.z + 180f;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            transform.rotation = rot;
        }
    }
}
