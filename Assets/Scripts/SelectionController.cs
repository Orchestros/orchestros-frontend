using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Controller allowing to select one or many objets with the "SelectableObject" tag.
/// </summary>
public class SelectionController : MonoBehaviour
{
    // The texture passed to the Highlighted component.
    public Texture selectedTexture;

    // An already existing panel that should be a child of the main canvas. It should
    // be disabled at first.
    public GameObject selectionPanel;
    private RectTransform _panelRectTransform;


    private Rect _rectParent;
    private Camera _camera;

    private bool _isCurrentlySelecting;

    private Vector3 _startHitPoint;
    private Vector2 _startPointCanvas;


    private readonly Dictionary<GameObject, Highlighted> _selectedObjects = new();

    /// <summary>
    ///     Retrieves expansive objects at initialization.
    /// </summary>
    private void Start()
    {
        _rectParent = transform.GetComponent<RectTransform>().rect;
        _camera = Camera.main;
        _panelRectTransform = selectionPanel.transform.GetComponent<RectTransform>();
    }

    /// <summary>
    ///     The core controller of the selection process. The SelectionController can be in different state described bellow.
    ///
    /// <para>If the left button is clicked, sends a raycast and retrieves the objet it collided with. If the object possesses the
    /// tag SelectableObject, selects it. Otherwise, the panel selection process allowing to select multiple objects at the same time is launched.
    /// Essentially, this means that the _isCurrentlySelecting is set to true. At the same time the _startPointCanvas and _startHitPoint are saved for later use by the UpdateSelection
    /// and EndSelection method.
    /// </para>
    ///
    /// <para>
    ///     If _isCurrentlySelecting is set to true, then simply updates the selection panel to follow the mouse.
    /// </para>
    ///
    /// <para>
    ///     If the left mouse button is lifted, add all the selectable objets under the selection panel to the selected
    /// objets list.
    /// </para>
    ///
    /// <para>Please note that if the shift key is pressed, the selected objets list will not be cleared.</para>
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked();
        }


        if (_isCurrentlySelecting)
        {
            if (Input.GetMouseButtonUp(0))
            {
                EndSelection();
            }
            else
            {
                UpdateSelection();
            }
        }
    }

    /// <summary>
    ///  Launches either the selection process or directly selects the object under the mouse.
    /// </summary>
    private void OnMouseClicked()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit) && !_isCurrentlySelecting)
        {
            var colliderGameObject = hit.collider.gameObject;
            ClearSelectionIfNeeded();

            if (colliderGameObject.CompareTag("SelectableObject"))
            {
                ToggleObjectSelection(colliderGameObject);
            }
            else
            {
                _startHitPoint = hit.point;
                StartSelection();
            }
        }
    }

    /// <summary>
    ///  Clear the selection objects only if the left shift key is not pressed.
    /// </summary>
    private void ClearSelectionIfNeeded()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var selectedObject in _selectedObjects.Keys.ToList())
            {
                ToggleObjectSelection(selectedObject);
            }

            _selectedObjects.Clear();
        }
    }

    /// <summary>
    ///     Adds/Removes the colliderGameObject from the selected objects dictionary. The value associated to the
    ///     object (which acts as the key) correspond to the Highlighted component added to the object. Allowing
    ///     for an optimize deletion once the object is not selected anymore.
    /// </summary>
    /// <param name="colliderGameObject">The object to remove or add to the selected objects dictionary</param>
    private void ToggleObjectSelection(GameObject colliderGameObject)
    {
        if (_selectedObjects.ContainsKey(colliderGameObject))
        {
            Destroy(_selectedObjects[colliderGameObject]);
            _selectedObjects.Remove(colliderGameObject);
        }
        else
        {
            var highlighted = colliderGameObject.AddComponent<Highlighted>();
            highlighted.selectedTexture = selectedTexture;
            _selectedObjects[colliderGameObject] = highlighted;
        }
    }

    /// <summary>
    ///     Mark all the objects under the selection panel as being selected.
    /// </summary>
    private void EndSelection()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var endHitPoint))
        {
            var selectableUnits = GameObject.FindGameObjectsWithTag("SelectableObject");

            var maxX = Math.Max(endHitPoint.point.x, _startHitPoint.x);
            var maxY = Math.Max(endHitPoint.point.z, _startHitPoint.z);

            var minX = Math.Min(endHitPoint.point.x, _startHitPoint.x);
            var minY = Math.Min(endHitPoint.point.z, _startHitPoint.z);

            foreach (var unit in selectableUnits)
            {
                if (maxX >= unit.transform.position.x && unit.transform.position.x >= minX &&
                    maxY >= unit.transform.position.z && unit.transform.position.z >= minY)
                {
                    ToggleObjectSelection(unit);
                }
            }
        }

        selectionPanel.SetActive(false);
        _isCurrentlySelecting = false;
    }

    /// <summary>
    ///     Resizes the selection panel according to the start mouse point and current mouse point.
    /// </summary>
    private void UpdateSelection()
    {
        _panelRectTransform.offsetMin = new Vector2(
            Math.Min(Input.mousePosition.x, _startPointCanvas.x),
            Math.Min(Input.mousePosition.y, _startPointCanvas.y)
        );
        _panelRectTransform.offsetMax = new Vector2(
            Math.Max(Input.mousePosition.x, _startPointCanvas.x),
            Math.Max(Input.mousePosition.y, _startPointCanvas.y)
        ) - _rectParent.size;
    }

    /// <summary>
    /// Starts the selection process
    /// </summary>
    private void StartSelection()
    {
        _isCurrentlySelecting = true;
        _startPointCanvas = Input.mousePosition;
        selectionPanel.SetActive(true);
    }
}