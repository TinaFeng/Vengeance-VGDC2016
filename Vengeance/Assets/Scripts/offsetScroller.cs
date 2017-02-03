using UnityEngine;
using System.Collections;

public class offsetScroller : MonoBehaviour {

    public float scrollSpeed;

    private Renderer myRenderer;
    private Vector2 startingPosition;
    private float offsetY;

	// Use this for initialization
	void Start () {

        myRenderer = GetComponent<Renderer>();

        startingPosition = myRenderer.material.GetTextureOffset("_MainTex");
    }
	
	// Update is called once per frame
	void Update () {
        offsetY = Mathf.Repeat(Time.time * scrollSpeed, 1);

        myRenderer.material.mainTextureOffset = new Vector2(startingPosition.x, offsetY);
	}

    private void OnDisable()
    {
        myRenderer.material.mainTextureOffset = startingPosition;
    }
}
