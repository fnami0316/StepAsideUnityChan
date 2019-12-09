using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    // メインカメラのオブジェクト
    private GameObject mainCamera;

    // Use this for initialization
    void Start () {
        // メインカメラのオブジェクトを取得
        this.mainCamera = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {
        // 本オブジェクトのz座標がメインカメラのz座標より小さい場合オブジェクトを削除
        if(this.gameObject.transform.position.z < this.mainCamera.transform.position.z) Destroy(this.gameObject);
    }


}
