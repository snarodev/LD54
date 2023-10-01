using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, GameInput.IGameplayActions, GameInput.IMenuActions, GameInput.ILevelSelectionActions
{
	public static InputReader input;

	private GameInput gameInput;

	// Gameplay
	public Action<Vector2> slideEvent { get; set; } = default;
	public Action<Vector2> mouseMoveEvent { get; set; } = default;
	public Action<bool> mouseClickEvent { get; set; } = default;

	// Menus
	public Action MoveSelectionEvent { get; set; } = default;
	public Action MenuMouseMoveEvent { get; set; } = default;
	public Action MenuConfirmEvent { get; set; } = default;
	public Action MenuCancelEvent { get; set; } = default;

	// Level Selection
	public Action<Vector2> LevelSelectionMoveEvent { get; set; } = default;

	private bool pointerOverUI;


	private void Awake()
	{
		input = this;

		if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Menu.SetCallbacks(this);
			gameInput.Gameplay.SetCallbacks(this);
			gameInput.LevelSelection.SetCallbacks(this);
		}
	}

	private void Update()
	{
		pointerOverUI = EventSystem.current.IsPointerOverGameObject();
	}

	#region InputMapControl

	public void EnableGameplayInput()
	{
		gameInput.Menu.Disable();
		gameInput.LevelSelection.Disable();
		gameInput.Gameplay.Enable();
	}

	public void EnableMenuInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.LevelSelection.Disable();
		gameInput.Menu.Enable();
	}

	public void EnableLevelSelectionInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.LevelSelection.Enable();
	}

	public void DisableAllInput()
	{
		gameInput.Gameplay.Disable();
		gameInput.Menu.Disable();
	}

	#endregion

	#region Gameplay


	public void OnSlide(InputAction.CallbackContext context)
	{
		Vector2 value = context.ReadValue<Vector2>();

		if (context.phase == InputActionPhase.Performed)
		{
			slideEvent.Invoke(value);
		}
	}



	public void OnMouseMovePosition(InputAction.CallbackContext context)
	{
		Vector2 mousePos = context.ReadValue<Vector2>();

		mouseMoveEvent?.Invoke(mousePos);
	}

	public void OnMouseClick(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			mouseClickEvent?.Invoke(pointerOverUI);
		}
	}

	#endregion

	#region LevelSelection

	public void OnLevelSelectionMove(InputAction.CallbackContext context)
	{
		Vector2 direction = context.ReadValue<Vector2>();

		if (context.phase == InputActionPhase.Performed)
		{
			LevelSelectionMoveEvent?.Invoke(direction);
		}
	}


	#endregion

	#region MenuInput

	public void OnCancel(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			MenuCancelEvent?.Invoke();
		}
	}

	public void OnConfirm(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			MenuConfirmEvent?.Invoke();
		}
	}

	public void OnMouseMove(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			MenuMouseMoveEvent?.Invoke();
		}
	}

	public void OnMoveSelection(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Performed)
		{
			MoveSelectionEvent?.Invoke();
		}
	}

	#endregion


}
