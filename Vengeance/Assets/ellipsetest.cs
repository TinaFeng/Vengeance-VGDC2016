using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ellipsetest : MonoBehaviour
{

    public Transform x;
    public Transform y;
    public ellipsetest test;
    GameObject player;
    public float preciscion;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        EllipseColldier(test);
    }

    // Update is called once per frame
    Vector3 EllipseColldier(GameObject target)
    {
        float rot = transform.rotation.eulerAngles.z * Mathf.PI / 180;
        float rotZ = (transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180;
        float h = Vector3.Angle(target.transform.position - transform.position, new Vector3(Mathf.Cos(rot), Mathf.Sin(rot), 0));
        if (-Mathf.Cos(rot) * (target.transform.position.y - transform.position.y) + Mathf.Sin(rot) * (target.transform.position.x - transform.position.x) > 0)
            h = 360 - h;
        float t = Mathf.Atan((x.localPosition.x / y.localPosition.y) * Mathf.Tan(h * Mathf.PI / 180));
        if (-Mathf.Cos(rotZ) * (target.transform.position.y - transform.position.y) + Mathf.Sin(rotZ) * (target.transform.position.x - transform.position.x) > 0)
            t += Mathf.PI;
        //Debug.Log("Target Angle: " + h + " ||| Ellipse Angle: " + (t * 180 / Mathf.PI));
        return new Vector3(transform.position.x + x.localPosition.x * Mathf.Cos(t) * Mathf.Cos(rot) - y.localPosition.y * Mathf.Sin(t) * Mathf.Sin(rot), 
                           transform.position.y + x.localPosition.x * Mathf.Cos(t) * Mathf.Sin(rot) + y.localPosition.y * Mathf.Sin(t) * Mathf.Cos(rot),
                           0.0f);
        //Debug.DrawLine(target.transform.position, temp, Color.blue);
        //Debug.DrawLine(temp, transform.position, Color.red);
        //return temp;
        //Debug.Log(Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(test, transform.position));
    }

    bool EllipseColldier(ellipsetest target)
    {
        //closets point along major axis
        Vector3 temp1 = EllipseColldier(target.gameObject);
        Vector3 temp2 = target.EllipseColldier(gameObject);
        return Vector3.Distance(temp1, transform.position) > Vector3.Distance(temp2, transform.position);
    }
}