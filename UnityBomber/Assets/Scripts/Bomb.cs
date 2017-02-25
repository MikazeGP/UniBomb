using UnityEngine;
using System.Collections;

public class Bomb : Origin {

    public GameObject colObj;
    public bool m_putCollider;
    public Player m_plr;
    Timer m_timer;
    private string m_playerName;

    void Start() {

        m_putCollider = false;

        m_playerName = GrobalData.Instance._plrCharaName;

        this.SetTimer();

        AudioManager.Instance.PlaySE(AUDIO.SE_PUTBOMB);

        if (!monobitView.isMine){

            return;
        }
        ChangeScale(m_playerName);
    }

    void Update() {

        Check();

        if (m_timer.Update()) {

        }
    }

    public void Explosion() {

        if (!monobitView.isMine){

            return;
        }

        // エフェクトを生成
        GameObject effect = MonobitEngine.MonobitNetwork.Instantiate("Prefabs/ring1", gameObject.transform.position, Quaternion.identity, 0, null);

        effect.GetComponent<Effect>().m_plr = m_plr;

        MonobitEngine.MonobitNetwork.Destroy(gameObject);

        // 置くことができるボムを増やす
        m_plr.m_stockBomb++;

    }

    void OnTriggerEnter(Collider col){

        // 爆発エフェクトに触れたら...
        if (col.tag == "Explosion") {

            Explosion();
        }
    }

    // 3病後に爆発するように設定
    void SetTimer() {

        m_timer = new Timer();
        // みさきのみ2秒で爆発するよう設定
        if (m_playerName == "Misaki") {

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

    void Check() {

        if (!monobitView.isMine) {

            return;
        }
        if(Vector3.Distance(m_plr.transform.position,gameObject.transform.position) > 1.25f && m_putCollider == false) {

            gameObject.layer = 0;

            m_plr.m_shotBomb = true;

            m_putCollider = true;
        }
    }
}
