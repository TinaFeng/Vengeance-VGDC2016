using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoundary : MonoBehaviour {

    public float angle;
    BulletStats temp;

    void OnTriggerEnter2D(Collider2D coll)
    {
        //if (coll.gameObject.tag == "playerBullet")
        //{
        //    temp = coll.GetComponent<BulletStats>();
        //    if (((int)temp.moveType & 2) == 2 && temp.bounceCount < temp.bounceMax)
        //    {
        //        temp.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle - temp.gameObject.transform.rotation.eulerAngles.z);
        //    }
        //}
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "playerBullet")
        {
            //temp = coll.GetComponent<BulletStats>();
            //if (temp.bounceCount > temp.bounceMax - 1)
            //{
                coll.gameObject.SetActive(false);
            //}
            //temp.bounceCount++;
        }
    }
}
