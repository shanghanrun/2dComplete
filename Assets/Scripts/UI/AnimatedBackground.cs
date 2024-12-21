using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType{ Blue,Brown,Gray,Green,Pink,Purple,Yellow }

public class AnimatedBackground : MonoBehaviour
{
    MeshRenderer mesh;
    [SerializeField] Vector2 movementDirection;

    [Header("Color")]
    [SerializeField] BackgroundType backgroundType; // enum이라서 선택버튼이 생긴다. 선택한다.
    [SerializeField] Texture2D[] textures; // 외부에서 인덱스에 따라서 넣어준다.

    void Awake(){
        mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }
    void Update(){
        mesh.material.mainTextureOffset += movementDirection *Time.deltaTime;
    }

    [ContextMenu("Update Background")]
    void UpdateBackgroundTexture(){
        if(mesh == null){
            mesh = GetComponent<MeshRenderer>();
        }
        mesh.sharedMaterial.mainTexture = textures[(int)backgroundType]; //선택한 타입의 인덱스번호
    }
}
