using LoopIt.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Responsible for passing pointer hover and exit events to the MenuSelectionHandler to allow seamless switching between controller, keyboard and mouse.
/// Gets automaticly added by the UIManager when opening panels
/// </summary>
public class MultiInputSelectableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
	private MenuSelectionHandler _menuSelectionHandler;

	private void Awake()
	{
		_menuSelectionHandler = MenuSelectionHandler.Instance;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseEnter(gameObject);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_menuSelectionHandler.HandleMouseExit(gameObject);
	}

	public void OnSelect(BaseEventData eventData)
	{
		_menuSelectionHandler.UpdateSelection(gameObject);
	}
}