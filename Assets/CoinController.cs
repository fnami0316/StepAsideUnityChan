using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

    // メインカメラのオブジェクト
    private GameObject mainCamera;

    // Use this for initialization
    void Start() {
        // メインカメラのオブジェクトを取得
        this.mainCamera = GameObject.Find("Main Camera");

        //回転を開始する角度を設定
        this.transform.Rotate(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update() {
        // 本オブジェクトのz座標がメインカメラのz座標より小さい場合オブジェクトを削除
        if(this.gameObject.transform.position.z < this.mainCamera.transform.position.z) Destroy(this.gameObject);

        //回転
        this.transform.Rotate(0, 3, 0);
    }

}