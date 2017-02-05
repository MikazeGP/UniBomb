using UnityEngine;
using System.Collections;

public class Bomb : Origin {

    public GameObject colObj;
    public bool putCollider;
    public Player plr;
    Timer _timer;
    private string m_playerName;

    void Start() {


        putCollider = false;

        m_playerName = GrobalData.Instance._plrCharaName;

        this.SetTimer();

        ChangeScale(m_playerName);

       
    }

    void Update() {

        Check();

        if (_timer.Update()) {

        }
    }

    public void Explosion() {

        // 置くことができるボムを増やす
        plr.stockBomb++;

        // エフェクトを生成
        GameObject effect = MonobitEngine.MonobitNetwork.Instantiate("Prefabs/ring1", gameObject.transform.position, Quaternion.identity, 0, null);

        //Destroy(this);
        MonobitEngine.MonobitNetwork.Destroy(gameObject);

        // エフェクトを取り付ける
        //AddEffect(X, Y, Z, 90, 0);

    }

    void OnTriggerEnter(Collider col){

        print("衝突した" + col.name);

        // 爆発エフェクトに触れたら...
        if (col.tag == "Explosion") {

            
            
            if(FrameCount%9999 == 0) {

                print("In");

                Explosion();
            }
        }
    }
    /*

    // プレイヤーがボムからでたら...
    void OnTriggerExit(Collider col) {

        // トリガーが変更されていないなら...
        if(putCollider == false) {

            // ボックスのトリガーをfalseにする
            BoxCollider.isTrigger = false;

            // スフィアのトリガーをtrueにする
            SphereColliderEnabled = true;

            // トリガーを変更したのでfalseにする
            putCollider = true;

            // プレイヤーが再度ボムを発射できるようにする
            plr.shotBomb = true;
        }
    }
    */

    // 3病後に爆発するように設定
    void SetTimer() {


        _timer = new Timer();
        // ３秒で爆発するよう設定
        _timer.LimitTime = 3.0f;
        // 上記で指定した時間に達したとき
        //　Delegateで指定した関数を呼び出す
        _timer.FireDelegate = Explosion;
    }
    
    /// <summary>
    /// キャラによってボムの大きさを変更
    /// </summary>
    /// <param name="name"></param>
    void ChangeScale(string name) {

        switch (name) {

            case "UnityChan":

                SetScale(0.5f, 0.5f, 0.5f);
                break;

            case "Misaki":

                SetScale(0.5f, 0.5f, 0.5f);
                break;

            case "Yuko":

                SetScale(0.7f, 0.7f, 0.7f);
                break;

            default:

                SetScale(0.5f, 0.5f, 0.5f);
                break;
        }
    }

    void Check() {

        if(Vector3.Distance(plr.transform.position,gameObject.transform.position) > 1.25f && putCollider == false) {

            gameObject.layer = 0;

            plr.m_shotBomb = true;

            putCollider = true;
        }
    }
}
