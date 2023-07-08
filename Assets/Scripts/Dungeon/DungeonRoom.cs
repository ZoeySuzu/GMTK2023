using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{

    [SerializeField] GameObject roomGameObject;

    public bool entryNorth { get; set; }
    public bool entryEast { get; set; }
    public bool entrySouth { get; set; }
    public bool entryWest { get; set; }
    
    public int rotation { get; set; } // 0, 1, 2, 3 (clockwise)
    public int xPosition { get; set; }
    public int yPosition { get; set; }

    public DungeonRoom(int x, int y, int r)
    {
        xPosition = x;
        yPosition = y;
        rotation = r;
    }

    public DungeonRoom(int x, int y, int r, bool n, bool e, bool s, bool w)
    {
        xPosition = x;
        yPosition = y;
        rotation = r;
        entryNorth = n;
        entryEast = e;
        entrySouth = s;
        entryWest = w;
    }
}
