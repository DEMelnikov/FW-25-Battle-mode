using UnityEngine;

public class MouseInputController : MonoBehaviour
{
    private Character selectedHero;
    [SerializeField] private DragShadow dragShadowPrefab; // prefab для создания тени
    private DragShadow dragShadow;
    private bool isDragging = false;
    private Vector3 mouseDownPosition;         // Позиция мыши при нажатии
    private const float dragThreshold = 5f;   // Минимальное расстояние (в пикселях) для начала drag

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryBeginDrag();

        // Проверяем, превышен ли порог для drag
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
                mouseDownPosition = Input.mousePosition; // Запоминаем стартовую позицию мыши
                isDragging = true;
                // Пока не создаём тень и не запускаем drag! Ждём смещения мыши
            }
        }
    }

    void UpdateDragShadow()
    {
        // Вычисляем насколько далеко ушла мышь от начальной позиции
        float distance = Vector3.Distance(mouseDownPosition, Input.mousePosition);
        if (distance > dragThreshold && dragShadow == null)
        {
            // Если порог пройден, стартует drag, создаём тень героя
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
            // Если drag не начат или мышь почти не двигалась — считаем это кликом
            // Например, просто выбираем героя без отметки цели или waypoint-a
            //SelectHero(selectedHero); // Ваша логика выбора
        }
        else
        {
            // Drag действительно имел место — обычная логика
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

        // Очистка состояния
        if (dragShadow != null) Destroy(dragShadow.gameObject);
        //dragShadow = null;
        selectedHero = null;
        isDragging = false;
    }

    //void SelectHero(Hero hero)
    //{
    //    // Реализуйте выделение персонажа (например, выделение визуально, инфо-панель и т.д.)
    //}
}
