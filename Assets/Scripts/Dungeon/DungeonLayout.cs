using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLayout
{

	enum Direction
	{
		North,
		East,
		South,
		West
	}

	private DungeonRoom[,] rooms;
	public int gridWidth { get; set; }
	public int gridHeight { get; set; }

	public int startingRoom { get; set; }
	public int bossRoom { get; set; }

	public DungeonLayout(int width, int height)
	{
		rooms = new DungeonRoom[width, height];
		gridWidth = width;
		gridHeight = height;
	}

	public bool isPath(int x, int y, Direction dir)
	{
		return false; //carry on from here :)
	}

	public DungeonRoom getRoom(int x, int y)
	{
		return rooms[x,y];
	}

	public void setRoom(int x, int y, DungeonRoom room)
	{
		rooms[x,y] = room;
	}
}
