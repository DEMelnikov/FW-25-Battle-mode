using UnityEngine;

public class MouseInputController : MonoBehaviour
{
    private Character selectedHero;
    [SerializeField] private DragShadow dragShadowPrefab; // prefab ��� �������� ����
    private DragShadow dragShadow;
    private bool isDragging = false;
    private Vector3 mouseDownPosition;         // ������� ���� ��� �������
    private const float dragThreshold = 5f;   // ����������� ���������� (� ��������) ��� ������ drag

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryBeginDrag();

        // ���������, �������� �� ����� ��� drag
        if (isDragging)
            UpdateDragShadow();

        if (Input.GetMouseButtonUp(0))
            TryFinishDrag();
    }
    public bool IsDragging => isDragging;

    void TryBeginDrag()
    {
        //Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, selectableLayers);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Character hero = hit.collider.GetComponent<Character>();
            if (hero != null && hero.SceneObjectTag == SceneObjectTag.Hero)
            {
                Debug.LogWarning("Hit Hero");
                selectedHero = hero;
                mouseDownPosition = Input.mousePosition; // ���������� ��������� ������� ����
                isDragging = true;
                // ���� �� ������ ���� � �� ��������� drag! ��� �������� ����
            }
        }
    }

    void UpdateDragShadow()
    {
        // ��������� ��������� ������ ���� ���� �� ��������� �������
        float distance = Vector3.Distance(mouseDownPosition, Input.mousePosition);
        if (distance > dragThreshold && dragShadow == null)
        {
            // ���� ����� �������, �������� drag, ������ ���� �����
            dragShadow = Instantiate(dragShadowPrefab);
        }
        if (dragShadow != null)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.WorldToScreenPoint(selectedHero.transform.position).z;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            dragShadow.transform.position = mouseWorld;
        }
    }

    void TryFinishDrag()
    {
        if (!isDragging) return;

        float distance = Vector3.Distance(mouseDownPosition, Input.mousePosition);
        if (dragShadow == null || distance < dragThreshold)
        {
            // ���� drag �� ����� ��� ���� ����� �� ��������� � ������� ��� ������
            // ��������, ������ �������� ����� ��� ������� ���� ��� waypoint-a
            //SelectHero(selectedHero); // ���� ������ ������
        }
        else
        {
            // Drag ������������� ���� ����� � ������� ������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ICharacter enemy = hit.collider.GetComponent<Character>();
                if (enemy != null && enemy.SceneObjectTag == SceneObjectTag.Enemy)
                {
                    //selectedHero.TargetsVault.AddAttackTarget(enemy);
                    selectedHero.GetTargetsVault().SetTargetEnemyCharacter(enemy);
                }
                else
                {
                    selectedHero.GetTargetsVault().SetWayPoint(hit.point);
                }
            }
        }

        // ������� ���������
        if (dragShadow != null) Destroy(dragShadow.gameObject);
        //dragShadow = null;
        selectedHero = null;
        isDragging = false;
    }

    //void SelectHero(Hero hero)
    //{
    //    // ���������� ��������� ��������� (��������, ��������� ���������, ����-������ � �.�.)
    //}
}
