using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class EditorLevelCreator : MonoBehaviour
{
    public string levelName = "Level1";

    public void CreateLevelConfiguration()
    {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();


        LevelObject[] levelObjects = GetComponentsInChildren<LevelObject>();
        asset.tiles = new LevelObjectData[levelObjects.Length];

        for (int i = 0; i < levelObjects.Length; i++)
        {
            int x = Mathf.RoundToInt(levelObjects[i].transform.localPosition.x);
            int y = Mathf.RoundToInt(levelObjects[i].transform.localPosition.z);

            asset.tiles[i] = new LevelObjectData(x, y, levelObjects[i].type, levelObjects[i].moveGroup);

		}


        AssetDatabase.CreateAsset(asset, "Assets/" + levelName + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public void LoadLevelConfiguration()
    {
		foreach (Transform child in transform)
		{
            DestroyImmediate (child.gameObject);
			//Destroy(child.gameObject);
		}

		LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/" + levelName + ".asset");

        for (int i = 0;i < levelData.tiles.Length;i++)
        {
            //GameObject go = new GameObject("LevelObject", typeof(LevelObject));
            GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
            go.name = $"T:{levelData.tiles[i].type} X:{levelData.tiles[i].x} Y:{levelData.tiles[i].y}";

            LevelObject levelObject = go.AddComponent<LevelObject>();

            go.transform.parent = transform;

            levelObject.transform.localPosition = new Vector3(levelData.tiles[i].x, i * 0.1f, levelData.tiles[i].y);
            levelObject.type = levelData.tiles[i].type;
            levelObject.moveGroup = levelData.tiles[i].moveGroup;

            if (levelData.tiles[i].type == LevelObjectType.Goal)
            {
                go.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);
            }
            else if (levelData.tiles[i].type == LevelObjectType.Pushable)
            {
                go.transform.localScale = new Vector3(0.8f, 0.05f, levelData.tiles[i].moveGroup * 0.4f + 0.2f);
            }
            else
            {
				go.transform.localScale = new Vector3(1, 0.05f, 1);
			}

			//GameObject visuals = Instantiate (LevelObjectVisualsHolder.Instance.get)
		}
    }

	private void Start()
	{
        Destroy(gameObject);
	}
}

#endif