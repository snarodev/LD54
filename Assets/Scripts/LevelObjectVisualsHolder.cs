using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectVisualsHolder : MonoBehaviour
{
    public static LevelObjectVisualsHolder Instance;

	[SerializeField]
	private List<LevelVisualData> levelVisualDatas = new List<LevelVisualData>();

	[SerializeField]
	private List<Material> slideGroupMaterials = new List<Material>();

	private void Awake()
	{
		Instance = this; 
	}

	public GameObject GetLevelVisualPrefab(LevelObjectType type)
	{
		for (int i = 0; i < levelVisualDatas.Count; i++)
		{
			if (levelVisualDatas[i].type == type)
			{
				return levelVisualDatas[i].prefab;	
			}
		}

		return null;
	}

	public Material GetSlideGroupMaterial(int slideGroup)
	{
		return slideGroupMaterials[slideGroup];
	}

	[System.Serializable]
	public class LevelVisualData
	{
		public LevelObjectType type;
		public GameObject prefab;
	}
}
