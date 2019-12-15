using UnityEngine;

namespace main {
    /* アイテム情報クラス
     * isGenerate アイテムが生成されたか(true:生成済み, false:生成済みでない)
     * position アイテムの座標
     * type アイテムの種類
     */
    public class ItemInfomation {
        // アイテムの種類の定義
        public enum ItemType {
            // 車
            CAR,
            // コーン
            CONE,
            // コイン
            COIN
        }

        // コンストラクタ
        public ItemInfomation(Vector3 position, ItemType type, bool isGenerate = false) {
            this.isGenerate = isGenerate;
            this.position = position;
            this.type = type;
        }

        // 生成されたか
        public bool isGenerate;
        // 位置
        public Vector3 position;
        // 種類
        public ItemType type;
    }
}