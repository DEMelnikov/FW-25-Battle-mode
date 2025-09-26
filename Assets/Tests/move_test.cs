using UnityEngine;

public class MoveTest : MonoBehaviour
{
    private Animator animator;
    private float moveSpeed = 5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveVector = new Vector3(moveX, moveY, 0f).normalized;

        // ��������
        transform.Translate(moveVector * moveSpeed * Time.deltaTime);

        // ����� ��������� Walk/Idle
        bool isMoving = moveVector.sqrMagnitude > 0.01f;
        animator.SetBool("Walk", isMoving);

        // �������� ��������� �� �����������
        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // ����� (�� �������)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("ATTACK");
            //animator.SetBool("Attack",);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("DEATH");
            //animator.SetBool("Attack",);
        }
    }
}
