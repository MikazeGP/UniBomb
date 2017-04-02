using UnityEngine;
using System.Collections;

/// <summary>
/// ボムについてのクラス
/// </summary>
public class Bomb : Origin {

    // 取り付けるコライダー
    public bool m_putCollider;
    // プレイヤー
    public Player m_plr;
    // タイマー
    Timer m_timer;
    // プレイヤー名
    private string m_playerName;

    // 初期化処理
    void Start() {

        // コライダーを外す
        m_putCollider = false;

        // グローバルデータにキャラの名前を設定
        m_playerName = GrobalData.Instance._plrCharaName;

        // タイマーを設定
        this.SetTimer();

        //　SEを再生
        AudioManager.Instance.PlaySE(AUDIO.SE_PUTBOMB);

        //　所有権が無ければ処理をしない
        if (!monobitView.isMine){

            return;
        }
        // 大きさを変更
        ChangeScale(m_playerName);
    }
    // 更新処理
    void Update() {

        //　確認
        Check();

        //　タイマーの更新
        if (m_timer.Update()) {

        }
    }

    //　爆発処理
    public void Explosion() {

        // 所有権がなければ処理しない
        if (!monobitView.isMine){

            return;
        }

        // エフェクトを生成
        GameObject effect = MonobitEngine.MonobitNetwork.Instantiate("Prefabs/ring1", gameObject.transform.position, Quaternion.identity, 0, null);

        // 生成したエフェクトにプレイヤー情報を渡す
        effect.GetComponent<Effect>().m_plr = m_plr;

        // オブジェクトを削除する
        MonobitEngine.MonobitNetwork.Destroy(gameObject);

        // 置くことができるボムを増やす
        m_plr.m_stockBomb++;

    }
    // 衝突判定処理
    void OnTriggerEnter(Collider col){

        // 爆発エフェクトに触れたら...
        if (col.tag == "Explosion") {

            // 爆発する
            Explosion();
        }
    }

    // 3病後に爆発するように設定
    void SetTimer() {

        // タイマーをインクリメントする
        m_timer = new Timer();
        // みさきのみ2秒で爆発するよう設定
        if (m_playerName == "Misaki") {

            // タイマーの時間を設定
            m_timer.LimitTime = 2.0f;
           
        }else if (m_playerName == "UnityChan") {

            m_timer.LimitTime = 3.0f;

        }else {
            // 4.0秒で爆発するよう設定
            m_timer.LimitTime = 4.0f;
        }
        // 上記で指定した時間に達したとき
        //　Delegateで指定した関数を呼び出す
        m_timer.FireDelegate = Explosion;
    }
    
    /// <summary>
    /// キャラによってボムの大きさを変更
    /// </summary>
    /// <param name="name"></param>
    void ChangeScale(string name) {

        switch (name) {

            case "UnityChan":
                float utc_Size = 0.5f * m_plr.m_bombPower;
                SetScale(utc_Size, utc_Size, utc_Size);
                break;

            case "Misaki":
                float misaki_Size = 0.5f * m_plr.m_bombPower;
                SetScale(misaki_Size, misaki_Size, misaki_Size);
                break;

            case "Yuko":
                float yuko_Size = 0.7f * m_plr.m_bombPower;
                SetScale(yuko_Size, yuko_Size, yuko_Size);
                break;

            default:

                SetScale(0.5f, 0.5f, 0.5f);
                break;
        }
    }

    // 確認処理
    void Check() {
        // 所有権が無ければ処理しない
        if (!monobitView.isMine) {

            return;
        }
        // 一定距離離れて、コライダーがなければ...
        if(Vector3.Distance(m_plr.transform.position,gameObject.transform.position) > 1.25f && m_putCollider == false) {

            // レイヤーを変更する
            gameObject.layer = 0;
            // ボムも発射できるようにする
            m_plr.m_shotBomb = true;
            // コライダーを取り付ける
            m_putCollider = true;
        }
    }
}
