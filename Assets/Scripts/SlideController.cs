using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideController : MonoBehaviour
{
	[SerializeField]
	private Renderer currentSlideGroupIndicator;

	[SerializeField]
	private AudioClip slideSound;

	private int currentSlideGroup;

	private Camera cam;

	private Vector2 mousePos;

	private void Start()
	{
		cam = Camera.main;

		InputReader.input.slideEvent += OnSlide;
		InputReader.input.mouseMoveEvent += OnMouseMove;
		InputReader.input.mouseClickEvent += OnMouseClick;
	}

	private void OnDestroy()
	{
		InputReader.input.slideEvent -= OnSlide;
		InputReader.input.mouseMoveEvent -= OnMouseMove;
		InputReader.input.mouseClickEvent -= OnMouseClick;
	}

	private void OnMouseClick(bool overUI)
	{
		if (overUI)
		{
			return;
		}

		Ray ray = cam.ScreenPointToRay(mousePos);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			TileVisual tileVisual = hit.collider.transform.parent.GetComponent<TileVisual>();

			if (tileVisual != null)
			{
				Tile tile = LevelController.Instance.playField.GetTile(tileVisual.tileId);

				if (tile != null)
				{
					currentSlideGroup = tile.slideGroup;

					currentSlideGroupIndicator.material = LevelObjectVisualsHolder.Instance.GetSlideGroupMaterial(currentSlideGroup);
				}
			}
		}
	}

	private void OnMouseMove(Vector2 mousePos)
	{
		this.mousePos = mousePos;
	}

	private void OnSlide(Vector2 vector)
	{
		if (LevelController.Instance.playField == null)
			return;

		// Cheat prevention
		if (vector.y > 0.4f && vector.y > 0.4f)
		{
			vector.x = 0;
			vector.y = 1;
		}

		if (vector.magnitude > 0.01f)
		{
			AudioSource.PlayClipAtPoint(slideSound, transform.position, 1);

			LevelController.Instance.playField.Slide(new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)), currentSlideGroup);
		}
	}
}
