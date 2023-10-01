using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	public int tileId;

	public int x;
	public int y;

	public LevelObjectType type;

	public int slideGroup;

	public Tile(int tileId, int x, int y, LevelObjectType type, int moveGroup)
	{
		this.tileId = tileId;
		this.x = x;
		this.y = y;
		this.type = type;
		this.slideGroup = moveGroup;
	}
}
