public class DungeonLayout
{

	private DungeonRoom[,] rooms;

	public (int x, int y) size { get; set; }
	public (int x, int y) startingRoomLocation { get; set; }
	public (int x, int y) bossRoomLocation { get; set; }

	public DungeonLayout(int width, int height)
	{
		rooms = new DungeonRoom[width, height];
		size = (width, height);
	}

	public DungeonRoom Room(int x, int y)
	{
		if (rooms[x, y] == null)
		{
			DungeonRoom room = new DungeonRoom();
			room.x = x; 
			room.y = y;
			return room;
		}
		return rooms[x,y];
	}

	public void setRoom(int x, int y, DungeonRoom room)
	{
		room.x = x;
		room.y = y;
		rooms[x,y] = room;
	}

	public bool canTravel(DungeonRoom room, Direction direction) {
		switch (direction)
		{
			case Direction.North:
				return room.y + 1 < size.y && room.door.north && Room(room.x, room.y + 1).door.south;
			case Direction.East:
                return room.x + 1 < size.x && room.door.east && Room(room.x + 1, room.y).door.west;
            case Direction.South:
                return room.y - 1 >= 0 && room.door.south && Room(room.x, room.y - 1).door.north;
            case Direction.West:
                return room.x - 1 >= 0 && room.door.west && Room(room.x - 1, room.y).door.east;
			default: 
				return false;
        }
	}
}
