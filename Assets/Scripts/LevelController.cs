using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

	[SerializeField]
	private TMPro.TMP_Text slideAmountText;

	[SerializeField]
	private TMPro.TMP_Text currentLevelText;

	[SerializeField]
	private GameObject backButton;

	public PlayField playField;

	private List<TileVisual> tileVisuals = new List<TileVisual>();

	private int slideAmount;
	private float levelStartTime;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		ClearLevel();
	}

	private void Update()
	{
		if (playField == null)
			return;

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 10, -5), 0.1f);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(70, 0, 0), 0.1f);
	}

	public void LoadLevel(LevelData levelData)
    {
		ClearLevel();

		playField = new PlayField();
		playField.RegisterTileChangedCallback(OnTileChanged);
		playField.RegisterSlideCallback(OnSlide);
		playField.RegisterOnGoalCallback(OnGoal);

		playField.GeneratePlayField(levelData);

		slideAmountText.gameObject.SetActive(true);
		currentLevelText.gameObject.SetActive(true);

		currentLevelText.text = $"Level: {LevelProgressController.instance.GetCurrentPlayingLevel() + 1}";

		levelStartTime = Time.time;

		backButton.SetActive(true);
	}

	private void OnGoal()
	{
		backButton.SetActive(false);
		InputReader.input.EnableMenuInput();
		UIManager.manager.OpenPanel<LevelCompletePanel>(new LevelCompletePanel.LevelCompletePanelOpenInfo() { slideAmount = slideAmount, time = Time.time - levelStartTime, levelName = (LevelProgressController.instance.GetCurrentPlayingLevel() + 1).ToString()}, false);
	}

	private void OnSlide()
	{
		slideAmount++;
		UpdateSlideText();
	}

	private void UpdateSlideText()
	{

		slideAmountText.text = $"Slides: {slideAmount}";
	}

	private void OnTileChanged(Tile tile)
	{
		TileVisual tileVisual = GetTileVisual(tile.tileId);

		if (tileVisual == null)
		{
			tileVisual = CreateTileVisual(tile);
		}

		tileVisual.UpdateVisual(tile);
	}

	private TileVisual GetTileVisual (int tileId)
	{
		for (int i = 0; i < tileVisuals.Count; i++)
		{
			if (tileVisuals[i].tileId == tileId)
			{
				return tileVisuals[i];
			}
		}

		return null;
	}

	private TileVisual CreateTileVisual(Tile tile)
	{
		GameObject go = new GameObject($"LevelObject{tile.x}-{tile.y}", typeof(TileVisual));

		TileVisual tileVisual = go.GetComponent<TileVisual>();

		tileVisual.SetUp(tile);

		tileVisuals.Add(tileVisual);

		return tileVisual;
	}

	public void ClearLevel()
	{
		slideAmount = 0;
		UpdateSlideText();
		slideAmountText.gameObject.SetActive(false);
		currentLevelText.gameObject.SetActive(false);
		backButton.SetActive(false);

		//while (transform.childCount > 0)
		//{
		//	Destroy (transform.GetChild(0).gameObject);
		//}

		while (tileVisuals.Count > 0)
		{
			var tileVisual = tileVisuals[0];
			Destroy(tileVisual.gameObject);
			tileVisuals.RemoveAt(0);
		}

		playField = null;
	}

	public void BackButton()
	{
		UIManager.manager.OpenPanel<LevelSelectionPanel>(null, false);
		ClearLevel();

		InputReader.input.EnableLevelSelectionInput();
	}
}
