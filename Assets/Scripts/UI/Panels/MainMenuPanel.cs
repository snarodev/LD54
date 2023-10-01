using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : Panel
{
	public override void Open(PanelOpenInfo panelOpenInfo)
	{
		
	}

	public void StartGameButton()
	{
		UIManager.manager.CloseCurrentPanel();

		SoundManager.instance.PlayButtonSound();

		UIManager.manager.OpenPanel<LevelSelectionPanel>(null, false);
	}

	public void HowToPlayButton()
	{
		SoundManager.instance.PlayButtonSound();
		UIManager.manager.OpenPanel<HowToPlayPanel>(null, true);
	}
}
