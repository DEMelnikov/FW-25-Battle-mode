using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private MouseInputController mouseInputController;

    [Header("Drag Movement")]
    [SerializeField] private bool enableDrag = true;
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private bool invertDrag = false;
    [SerializeField] private float dragSmoothing = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private bool enableZoom = true;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomSmoothing = 10f;

    [Header("Movement Boundaries")]
    [SerializeField] private bool useBoundaries = false;
    [SerializeField] private Vector2 minBoundary = new Vector2(-10f, -10f);
    [SerializeField] private Vector2 maxBoundary = new Vector2(10f, 10f);

    // Private variables
    private Vector3 dragOrigin;
    private Vector3 targetPosition;
    private Camera cam;
    private float targetZoom;

    // Touch handling
    private int prevTouchCount = 0;
    private Vector2[] prevTouchPositions = new Vector2[2];

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleInput();
        ApplyMovement();
        ApplyZoom();
    }

    private void HandleInput()
    {
        if (mouseInputController != null && mouseInputController.IsDragging)
            return;


        // Определяем тип управления на основе платформы
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
            HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        // Drag movement
        if (enableDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                ContinueDrag(Input.mousePosition);
            }
        }

        // Mouse wheel zoom
        if (enableZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetZoom -= scroll * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            // Single touch - drag movement
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                StartDrag(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ContinueDrag(touch.position);
            }
        }
        else if (Input.touchCount == 2)
        {
            // Two touches - pinch to zoom
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began ||
                prevTouchCount != 2)
            {
                prevTouchPositions[0] = touch1.position;
                prevTouchPositions[1] = touch2.position;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2[] currentTouchPositions = new Vector2[] { touch1.position, touch2.position };
                HandlePinchZoom(currentTouchPositions);
            }
        }

        prevTouchCount = Input.touchCount;
    }

    private void StartDrag(Vector3 inputPosition)
    {
        dragOrigin = cam.ScreenToWorldPoint(inputPosition);
        dragOrigin.z = targetPosition.z;
    }

    private void ContinueDrag(Vector3 inputPosition)
    {
        Vector3 currentPosition = cam.ScreenToWorldPoint(inputPosition);
        currentPosition.z = targetPosition.z;

        Vector3 difference = dragOrigin - currentPosition;
        if (invertDrag) difference = -difference;

        targetPosition += difference;

        // Обновляем dragOrigin для плавного продолжения drag
        dragOrigin = cam.ScreenToWorldPoint(inputPosition);
        dragOrigin.z = targetPosition.z;
    }

    private void HandlePinchZoom(Vector2[] currentTouchPositions)
    {
        float previousDistance = Vector2.Distance(prevTouchPositions[0], prevTouchPositions[1]);
        float currentDistance = Vector2.Distance(currentTouchPositions[0], currentTouchPositions[1]);

        float pinchAmount = (previousDistance - currentDistance) * 0.01f;
        targetZoom += pinchAmount * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        prevTouchPositions = currentTouchPositions;
    }

    private void ApplyMovement()
    {
        if (useBoundaries)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBoundary.x, maxBoundary.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBoundary.y, maxBoundary.y);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, dragSmoothing * Time.deltaTime);
    }

    private void ApplyZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSmoothing * Time.deltaTime);
    }

    // Public methods for external control
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        if (useBoundaries)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBoundary.x, maxBoundary.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBoundary.y, maxBoundary.y);
        }
    }

    public void SetZoom(float newZoom)
    {
        targetZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    // Gizmos for boundary visualization
    private void OnDrawGizmosSelected()
    {
        if (useBoundaries)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3(
                (minBoundary.x + maxBoundary.x) * 0.5f,
                (minBoundary.y + maxBoundary.y) * 0.5f,
                transform.position.z
            );
            Vector3 size = new Vector3(
                maxBoundary.x - minBoundary.x,
                maxBoundary.y - minBoundary.y,
                0.1f
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}