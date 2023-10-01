using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Panel : MonoBehaviour
{
	public Selectable initialSelected;

	protected PanelState panelState;

	private Dictionary<Selectable, bool> originalSelectableInteractableState;

	public abstract void Open(PanelOpenInfo panelOpenInfo);
	public virtual void Close()
	{
		if (panelState != PanelState.Active)
			return;

		UIManager.manager.RemovePanelFromStack();

		Destroy(gameObject);
	}
	public virtual PanelState GetPanelState() => panelState;

	public void ChangeFocus(bool inFocus)
	{
		if (!inFocus)
		{
			// Blur
			Selectable[] selectables = GetComponentsInChildren<Selectable>();

			originalSelectableInteractableState = new Dictionary<Selectable, bool>();

			for (int i = 0; i < selectables.Length; i++)
			{
				originalSelectableInteractableState.Add(selectables[i], selectables[i].interactable);

				selectables[i].interactable = false;
			}
		}
		else
		{
			// Focus

			if (originalSelectableInteractableState == null)
			{
				Selectable[] selectables = GetComponentsInChildren<Selectable>();
				for (int i = 0; i < selectables.Length; i++)
				{
					selectables[i].gameObject.AddComponent<MultiInputSelectableElement>();
				}
			}
			else
			{
				if (originalSelectableInteractableState.Count != 0)
				{
					foreach (KeyValuePair<Selectable, bool> item in originalSelectableInteractableState)
					{
						item.Key.interactable = item.Value;
					}

					originalSelectableInteractableState.Clear();
				}
			}
		}
	}
}