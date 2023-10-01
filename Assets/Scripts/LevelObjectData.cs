using System;

[Serializable]
public class LevelObjectData
{
	public int x;
	public int y;

	public LevelObjectType type;
	public int moveGroup;

	public LevelObjectData(int x, int y, LevelObjectType type, int moveGroup)
	{
		this.x = x;
		this.y = y;
		this.type = type;
		this.moveGroup = moveGroup;
	}
}

public enum LevelObjectType
{
	Obstacle,
	Pushable,
	Goal
}