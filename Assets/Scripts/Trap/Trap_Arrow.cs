using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Arrow : Trap_Trampoline
{
    [Header("Additional info")]
    [SerializeField] bool clockwise = true;
    [SerializeField] float rotationSpeed = 120;
    // [Space]
    // [SerializeField] float scaleUpSpeed = 4;
    // [SerializeField] Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    int direction = 1;
    bool isDestroyed = false;

    // void Start(){
    //     transform.localScale = new Vector3(.3f, .3f, .3f);
    // }

    void Update()
    {
        // if(transform.localScale.x < targetScale.x){ //비교는 단일 수치로
        //     transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
        // }
        HandleRotate();
    }

    private void HandleRotate()
    {
        direction = clockwise ? -1 : 1;
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * direction);
    }

    void DestroyMe(){
        if(isDestroyed) return;
        isDestroyed = true;

        GameObject newObject = GameManager.instance.arrowPrefab;
        GameManager.instance.CreateObject(newObject, transform);
        Destroy(gameObject, 0.3f); // 에니메이션 이벤트에서 사용
    }
}
