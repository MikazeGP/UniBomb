using UnityEngine;
using System.Collections;
using MonobitEngine;


public class Player : Origin{

    // ゲームマネージャーオブジェクトの名前
    private const string GAME_MGR = "GameMgr";

    // RPC
    // 死亡受信処理名
    private const string RPC_DIE = "Die";

    // 現在所持しているボムの数
    public int stockBomb;
    // 移動速度
    public float moveSpeed;

    // プレイヤーが移動できる最大位置
    private float EndPosxMin = -4.8f;
    private float EndPosxMax = 4.8f;
    private float EndPoszMin = -4.3f;
    private float EndPoszMax = 4.3f;

    // 初期化フラグ
    private bool m_init = false;
    // ボム発射フラグ true..発射可能 false..発射不可能
    public bool m_shotBomb;
    // 死亡フラグ true..死亡 false..生存
    public bool m_deathFlag; 
    // タイマークラス
    private Timer m_timer;
    // プレイヤー残機
    private int m_plrStock;
    // ゲームマネージャー
    private GameMgr m_gamemgr;

    // 初期化
    void Start () {


        FrameCount = 0;

        //Bomb.parent = new OriginMgr<Bomb>("Pbomb", 128);
        // 発射可能にする
        m_shotBomb = true;

        // 最初は生きているのでtrueにする
        m_deathFlag = false;

        // グローバルデータから残機の数を取得

        m_plrStock = GrobalData.Instance._plrStock[PlayerId];

        // ゲームマネージャーをシーンから探す
        m_gamemgr = GameObject.Find(GAME_MGR).GetComponent<GameMgr>();
    }
	
	// Update is called once per frame
	void Update () {

        // オブジェクト所有権を所持しなければ実行しない
        if (!monobitView.isMine) {

            GetComponent<CharacterController>().enabled = false;

            return;

        }
        //if(gamemgr.gameStart == false) { return; }


        if(m_deathFlag == true) {

            if (m_timer.Update()){

            }

            return;

        }

        this.Shot();

        this.MoveFunc();
	}

    // ボムを置く
    void Shot() {

        
        if (Input.GetButton(FIRE1_BUTTON)) {

            
            if (FrameCount % 12 == 0 && m_shotBomb == true && stockBomb > 0) {

                //AddBomb(X, Y+ 0.2f, Z, 0, 0);

                GameObject bombobj = MonobitEngine.MonobitNetwork.Instantiate("Prefabs/Pbomb", new Vector3(X, Y + 0.2f, Z), Quaternion.identity, 0, null);
                bombobj.GetComponent<Bomb>().plr = this;

                bombobj.layer = gameObject.layer;
                m_shotBomb = false;

                stockBomb--;                                

                // ここで音を再生
                AudioManager.Instance.PlaySE(AUDIO.SE_PUTBOMB);
            }
        }
    }

    void MoveFunc() {

        // 自分の位置を取得
        Vector3 p = new Vector3(X, 0f, Z);

        
        p.x = 0.05f*moveSpeed*Input.GetAxisRaw(AXIS_HORIZONTAL);
        p.z = 0.05f*moveSpeed*Input.GetAxisRaw(AXIS_VERTICAL);


        // 移動する向きに回転
        gameObject.transform.LookAt(p + transform.position);
        // 位置を代入して移動
        //gameObject.transform.position = p;

        gameObject.GetComponent<CharacterController>().Move(p);

        
        // 移動している時...移動アニメーション 移動していない時...待機処理
        if(Input.GetAxisRaw(AXIS_HORIZONTAL) != 0 || Input.GetAxisRaw(AXIS_VERTICAL) != 0) {

            // 移動アニメーション
            Animator.SetFloat("Speed", 2.0f);

        
        }else {

            // 待機アニメーション
            Animator.SetFloat("Speed", 0);
        }

        this.Clamp();
    }

    // プレイヤーの座標制限
    void Clamp()
    {
        // プレイヤーの座標を取得
        Vector3 pos = gameObject.transform.position;

        //プレイヤーの位置の制限
        pos.x = Mathf.Clamp(pos.x, EndPosxMin, EndPosxMax);
        pos.z = Mathf.Clamp(pos.z, EndPoszMin, EndPoszMax);

        //プレイヤーの制限位置を取得
        gameObject.transform.position = pos;
    }

    // 爆風やアイテムに触れた時の処理
    void OnTriggerEnter(Collider col) {

        // エフェクトに触れたら倒れる
        if(col.tag == EXPLOSION_TAG && m_deathFlag == false) {

            // デバッグ用
            print("Col");
            
            // ダウンアニメーション
            Animator.SetBool("Down", true);

            // 死亡フラグをtrueにする
            m_deathFlag = true;

            // タイマーを設定
            SetTimer0();

            // 死亡処理を送信
            monobitView.RPC(RPC_DIE, MonobitTargets.All, PlayerId, stockBomb);

        }
    }

    // タイマー
    void SetTimer0(){

        m_timer = new Timer();
        m_timer.LimitTime = 4.0f;
        m_timer.FireDelegate = Death;
    }

    void Death() {

        MonobitEngine.MonobitNetwork.Destroy(gameObject);
    }


    private string DieVoice(int stock) {

        string voice = "";

        switch (stock) {

            case 2:
                if(m_plrStock != 1) {

                    voice = AUDIO.VOICE_V2018;

                }else {

                    voice = AUDIO.VOICE_V2019;
                }

                break;

            case 3:

                if (m_plrStock != 1){

                    voice = AUDIO.VOICE_V0018;
                }else{

                    voice = AUDIO.VOICE_V0019;
                }

                break;

            case 4:

                if (m_plrStock != 1){

                    voice = AUDIO.VOICE_V1018;

                }else{

                    voice = AUDIO.VOICE_V1019;
                }

                break;

        }

        return voice;
    }

    [MunRPC]
    // 死亡処理を受信
    void Die(int id,int bomb) {

        // SEを再生
        AudioManager.Instance.PlayVoice(DieVoice(bomb));

        // 
        GrobalData.Instance._plrStock[id]--;
    }
}
