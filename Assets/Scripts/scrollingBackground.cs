using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBackground : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private Renderer bgrenderer;

    // Update is called once per frame
    void Update()
    {
     bgrenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0); 
    }
}
