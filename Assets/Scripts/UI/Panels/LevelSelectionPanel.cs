using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionPanel : Panel
{
	

	[SerializeField]
	private GameObject levelSelectionPointPrefab;

	[SerializeField]
	private GameObject levelSelectionPointIndicatorPrefab;

	private GameObject levelSelectionPointIndicator;

	private List<GameObject> levelSelectionPoints = new List<GameObject>();

	private Camera cam;

	private int currentSelectedLevelPointIndex = 0;

	public override void Open(PanelOpenInfo panelOpenInfo)
	{
		cam = Camera.main;

		levelSelectionPointIndicator = Instantiate(levelSelectionPointIndicatorPrefab);

		InputReader.input.EnableLevelSelectionInput();

		InputReader.input.LevelSelectionMoveEvent += OnLevelSelectionMove;

		//for (int i = levels.Length - 1; i >= 0 ; i--)
		for (int i = 0; i < LevelProgressController.instance.levels.Length; i++)
		{
			GameObject go = Instantiate(levelSelectionPointPrefab);

			go.transform.position = new Vector3(0, 0, i * 2 + LevelProgressController.instance.levels.Length * 2);

			levelSelectionPoints.Add(go);
		}
	}


	public override void Close()
	{
		base.Close();
		InputReader.input.LevelSelectionMoveEvent -= OnLevelSelectionMove;

		//InputReader.input.EnableMenuInput();


		Destroy(levelSelectionPointIndicator);

		for (int i = 0; i < levelSelectionPoints.Count; i++)
		{
			Destroy(levelSelectionPoints[i].gameObject);
		}
	}

	private void OnLevelSelectionMove(Vector2 direction)
	{
		if (direction.y > 0.1f)
		{
			currentSelectedLevelPointIndex++;
		}
		else if (direction.y < -0.1f)
		{
			currentSelectedLevelPointIndex--;
		}

		currentSelectedLevelPointIndex = Mathf.Clamp(currentSelectedLevelPointIndex, 0, LevelProgressController.instance.levels.Length - 1);

		SoundManager.instance.PlayButtonSound();

		UpdateLevelSelectionPointIndicator();
	}

	private void Update()
	{
		Vector3 targetPos = levelSelectionPoints[currentSelectedLevelPointIndex].transform.position + new Vector3(0, 10, -3);

		cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 0.1f);



		Vector3 levelSelectionPointIndicatorTargetPos = levelSelectionPoints[currentSelectedLevelPointIndex].transform.position + new Vector3(0, 0.8f, 0.8f);
		levelSelectionPointIndicator.transform.position = Vector3.Lerp(levelSelectionPointIndicator.transform.position, levelSelectionPointIndicatorTargetPos, 0.1f);



		//Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 5, -2.8f), 0.1f);
		Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(60, 0, 0), 0.1f);
	}

	private void UpdateLevelSelectionPointIndicator()
	{
		var text = levelSelectionPointIndicator.GetComponentInChildren<TMPro.TMP_Text>();

		if (text == null)
		{
			Debug.LogError("BAD");
		}

		text.text = $"Level {currentSelectedLevelPointIndex + 1}";
	}

	public void PlayLevelButton()
	{
		InputReader.input.EnableGameplayInput();

		LevelProgressController.instance.SetCurrentPlayingLevel(currentSelectedLevelPointIndex);

		LevelController.Instance.LoadLevel(LevelProgressController.instance.levels[currentSelectedLevelPointIndex]);
		SoundManager.instance.PlayButtonSound();

		UIManager.manager.CloseCurrentPanel();
	}

	public void BackButton()
	{
		InputReader.input.EnableMenuInput();
		UIManager.manager.CloseCurrentPanel();
		SoundManager.instance.PlayButtonSound();

		UIManager.manager.OpenPanel<MainMenuPanel>(null,false);
	}
}
