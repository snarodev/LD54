using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField
{
	public List<Tile> tiles = new List<Tile>();

	private Action<Tile> cbTileChanged;
	private Action cbSlide;
	private Action cbGoal;

	private int nextTileId = 1;

	public void GeneratePlayField(LevelData levelData)
	{
		for (int i = 0; i < levelData.tiles.Length; i++)
		{
			if (GetTileAt(levelData.tiles[i].x, levelData.tiles[i].y) == null)
			{
				GenerateTile (levelData.tiles[i]);	
			}
		}
	}

	private Tile GenerateTile(LevelObjectData tileData)
	{
		Tile t = new Tile(nextTileId++, tileData.x, tileData.y, tileData.type, tileData.moveGroup);

		tiles.Add(t);
		cbTileChanged?.Invoke (t);

		return t;
	}

	public void Slide(Vector2Int direction, int moveGroup)
	{
		bool canMove = true;

		Vector2Int moveAmount = Vector2Int.zero;

		int currentMoveStep = 0;

		bool goalReached = false;

		// Figure out how far the tiles can be moved.
		while (canMove && !goalReached && currentMoveStep < 20)
		{
			moveAmount += direction;

			for (int i = 0; i < tiles.Count; i++)
			{
				if (tiles[i].type == LevelObjectType.Pushable &&
					tiles[i].slideGroup == moveGroup)
				{
					// Check if the tile can be moved in the specified direction

					Tile t = GetTileAt(tiles[i].x + moveAmount.x, tiles[i].y + moveAmount.y);

					if (t != null)
					{
						if (t.type == LevelObjectType.Obstacle|| 
							(t.type == LevelObjectType.Pushable && t.slideGroup != moveGroup))
						{
							canMove = false;
							//goalReached = false;
							break;
						}
						else if (t.type == LevelObjectType.Goal)
						{
							goalReached = true;
						}
					}
				}
				 
			}

			//if (!canMove)
			//{
				
			//}

			//if (goalReached)
			//{
			//	canMove = false;
			//}

			currentMoveStep++;
		}

		moveAmount -= direction;


		if (moveAmount.magnitude > 0)
		{
			// Actually move them
			for (int i = 0; i < tiles.Count; i++)
			{
				if (tiles[i].type == LevelObjectType.Pushable &&
					tiles[i].slideGroup == moveGroup)
				{
					tiles[i].x += moveAmount.x;
					tiles[i].y += moveAmount.y;
					cbTileChanged?.Invoke(tiles[i]);
				}
			}

			cbSlide?.Invoke();
		}

		if (goalReached)
		{
			cbGoal();
			
			
		}
		
	}

	public Tile GetTileAt(int x, int y)
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i].x == x &&
				tiles[i].y == y)
			{
				return tiles[i];
			}
		}

		return null;
	}

	public Tile GetTile(int tileId)
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			if (tiles[i].tileId == tileId)
			{
				return tiles[i];
			}
		}

		return null;
	}

	public void RegisterTileChangedCallback (Action<Tile> cb)
	{
		cbTileChanged += cb;
	}

	public void UnregisterTileChangedCallback(Action<Tile> cb)
	{
		cbTileChanged -= cb;
	}

	public void RegisterSlideCallback(Action cb)
	{
		cbSlide += cb;
	}

	public void UnregisterSlideCallback(Action cb)
	{
		cbSlide -= cb;
	}

	public void RegisterOnGoalCallback (Action cb)
	{
		cbGoal += cb;
	}
	public void UnregisterOnGoalCallback(Action cb)
	{
		cbGoal -= cb;
	}
}
