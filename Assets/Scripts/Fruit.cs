using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType{Apple, Banana, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry}

public class Fruit : MonoBehaviour
{
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickupVfx; // pickupVfsPrefab

    private GameManager gameManager;
    protected Animator animator; // 공유하기 위해 protected로
    protected SpriteRenderer sr;

    void Awake(){
       animator = gameObject.GetComponentInChildren<Animator>();
       sr = GetComponentInChildren<SpriteRenderer>(); // 사실 gameObject는 생략해도 된다.
    }

    protected virtual void Start(){
       gameManager = GameManager.instance; 
       SetRandomLook();
    }

    void SetRandomLook(){
        if(gameManager.DoFruitsNeedRandomLook() == false){ 
            UpdateFruitVisuals();    
            return;
        } //메니저에서 관리하지 않고 각자 상황

        int randomIndex = Random.Range(0,8); //0~7
        animator.SetFloat("fruitIndex", randomIndex);
    }
    void UpdateFruitVisuals() => animator.SetFloat("fruitIndex", (int)fruitType);

    protected virtual void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            gameManager.AddFruit();
            AudioManager.instance.PlaySFX(7); //Pickup
            Destroy(gameObject);

            GameObject newFx = Instantiate(pickupVfx, transform.position, Quaternion.identity);
            // Destroy(newFx, .5f);
        }
    }
}
