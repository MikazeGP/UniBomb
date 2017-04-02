﻿using UnityEngine;
using System.Collections;
using MonobitEngine;
using ItemAbility;

public class Item : Origin {

    // アイテムの種類
    public enum  ITEM_TYPE {
        NONE = 0,
        BOX,
        BUFF,
        NUFF
    };
    //========================================================
    // 定数
    //========================================================
    // アイテムの生存時間
    private const  int LIMIT_TIME = 30;
    // アイテムプレハブパス
    private const string BUFF_ITEM_PREFAB_PASS = "Prefabs/BuffItem";
    private const string NUFF_ITEM_PREFAB_PASS = "Prefabs/NuffItem";

    // コルーチン
    // アイテムの無敵化
    private const string COROUTINE_INVINCIBLE_ITEM = "InvincibleItem";

    // 回転量
    private Vector3 ITEM_ROTATION =  new Vector3(0f, 1.666f, 0f);
    //========================================================
    // リテラル
    //========================================================
    // 現在のアイテムタイプ
    public ITEM_TYPE m_currentItemType;
    // タイマー
    private Timer m_timer;
    // アイテム能力クラス
    private ItemAbility.ItemAbility m_itemAbility;
    // アイテムの無敵化
    private bool m_invincible;
    //========================================================
    // 初期化処理
    //========================================================
    void Start () {

        if(m_currentItemType == ITEM_TYPE.NONE || m_currentItemType == ITEM_TYPE.BOX) { return; }
        
        // タイマーの初期化
        InitTimer();
        // アイテムの初期化
        InitItem();
    }
    // タイマーの初期化
    void InitTimer()
    {
        m_timer = new Timer();
        m_timer.LimitTime = LIMIT_TIME;
        m_timer.FireDelegate = Extinction;
        m_timer.IsEnable = true;
    }
    // アイテムの初期化
    void InitItem() {
        
        if(m_currentItemType == ITEM_TYPE.BUFF) {

            // アイテムの設定
            SetItemAbility(new ItemAbility.UpAbility());            
        }
        else if(m_currentItemType == ITEM_TYPE.NUFF) {
            // アイテムの設定
            SetItemAbility(new ItemAbility.DownAbility());
        }
        // コルーチンの開始
        StartCoroutine(COROUTINE_INVINCIBLE_ITEM);
    }
    // 無敵アイテムの設定
    private IEnumerator InvincibleItem() {
        m_invincible = true;
        // 2秒待機
        yield return new WaitForSeconds(2.0f);
        m_invincible = false;
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update () {

        if(m_currentItemType == ITEM_TYPE.NONE ) { return; }
        // 移動、回転の更新
        UpdateTransform();
        
        if(m_currentItemType == ITEM_TYPE.BOX) { return; }
        // タイマーの更新
        if (m_timer.Update()) {}
       
	}
    // 移動の更新
    void UpdateTransform() {

        if(m_currentItemType == ITEM_TYPE.BOX) {
            //箱の移動
            BoxMove();

        }else {
            // アイテムの回転
            ItemRotation();
        }
    }
    // 箱の移動
    void BoxMove() {
            // 箱を位置を変更する
            if (Y > -0.185f) { Y -= 0.1f; }
    }
    // アイテムの回転
    void ItemRotation() {
        // アイテムを回転
        gameObject.transform.Rotate(ITEM_ROTATION);
    }
    //========================================================
    // 更新処理はここまで
    //========================================================

    // アイテムの接触処理
    void OnTriggerEnter(Collider col) {

        // アイテムの種類がなければ処理をしない
        if(m_currentItemType == ITEM_TYPE.NONE) { return; }

        // プレイヤーに当たった時
        if (col.tag == PLAYER_TAG&& m_currentItemType != ITEM_TYPE.BOX) {
 
            if (col.GetComponent<Player>() == null) { return; }
            Player m_plr = col.GetComponent<Player>();
            // アイテムの効果発動
            GetItemAbility(m_plr);

            monobitView.RPC("DestoryItem", MonobitTargets.All, null);
            //Extinction();
        }
        //箱以外で 爆風に当たった時
        if(col.tag == EXPLOSION_TAG && m_currentItemType != ITEM_TYPE.BOX) {
            print("消滅");
            // 無敵時かホスト以外は処理をしない
            if (!MonobitEngine.MonobitNetwork.isHost || m_invincible == true){
                return;
            }
            // 消滅処理
            Extinction();
        }
        // 箱で 爆風に当たった時
        if (col.tag == EXPLOSION_TAG &&m_currentItemType == ITEM_TYPE.BOX) {

            // ホスト以外は処理をしない
            if (!MonobitEngine.MonobitNetwork.isHost){
                return;
            }
            // アイテムをスポーンする
            SpawnItem();
            // 位置を変更する
            gameObject.transform.position = new Vector3(X, 140, Z);
        }
    }
    // 消滅処理
    void Extinction() {
        print(gameObject);
        // シーンから削除する
        MonobitNetwork.Destroy(gameObject);
    }
    // アイテムの効果を呼び出す
    void GetItemAbility(Player plr) {

        this.m_itemAbility.SetAbility(plr);
    }
    // アイテムの効果を設定
    void SetItemAbility(ItemAbility.ItemAbility ability) {

        m_itemAbility = ability;
    }
    // アイテムをスポーンする
    void SpawnItem(){

        int randomInt = Random.Range(0, 10);
        Vector3 pos = new Vector3(X,0.3f, Z);
        // 4より小さいなら出現しない
        if( randomInt < 2) { return; }
        // 2より大きく７より小さいとき
        else if(1 < randomInt && randomInt < 8) {
            // Buffアイテム出現
            GameObject buffItem = MonobitNetwork.Instantiate(BUFF_ITEM_PREFAB_PASS, pos, Quaternion.identity, 0, null);
            return;
            // それ以外
        }else {
            // Nuffアイテム出現
            GameObject nuffItem = MonobitNetwork.Instantiate(NUFF_ITEM_PREFAB_PASS, pos, Quaternion.identity, 0, null);
            return;
        }
    }
    //========================================================
    // RPC処理  
    //========================================================

    [MunRPC]
    /// <summary>
    /// 　アイテム破壊処理
    /// </summary>
    void DestoryItem() {
        Destroy(gameObject);
    }
    //========================================================
    // RPC処理 はここまで
    //========================================================
}
