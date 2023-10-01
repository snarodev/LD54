using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressController : MonoBehaviour
{
	public static LevelProgressController instance;

	public LevelData[] levels;

	private int currentPlayingLevel = -1;

	private List<int> completedLevels = new List<int>();

	private void Awake()
	{
		instance = this;
	}

	public LevelData GetLevel(int index)
	{
		return levels[index];
	}

	public void SetCurrentPlayingLevel(int levelId)
	{
		currentPlayingLevel = levelId;
	}

	public int GetCurrentPlayingLevel()
	{
		return currentPlayingLevel;
	}

	public bool HasCompletedLevel(int levelId)
	{
		return completedLevels.Contains(levelId);
	}

	public void MarkLevelCompleted (int levelId)
	{
		completedLevels.Add(levelId);
	}

	public int LevelCount()
	{
		return levels.Length;
	}
}
