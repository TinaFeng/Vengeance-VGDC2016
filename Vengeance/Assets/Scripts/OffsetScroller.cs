using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    public float scrollSpeed;
    PoolManager poolManager;
    private Renderer myRenderer;
    private Vector2 startingPosition;
    private float offsetY;
    float timeAlive;

	// Use this for initialization
	void Start () {
        timeAlive = 0;
        if (poolManager == null)
        {
            poolManager = GameObject.Find("ObjectPooling").GetComponent<PoolManager>();
        }
        myRenderer = GetComponent<Renderer>();

        startingPosition = myRenderer.material.GetTextureOffset("_MainTex");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!poolManager.timeStop)
        {
            if (poolManager.timeSlow)
            {
                timeAlive += Time.deltaTime * poolManager.timeSlowAmount;
            }
            else
            {
                timeAlive += Time.deltaTime;
            }
            offsetY = Mathf.Repeat(timeAlive * scrollSpeed, 1);
        }

        myRenderer.material.mainTextureOffset = new Vector2(startingPosition.x, offsetY);
	}

    private void OnDisable()
    {
        myRenderer.material.mainTextureOffset = startingPosition;
    }
}
