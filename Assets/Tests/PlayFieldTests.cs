using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayFieldTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayFieldSimpleMoveTest()
    {
        PlayField playField = new PlayField();

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

		levelData.tiles = new LevelObjectData[] {
            new LevelObjectData (0,0,LevelObjectType.Obstacle,0),
            new LevelObjectData (3,0,LevelObjectType.Pushable,0),
        };

        playField.GeneratePlayField(levelData);

		Tile pushable = playField.GetTileAt(1, 0);
        Assert.IsNull(pushable);

		pushable = playField.GetTileAt(3, 0);
		Assert.IsNotNull(pushable);

		playField.Slide(new Vector2Int(-1, 0), 0);

        pushable = playField.GetTileAt(1, 0);

        Assert.IsNotNull(pushable);
    }

	[Test]
	public void PlayFieldSimpleMoveGoalTest()
    {
		PlayField playField = new PlayField();

		LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

		levelData.tiles = new LevelObjectData[] {
			new LevelObjectData (0,0,LevelObjectType.Goal,0),
			new LevelObjectData (6,0,LevelObjectType.Obstacle,0),
			new LevelObjectData (3,0,LevelObjectType.Pushable,0),
		};

		playField.GeneratePlayField(levelData);

        bool goalRegistered = false;
        playField.RegisterOnGoalCallback(() =>
        {
            goalRegistered = true;
        });


		playField.Slide(new Vector2Int(1, 0), 0);

        Assert.IsFalse(goalRegistered);


		playField.Slide(new Vector2Int(-1, 0), 0);

        Assert.IsTrue(goalRegistered);
	}
}
