using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeleter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        // 本オブジェクトのz座標がメインカメラのz座標より小さい場合オブジェクトを削除
        if(this.gameObject.transform.position.z < Camera.main.transform.position.z) Destroy(this.gameObject);
    }
}
