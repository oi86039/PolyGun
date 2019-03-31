using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_health_gui : MonoBehaviour
{
    public float playerHealth;

    public float bar = 0;
    Vector2 pos = new Vector2(20, 40);
    Vector2 size = new Vector2(60, 20);
    public Texture2D Empty;
    public Texture2D Full;
 
 void OnGUI()
    {
        //Image Box (For empty health)
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), Empty);

        // Actual bar for filled part.:
        GUI.BeginGroup(new Rect(0, 0, size.x * bar, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), Full);
        GUI.EndGroup();

        GUI.EndGroup();

    }

    void Update()
    {
        bar = playerHealth;
    }
}
