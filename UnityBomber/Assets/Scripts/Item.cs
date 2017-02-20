using UnityEngine;
using System.Collections;
using MonobitEngine;
using ItemAbility;

public class Item : Origin {

    enum  ITEM_TYPE {
        NONE = 0,
        BOX,
        UP,
        DOWN
    };
    //========================================================
    // 定数
    //========================================================
    // アイテムの生存時間
    private const  int LIMIT_TIME = 30;
    //========================================================
    // リテラル
    //========================================================
    // 現在のアイテムタイプ
    ITEM_TYPE m_currentItemType;
    // タイマー
    private Timer m_timer;
    // アイテム能力クラス
    private ItemAbility.BaseItemAbility m_itemAbility;
    //========================================================
    // 初期化処理
    //========================================================
    void Start () {
	    
	}
    // タイマーの初期化
    void InitTimer()
    {
        m_timer = new Timer();
        m_timer.LimitTime = LIMIT_TIME;
        m_timer.FireDelegate = Extinction;
        m_timer.IsEnable = true;
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update () {

        // タイマーの更新
        if (m_timer.Update()) {}
	}
    //========================================================
    // 更新処理はここまで
    //========================================================

    // アイテムの接触処理
    void OnTriggerEnter(Collider col) {

        // プレイヤーに当たった時
        if(col.tag == PLAYER_TAG) {
        }
        //箱以外で 爆風に当たった時
        if(col.tag == EXPLOSION_TAG && m_currentItemType != ITEM_TYPE.BOX) {

            // 消滅処理
            Extinction();
        }
        // 箱で 爆風に当たった時
        if (m_currentItemType == ITEM_TYPE.BOX) {

            // アイテムを出現
        }
    }
    // 消滅処理
    void Extinction() {
        // シーンから削除する
        MonobitNetwork.Destroy(gameObject);
    }
}
