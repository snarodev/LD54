using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePanel : Panel
{
	[SerializeField]
	private TMPro.TMP_Text titleText;

	[SerializeField]
	private TMPro.TMP_Text slideAmountText;
	[SerializeField]
	private TMPro.TMP_Text timeText;

	[SerializeField]
	private GameObject nextLevelButton;

	public override void Open(PanelOpenInfo panelOpenInfo)
	{
		LevelCompletePanelOpenInfo info = (LevelCompletePanelOpenInfo)panelOpenInfo;

		titleText.text = $"Level {info.levelName} Complete";
		slideAmountText.text = $"Slides: {info.slideAmount}";
		timeText.text = $"Time: {Mathf.RoundToInt(info.time)}s";

		if (LevelProgressController.instance.GetCurrentPlayingLevel() + 1 >= LevelProgressController.instance.LevelCount())
		{
			nextLevelButton.SetActive (false);
		}

		StartCoroutine(OpenAnim());
	}

	IEnumerator OpenAnim()
	{
		transform.localScale = Vector3.zero;
		yield return new WaitForSeconds(0.5f);


		for (float i = 0; i < 1; i+= 0.1f)
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, i);

			yield return 0;
		}

		transform.localScale = Vector3.one;
	}

	public class LevelCompletePanelOpenInfo : PanelOpenInfo
	{
		public string levelName;
		public int slideAmount;
		public float time;
	}

	public void BackButton()
	{
		UIManager.manager.CloseCurrentPanel();

		UIManager.manager.OpenPanel<LevelSelectionPanel>(null, false);

		LevelController.Instance.ClearLevel();

		SoundManager.instance.PlayButtonSound();

		LevelProgressController.instance.SetCurrentPlayingLevel(-1);

		InputReader.input.EnableLevelSelectionInput();
	}

	public void RetryButton()
	{
		InputReader.input.EnableGameplayInput();

		int currentLevel = LevelProgressController.instance.GetCurrentPlayingLevel();

		SoundManager.instance.PlayButtonSound();

		LevelProgressController.instance.SetCurrentPlayingLevel(currentLevel);

		LevelController.Instance.LoadLevel(LevelProgressController.instance.levels[currentLevel]);

		UIManager.manager.CloseCurrentPanel();
	}

	public void NextLevelButton()
	{
		InputReader.input.EnableGameplayInput();

		SoundManager.instance.PlayButtonSound();

		int nextLevel = LevelProgressController.instance.GetCurrentPlayingLevel() + 1;

		LevelProgressController.instance.SetCurrentPlayingLevel(nextLevel);

		LevelController.Instance.LoadLevel(LevelProgressController.instance.levels[nextLevel]);

		UIManager.manager.CloseCurrentPanel();
	}

}
