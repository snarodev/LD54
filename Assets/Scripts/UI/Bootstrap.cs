using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	private void Start()
	{
		InputReader.input.EnableMenuInput();

		UIManager.manager.OpenPanel<MainMenuPanel>(null, false);
	}
}
