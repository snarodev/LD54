using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public List<Panel> panelPrefabs;

	private Stack<Panel> panels = new Stack<Panel>();
	private Stack<bool> enableGeneralCloseButtonStack = new Stack<bool>();

	public GameObject panelParent;

	public static UIManager manager;

	public Transform generalCloseButton;

	private InputReader inputReader;

	private void Awake()
	{
		manager = this;
		generalCloseButton.gameObject.SetActive(false);

		foreach (Transform child in panelParent.transform)
		{
			if (child != generalCloseButton)
			{
				Destroy(child.gameObject);
			}
		}
	}

	private void Start()
	{
		inputReader = InputReader.input;
		inputReader.MenuCancelEvent += OnMenuCancel;
	}

	public void OpenPanel<T>(PanelOpenInfo panelOpenInfo, bool enableGeneralCloseButton) where T : Panel
	{
		for (int i = 0; i < panelPrefabs.Count; i++)
		{
			if (panelPrefabs[i].GetType() == typeof(T))
			{
				OpenPanel(panelPrefabs[i], panelOpenInfo, enableGeneralCloseButton);

				return;
			}
		}


		Debug.LogError("UI - Can't open panel of type " + typeof(T).ToString());

	}

	private void OpenPanel(Panel panelPrefab, PanelOpenInfo panelOpenInfo, bool enableGeneralCloseButton)
	{
		// Disable interactions with previous panel
		// If any are already open
		if (panels.Count != 0)
		{
			panels.Peek().ChangeFocus(false);
		}


		// Activate or deactivate general close button
		// Push state to stack and set as last sibling
		generalCloseButton.gameObject.SetActive(enableGeneralCloseButton);
		generalCloseButton.SetAsLastSibling();
		enableGeneralCloseButtonStack.Push(enableGeneralCloseButton);

		// Then actually create the panel ...
		GameObject go = Instantiate(panelPrefab.gameObject, panelParent.transform);
		Panel panel = go.GetComponent<Panel>();

		// Set the currently selected
		if (panel.initialSelected == null)
		{
			EventSystem.current.SetSelectedGameObject(panel.gameObject.GetComponentInChildren<Selectable>()?.gameObject);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(panel.initialSelected?.gameObject);
		}

		// ... and push it on the stack
		panels.Push(panel);

		// Run open logic of panel
		panel.Open(panelOpenInfo);
		panel.ChangeFocus(true);
	}

	public void RemovePanelFromStack()
	{
		// Pop the state of the closed panel
		panels.Pop();
		enableGeneralCloseButtonStack.Pop();

		// Check if any more panels are open
		if (panels.Count == 0)
		{
			// If not disable the general close button ...
			generalCloseButton.gameObject.SetActive(false);
		}
		else
		{
			// ... Otherwise put the general close button in the correct order (behind the now top panel) ...
			generalCloseButton.gameObject.SetActive(enableGeneralCloseButtonStack.Peek());
			generalCloseButton.SetAsLastSibling();

			//  ... And reactivate the now top panel
			panels.Peek().ChangeFocus(true);
			panels.Peek().transform.SetAsLastSibling();

			// Set the currently selected
			if (panels.Peek().initialSelected == null)
			{
				EventSystem.current.SetSelectedGameObject(panels.Peek().gameObject.GetComponentInChildren<Selectable>()?.gameObject);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(panels.Peek().initialSelected?.gameObject);
			}
		}
	}

	private void OnMenuCancel()
	{
		if (!enableGeneralCloseButtonStack.Peek())
		{
			return;
		}

		CloseCurrentPanel();
	}

	/// <summary>
	/// Called by general Close Button of Canvas
	/// </summary>
	public void GeneralCloseButton()
	{
		if (!enableGeneralCloseButtonStack.Peek())
		{
			return;
		}

		CloseCurrentPanel();
	}

	public void CloseCurrentPanel()
	{
		panels.Peek().Close();
	}

	public bool AnyPanelsOpen()
	{
		return panels.Count != 0;
	}
}

public enum PanelState
{
	Active,
	Opening,
	Closing,
	Inactive
}