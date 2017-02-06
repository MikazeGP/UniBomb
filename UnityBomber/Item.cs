using UnityEngine;
using System.Collections;
using MonobitEngine;

public class Item : Origin
{

    enum ITEM_TYPE : int
    {
        NONE = 0,
        BOX,
        PARAM_UP,
        PARAM_DOWN
    }

    //========================================================
    // 定数
    //========================================================
    // RPC
    private const string ITEM_ABILITY = "ItemAbility";

    //========================================================
    // リテラル
    //========================================================
    // 現在のアイテムの種類
    ITEM_TYPE m_nowItemType;



    //========================================================
    // 初期化処理
    //========================================================
    void Start(){

        if(m_nowItemType == ITEM_TYPE.NONE) {

        }

    }

    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理処理
    //========================================================
    void Update()
    {

    }

    //========================================================
    // 更新処理処理はここまで
    //========================================================

    //========================================================
    //　衝突判定処理
    //========================================================
    void OnTriggerEnter(Collider col)
    {

        // 爆発エフェクトに触れたとき
        if (col.tag == EXPLOSION_TAG && m_nowItemType == ITEM_TYPE.BOX)
        {

            string itemName = RandomItemName();
            // アイテムを出現
            GameObject newItem = MonobitNetwork.Instantiate(itemName, new Vector3(X, Y + 0.5f, Z), Quaternion.identity, 0);
            // アイテムの名前を変更
            newItem.GetComponent<Item>().m_nowItemType = GetItemType(itemName);
            // このアイテムを削除する
            MonobitNetwork.Destroy(gameObject);
        }
        // プレイヤーに触れたとき
        if (col.tag == "Player" && m_nowItemType != ITEM_TYPE.NONE)
        {

            Player plr = col.GetComponent<Player>();
            monobitView.RPC(ITEM_ABILITY, MonobitTargets.All, plr);

        }
    }

    /// <summary>
    /// 乱数でアイテムを決める
    /// </summary>
    /// <returns>アイテムの名前</returns>
    string RandomItemName()
    {

        // 7割りの確立で能力UPアイテム
        if (RandomBool(0.7f)) { return "UpItem"; }
        // 3割りの確立で能力ダウンアイテム
        else { return "BudItem"; }
    }
    /// <summary>
    /// アイテム名からアイテムを設定する
    /// </summary>
    /// <param name="itemName">ITEM_TYPE</param>
    /// <returns></returns>
    ITEM_TYPE GetItemType(string itemName)
    {

        ITEM_TYPE itemType = ITEM_TYPE.NONE;
        switch (itemName)
        {

            case "Box":
                itemType = ITEM_TYPE.BOX;
                break;
            case "UpItem":
                itemType = ITEM_TYPE.PARAM_UP;
                break;

            case "BudItem":
                itemType = ITEM_TYPE.PARAM_DOWN;
                break;
            default:

                break;
        }
        return itemType;
    }
}

