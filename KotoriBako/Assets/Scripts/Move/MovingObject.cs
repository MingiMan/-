using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public string characterName;
    public float moveSpeed;
    int runSpeed;
    public int walkCount;
    public LayerMask colliderMask;

    protected int currentWalkCount;
    protected Vector3 vector;
    public BoxCollider2D boxCollider2D;
    public Animator animor;

    bool notCoroutine = false;
    // 선입선출 구조 먼저 들어온게 있으면 먼저 들어온것부터 뺀다.
    public Queue<string> queue;

    bool IsRun;

    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0)
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    IsRun = true;
                    runSpeed = 2;
                    break;
                case 4:
                    IsRun = true;
                    runSpeed = 3;
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animor.SetFloat("DirX", vector.x);
            animor.SetFloat("DirY", vector.y);

            while (true)
            {
                bool checkCollsionFlag = CheckCollsion();

                if (checkCollsionFlag)
                {
                    animor.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                    break;
            }

            animor.SetBool("Walking", true);

            // 픽셀 단위만큼 미리 먼저 움직인다..

            // boxCollider2D.offset = new Vector2(vector.x * 0.7f * moveSpeed * walkCount, vector.y * 0.7f * moveSpeed * walkCount);

            boxCollider2D.offset = new Vector2(vector.x, vector.y);


            while (currentWalkCount < walkCount)
            {
                if (IsRun)
                    transform.Translate(vector.x * moveSpeed * runSpeed, vector.y * moveSpeed * runSpeed, 0);
                else
                    transform.Translate(vector.x * moveSpeed, vector.y * moveSpeed, 0);

                currentWalkCount++;

                if (currentWalkCount == walkCount * 0.5f + 2)
                {
                    boxCollider2D.offset = Vector2.zero;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;


            if (_frequency != 5)
                animor.SetBool("Walking", false);

        }
        animor.SetBool("Walking", false);
        notCoroutine = false;
        IsRun = false;
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit;

        // A 지점 캐릭터의 현재 위치 값
        Vector2 start = new Vector2(transform.position.x + vector.x * moveSpeed * walkCount,
            transform.position.y + vector.y * moveSpeed * walkCount);


        // B지점 캐릭터가 이동하고자 하는 위치 값
        Vector2 end = start + new Vector2(vector.x * moveSpeed, vector.y * moveSpeed);

        boxCollider2D.enabled = false;

        hit = Physics2D.Linecast(start, end, colliderMask);

        boxCollider2D.enabled = true;

        if (hit.collider != null)
            return true;

        return false;
    }
}
