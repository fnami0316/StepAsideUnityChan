using UnityEngine;
using System.Collections;
using main;

public class ItemGenerator : MonoBehaviour {
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //アイテム生成開始z座標
    private int startPos = -160;
    //アイテム生成終了z座標
    private int goalPos = 120;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    // アイテム情報管理クラスオブジェクト
    private ItemInfomationManager itemInfomationManager;

    // Unityちゃんのオブジェクト情報
    // (Unityちゃんの前方範囲を決める上で絶対必要)
    private GameObject unitychan;

    // アイテム生成範囲 
    // (Unityちゃん前方をz軸正方向で決め打ちしてminZ〜maxZで表現
    private float minZ;
    private float maxZ;

    // 10mカウンター
    float counter10m = 0;

    // デバッグ用
    int tmpGenCnt = 1;
    int tmpItemGenCnt = 0;

    void Start() {

        Debug.Log("★ゲーム開始★");

        // アイテム管理情報クラスのインスタンスを生成
        this.itemInfomationManager = new ItemInfomationManager();

        // ユニティちゃんのオブジェクト情報を取得
        this.unitychan = GameObject.Find("unitychan");
        this.counter10m = this.unitychan.transform.position.z;

        Debug.Log("--Unityちゃんの位置情報--");
        Debug.Log("Unitychan.transform.position :" + this.unitychan.transform.position);
        
        // 各アイテムのPrefab情報を取得
        this.carPrefab = (GameObject)Resources.Load("CarPrefab");
        this.coinPrefab = (GameObject)Resources.Load("CoinPrefab");
        this.conePrefab = (GameObject)Resources.Load("TrafficConePrefab");

        Debug.Log("-- 各プレハブの位置情報 --");

        Debug.Log("CarPrefab.transform.position :" + carPrefab.transform.position);
        Debug.Log("CoinPrefab.transform.position :" + coinPrefab.transform.position);
        Debug.Log("ConePrefab.transform.position :" + conePrefab.transform.position);

        // 各アイテムを生成する位置を決定
        this.DecideAllItemPosition();

        Debug.Log("-- アイテムタイプの定義 --");
        Debug.Log("(車, コーン, コイン):(" + ItemInfomation.ItemType.CAR + ", " + ItemInfomation.ItemType.COIN + ", " + ItemInfomation.ItemType.CONE + ")");

        Debug.Log("--アイテム情報群の確認 Start()--");
        int tmpCnt = 0;
        foreach(ItemInfomation itemInfo in this.itemInfomationManager.GetList()) {

            Debug.Log("アイテム[" + tmpCnt + "] : (" + itemInfo.isGenerate + "," + itemInfo.type + "," + itemInfo.position + ")");
     
            tmpCnt++;
        }

    }

    // Update is called once per frame
    void Update() {
        //Unityちゃんが次の10m進んでいない場合は打ち切り
        if(unitychan.transform.position.z < this.counter10m) {
            
            return;
        }

        Debug.Log("★★Update():" + this.tmpGenCnt + "回目の生成しようとする処理★★");
        this.tmpGenCnt++;
        Debug.Log("unityちゃんのz座標: " + unitychan.transform.position.z);

        this.counter10m += 10;


        // アイテム生成対象範囲の計算
        CalcItemGenerateTargetRange(this.unitychan.transform.position);
        Debug.Log("アイテム生成対象範囲(minZ, maxZ) = (" + minZ + ", " + maxZ + ")");

        // 各アイテムがアイテム生成対象範囲に含まれるならばアイテムを生成(ただし一度生成されているアイテムは生成しない)
        // 全アイテム情報をチェック
        //int tmpCnt = 0;
        foreach(ItemInfomation itemInfo in this.itemInfomationManager.GetList()) {
            //Debug.Log("アイテム[" + tmpCnt + "] : (" + itemInfo.isGenerate + "," + itemInfo.type + "," + itemInfo.position + ")");
            //tmpCnt++;

            // 当該アイテムがすでに生成されている場合次のアイテムへ
            if(itemInfo.isGenerate) continue;

            // 当該アイテムがアイテム生成対象範囲に含まれない場合次のアイテムへ
            if(!IsItemInGenerateTargetRange(itemInfo.position.z)) continue;

            // 当該アイテムを生成
            this.tmpItemGenCnt++;
            Debug.Log("アイテム生成[" + tmpItemGenCnt + "]個目");
            switch(itemInfo.type) {
                // 当該アイテムが車のとき
                case ItemInfomation.ItemType.CAR:
                    this.GenerateCarObject(itemInfo.position);
                    break;
                // 当該アイテムがコインのとき
                case ItemInfomation.ItemType.COIN:
                    this.GenerateCoinObject(itemInfo.position);
                    break;
                // 当該アイテムがコーンのとき
                case ItemInfomation.ItemType.CONE:
                    this.GenerateConeObject(itemInfo.position);
                    break;
            }

            // 当該アイテムを生成したことを記録
            itemInfo.isGenerate = true;

            
        }
    }

