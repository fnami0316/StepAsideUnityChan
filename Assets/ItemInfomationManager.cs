using System.Collections.Generic;
using UnityEngine;

namespace main {

    /* アイテム情報管理クラス
     * itemInfomations アイテム情報の配列
     * Add(isGenerate, position, type) アイテムを追加する
     *
     *
     *
     */
    public class ItemInfomationManager {

        // アイテム情報の配列
        List<ItemInfomation> itemInfomations;

        // コンストラクタ
        public ItemInfomationManager() {
            this.itemInfomations = new List<ItemInfomation>();
        }

        // アイテム情報を追加する
        public void Add(Vector3 position, ItemInfomation.ItemType type, bool isGenerate = false) {
            ItemInfomation item = new ItemInfomation(position, type, isGenerate);
            this.itemInfomations.Add(item);
        }

        // アイテム情報を取り出す
        public List<ItemInfomation> GetList() {
            return itemInfomations;
        }
    }
}