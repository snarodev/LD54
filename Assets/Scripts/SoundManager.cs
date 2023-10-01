using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip buttonClickSound;

	public static SoundManager instance;

	private void Awake()
	{
		instance = this;
	}

	public void PlayButtonSound()
	{
		AudioSource.PlayClipAtPoint(buttonClickSound, transform.position);
	}
}