    // 各アイテムの座標を決定
    void DecideAllItemPosition() {
        //一定の距離ごとにアイテムを生成
        for(int i = startPos; i < goalPos; i += 15) {
            //どのアイテムを出すのかをランダムに設定
            int num = Random.Range(1, 11);
            if(num <= 2) {
                //コーンをx軸方向に一直線に生成
                for(float j = -1; j <= 1; j += 0.4f) {
                    // 当該コーンの位置
                    var conePos = new Vector3(4 * j, conePrefab.transform.position.y, i);
                    // 当該コーンのアイテム情報を追加
                    this.itemInfomationManager.Add(conePos, ItemInfomation.ItemType.CONE);
                }
            }
            else {

                //レーンごとにアイテムを生成
                for(int j = -1; j <= 1; j++) {
                    //アイテムの種類を決める
                    int item = Random.Range(1, 11);
                    //アイテムを置くZ座標のオフセットをランダムに設定
                    int offsetZ = Random.Range(-5, 6);
                    //60%コイン配置:30%車配置:10%何もなし
                    if(1 <= item && item <= 6) {
                        // 当該コインの位置
                        var coinPos = new Vector3(posRange * j, coinPrefab.transform.position.y, i + offsetZ);
                        // 当該コインのアイテム情報を追加
                        itemInfomationManager.Add(coinPos, ItemInfomation.ItemType.COIN);
                    }
                    else if(7 <= item && item <= 9) {
                        // 当該コインの位置
                        var carPos = new Vector3(posRange * j, carPrefab.transform.position.y, i + offsetZ);
                        // 当該コインのアイテム情報を追加
                        itemInfomationManager.Add(carPos, ItemInfomation.ItemType.CAR);
                    }
                }
            }
        }
    }

    // アイテム生成対象範囲の計算
    // Vector3 unityChanPos Unityちゃんの位置
    void CalcItemGenerateTargetRange(Vector3 unityChanPos) {
        // Unityちゃん前方50m範囲を取得(minZ, maxZ)
        minZ = unityChanPos.z;
        maxZ = unityChanPos.z + 40;
    }

    /*
     *  アイテム生成対象範囲に含まれるか
     *  z 対象アイテムの座標
     *  返り値 含まれる場合true
     */
    bool IsItemInGenerateTargetRange(float z) {
        if(z >= minZ && z < maxZ) {
            return true;
        }
        return false;
    }

    /*
     * 車オブジェクトを生成
     * Vector3 pos オブジェクトの位置
     */
    void GenerateCarObject(Vector3 pos) {
        var car = Instantiate(carPrefab) as GameObject;
        car.transform.position = new Vector3(pos.x, pos.y, pos.z);
        Debug.Log("アイテム[車]を生成しました(z座標:" + car.transform.position.z + ")");
    }

    /*
     * コインオブジェクトを生成
     * Vector3 pos オブジェクトの位置
     */
    void GenerateCoinObject(Vector3 pos) {
        var coin = Instantiate(coinPrefab) as GameObject;
        coin.transform.position = new Vector3(pos.x, pos.y, pos.z);
        Debug.Log("アイテム[コイン]を生成しました(z座標:" + coin.transform.position.z + ")");
    }

    /*
     * コーンオブジェクトを生成
     * Vector3 pos オブジェクトの位置
     */
    void GenerateConeObject(Vector3 pos) {
        var cone = Instantiate(conePrefab) as GameObject;
        
        cone.transform.position = new Vector3(pos.x, pos.y, pos.z);
        Debug.Log("アイテム[コーン]を生成しました(z座標:" + cone.transform.position.z + ")");
    }


}
