using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ObjectSelector2D : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private LayerMask selectableLayers; // Какие слои можно выбирать
    [SerializeField] private bool autoAssignMainCamera = true;
    [SerializeField] private bool clearSelectionOnMissClick = true;

    private Camera _camera;

    private void Awake()
    {
        if (autoAssignMainCamera)
        {
            _camera = Camera.main;
        }
        else
        {
            _camera = GetComponent<Camera>();
        }
    }

    private void Update()
    {
        //if (SelectionManager.Instance.HasSelectedObject())
        //{
        //    Debug.Log($"Selection is {SelectionManager.Instance.GetSelectedObject().name}");
        //}
        //else
        //{
        //    Debug.Log($"Selection is empty");
        //}

        if (Input.GetMouseButtonDown(0)) // Левый клик - выбор объекта
        {
            SelectObject();

        }
    }

    private void SelectObject()
    {
        var clickedObject = GetObjectUnderMouse();

        if (clickedObject != null)
        {
            //SelectionManager.Instance.SetSelectedObject(clickedObject);
            SelectionManager.Select( clickedObject);
            Debug.Log($"Selected: {clickedObject.name}");
        }
        else if (clearSelectionOnMissClick)
        {
            //SelectionManager.Instance.ClearSelection();
            Debug.Log("Selection cleared");
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, selectableLayers);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}