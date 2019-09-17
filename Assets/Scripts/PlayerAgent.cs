using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using MLAgents;

public class PlayerAgent : Agent
{
    // 발판의 이동속도
    public float speed = 5f;

    // 공의 발사 속도
    public float ballSpeed = 6f;

    // 공 게임 오브젝트
    public GameObject ball;

    // 모든 블록 배열
    public GameObject[] blocks;

    // 공의 리지드 바디 컴포넌트
    Rigidbody rb;

    // 최기 위치 변수
    Vector3 originPlayerPos;
    Vector3 originBallPos;

    // 블록 갯수 체크용 변수
    public float blockCount;

    void Start()
    {
        // 발판의 초기 위치 저장하기
        originPlayerPos = this.transform.position;

        // 공의 위치 저장하기
        originBallPos = ball.transform.position;

        // 블록 갯수 저장
        blockCount = blocks.Length;

        // 2초뒤에 공을 발사하는 함수 실행
        Invoke("ShootBall", 2f);
    }

    private void ShootBall()
    {
        // 공의 리지드바디 컴포넌트를 가져온다(컴포넌트 캐싱).
        rb = ball.GetComponent<Rigidbody>();

        // 공을 랜덤(-45도 ~ 45도) 한 방향으로 발사하고 싶다!
        float degree = UnityEngine.Random.Range(45, 135);

        Vector3 dir = new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad),
                                Mathf.Sin(degree * Mathf.Deg2Rad),
                                0);

        rb.velocity = dir * ballSpeed;
    }

    void Update()
    {
        // 플레이어의 좌우 입력을받아서 발판을 좌우로 움직이게 하고 싶다.
        
        // 1. 입력, 이동 속도, 이동 방향
        float h = Input.GetAxis("Horizontal");

        // 2. 이동 방향
        Vector3 dir = new Vector3(h,0,0);
        dir.Normalize();

        // 3. 이동하기
        transform.position += dir * speed * Time.deltaTime;

        // 4. 좌표 움직임 -3.25 ~ 3.25 사이의 움직임만 가능하도록 함
        Vector3 myPos = transform.position;
        myPos.x = Mathf.Clamp(myPos.x, -3.25f, 3.25f);
        transform.position = myPos;

        // 5.승리 조건 패배 조건 체크 함수
        CheckBallState();
        CheckBlocks();
    }

    // 모든 블록이 사라지거나, 공의 위치가 발판 위치보다 아래로 내려가면 게임을 리셋 함
    private void CheckBallState()
    {
        if(ball.transform.position.y < transform.position.y)
        {
            ReSetGame();
        }
    }

    // 모든 블록이 다 제거되면 게임을 리셋하고 싶다.
    void CheckBlocks()
    {
        if(blockCount <= 0)
        {
            ReSetGame();
        }
    }

    // 리셋 하기
    private void ReSetGame()
    {
        // 1. 발판의 위치를 원래대로 하고 싶다.
        transform.position = originPlayerPos;

        // 2. 공의 위치를 원래대로 하고 싶다.
        ball.transform.position = originBallPos;

        // 3. 모든 블록을 다시 원위치 시키고 싶다(활성화).
        for(int i=0; i < blocks.Length; i++)
        {
            blocks[i].SetActive(true);
        }

        // 4. 블록 갯수 리셋
        blockCount = blocks.Length;

        // 공의 속도를 멈춰 놓은 상태
        rb.velocity = Vector3.zero;

        // 2초뒤에 공을 발사하는 함수 실행
        Invoke("ShootBall", 2f);
    }
}
