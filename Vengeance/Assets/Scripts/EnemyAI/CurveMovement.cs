using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveMovement : MonoBehaviour {

    [System.Serializable]
    public class Movements
    {
        public Vector2[] positions;
        public float speed;
        public bool repeat;
    }

    int curr = 0;
    float t = 0;
    Vector2[] xy;
    Vector2[] xyTemp;
    int sl;
    public Movements[] moves;
    public bool repeat;

	void Update () {
        xyTemp = moves[curr].positions;
        sl = xyTemp.Length;
        xy = new Vector2[sl];
        System.Array.Copy(xyTemp, xy, sl);
        while (sl > 1)
        {
            for(int i = 0; i < sl-1; i++)
            {
                xy[i] = Vector2.Lerp(xy[i], xy[i + 1], t);
            }
            sl--;
        }
        gameObject.transform.position = xy[0];
        t = Mathf.Min(1, t + (Time.deltaTime / moves[curr].speed));
        if(t >= 1)
        {
            t = 0;
            curr++;
            if(curr >= moves.Length && !repeat)
            {
                Destroy(gameObject);
            }
            else
            {
                curr = 0;
            }
        }
    }
}
