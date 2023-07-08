using UnityEngine;

public class DungeonRoom
{

    [SerializeField] GameObject roomGameObject;

    public (bool north, bool east, bool south, bool west) door { get; set; }
    
    public int rotation { get; set; } // 0, 1, 2, 3 (clockwise)
    public int x { get; set; }
    public int y { get; set; }

    public DungeonRoom()
    {
    }

    public DungeonRoom(int r)
    {
        rotation = r;
    }

    public DungeonRoom(int x, int y, int r, bool n, bool e, bool s, bool w)
    {
        this.x = x;
        this.y = y;
        rotation = r;
        door = (n, e, s, w);
    }

    public override string ToString()
    {
        return "x: " + x + ", y: " + y + ", rotation: " + rotation + ", North: " + door.north + ", East: " + door.east + ", South: " + door.south + ", West: " + door.west;
    }
}
