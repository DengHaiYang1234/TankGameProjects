using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTrackAnimation : MonoBehaviour
{
    Renderer render;
    float scrollSpeed = 0.5f;
    public Vector2 UVDirection = new Vector2(0, 1);
    void Start()
    {
        render = GetComponent<Renderer>();
    }
    
    public void MoveTrack(Vector2 vector)
    {
        if (render == null || render.material == null)
            return;

        float offset = Time.deltaTime * vector.x * scrollSpeed;
         
        render.material.mainTextureOffset += new Vector2(0, offset);
    }

}
