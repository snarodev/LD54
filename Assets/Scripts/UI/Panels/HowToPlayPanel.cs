using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayPanel : Panel
{
	public override void Open(PanelOpenInfo panelOpenInfo)
	{
		
	}

	public void CloseButton()
	{
		SoundManager.instance.PlayButtonSound();
		UIManager.manager.CloseCurrentPanel();
	}
}
