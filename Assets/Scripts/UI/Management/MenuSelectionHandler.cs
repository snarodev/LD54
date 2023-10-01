using UnityEngine;
using UnityEngine.EventSystems;

namespace LoopIt.Client
{
	public class MenuSelectionHandler : MonoBehaviour
	{
		public static MenuSelectionHandler Instance;

		private InputReader _inputReader;
		public GameObject currentSelection;
		public GameObject mouseSelection;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			_inputReader = InputReader.input;

			_inputReader.MenuMouseMoveEvent += HandleMoveCursor;
			_inputReader.MoveSelectionEvent += HandleMoveSelection;
		}

		private void OnDestroy()
		{
			_inputReader.MenuMouseMoveEvent -= HandleMoveCursor;
			_inputReader.MoveSelectionEvent -= HandleMoveSelection;
		}


		/// <summary>
		/// Fired by keyboard and gamepad inputs. Current selected UI element will be the ui Element that was selected
		/// when the event was fired. The _currentSelection is updated later on, after the EventSystem moves to the
		/// desired UI element, the UI element will call into UpdateSelection()
		/// </summary>
		private void HandleMoveSelection()
		{
			Cursor.visible = false;

			// Handle case where no UI element is selected because mouse left selectable bounds
			if (EventSystem.current.currentSelectedGameObject == null)
				EventSystem.current.SetSelectedGameObject(currentSelection);
		}

		private void HandleMoveCursor()
		{
			if (mouseSelection != null)
			{
				EventSystem.current.SetSelectedGameObject(mouseSelection);
			}

			//EventSystem.current.SetSelectedGameObject(null);
			Cursor.visible = true;
		}

		public void HandleMouseEnter(GameObject UIElement)
		{
			mouseSelection = UIElement;
			EventSystem.current.SetSelectedGameObject(UIElement);
		}

		public void HandleMouseExit(GameObject UIElement)
		{
			if (EventSystem.current.currentSelectedGameObject != UIElement)
				return;

			// deselect UI element if mouse moves away from it
			mouseSelection = null;
			//EventSystem.current.SetSelectedGameObject(null);
		}

		/// <summary>
		/// Method interactable UI elements should call on Submit interaction to determine whether to continue or not.
		/// </summary>
		/// <returns></returns>
		//public bool AllowsSubmit()
		//{
		//	// if LMB is not down, there is no edge case to handle, allow the event to continue
		//	return !_inputReader.LeftMouseDown()
		//		   // if we know mouse & keyboard are on different elements, do not allow interaction to continue
		//		   || mouseSelection != null && mouseSelection == currentSelection;
		//}

		/// <summary>
		/// Fired by gamepad or keyboard navigation inputs
		/// </summary>
		/// <param name="UIElement"></param>
		public void UpdateSelection(GameObject UIElement) => currentSelection = UIElement;

		// Debug
		// private void OnGUI()
		// {
		//	 	GUILayout.Box($"_currentSelection: {(_currentSelection != null ? _currentSelection.name : "null")}");
		//	 	GUILayout.Box($"_mouseSelection: {(_mouseSelection != null ? _mouseSelection.name : "null")}");
		// }
	}
}