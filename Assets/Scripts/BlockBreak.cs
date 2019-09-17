using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    // 플레이어 변수
    public GameObject Player;

    // 플레이어 스크립트
    PlayerAgent pa;

    private void Start()
    {
        pa = Player.GetComponent<PlayerAgent>();
    }

    private void OnCollisionEnter(Collision col)
    {
        // 만일 부딪힌 게임오브젝트의 Tag 이름이 "Ball" 이라면,
        // 나를 비활성화 시키겠다.
        if(col.gameObject.CompareTag("ball"))
        {
            // 플레이어의 blockCount 갯수에서 1개씩 제거
            pa.blockCount--;

            // 나를 비활성화 함
            this.gameObject.SetActive(false);
        }
    }
}
