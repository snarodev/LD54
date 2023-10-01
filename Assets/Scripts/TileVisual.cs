using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
	public int tileId;

	private LevelObjectType type;

	private Vector3 targetPos;

	public void SetUp(Tile tile)
	{
		this.type = tile.type;
		this.tileId = tile.tileId;

		GameObject prefab = LevelObjectVisualsHolder.Instance.GetLevelVisualPrefab(type);
		targetPos = transform.position;

		if (prefab != null)
		{
			GameObject go = Instantiate (prefab, transform);

			go.transform.localPosition = Vector3.zero;

			if (type == LevelObjectType.Pushable)
			{
				MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();

				mr.material = LevelObjectVisualsHolder.Instance.GetSlideGroupMaterial(tile.slideGroup);
			}
		}
	}

	public void UpdateVisual(Tile tile)
	{
		// TODO: Add animations here
		//transform.position = new Vector3(tile.x, 0, tile.y);
		targetPos = new Vector3(tile.x, 0, tile.y);
	}

	private void Update()
	{
		transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
	}
}
