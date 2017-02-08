using UnityEngine;
using System.Collections;

public class Effect : Origin {

    private Timer m_timer;
    public Player m_plr;
    private string m_playerName;

    // 親オブジェクト
    //public static OriginMgr<Effect> parent = null;

    // インスタンスを取得
    /*
    public static Effect Add(float x, float y, float z, float direction, float speed){

        return parent.Add(x, y, z, direction, 0, speed);
    }*/

    // Use this for initialization
    void Start () {

        this.SetTimer();

        if (!monobitView.isMine) {

            return;
        }

        m_playerName = GrobalData.Instance._plrCharaName;
        
        ChangeScale(m_playerName);

        // キャラに応じて再生する爆発音を変更する
        switch (m_playerName)
        {

            case "UnityChan":

                // ここで音を再生
                AudioManager.Instance.PlaySE(AUDIO.SE_UTC_RUPTURE);

                break;

            case "Misaki":

                AudioManager.Instance.PlaySE(AUDIO.SE_MISAK_RUPTURE);

                break;

            case "Yuko":

                AudioManager.Instance.PlaySE(AUDIO.SE_YUKO_RUPTURE);

                break;

            default:

                AudioManager.Instance.PlaySE(AUDIO.SE_UTC_RUPTURE);

                break;
        }
    }

    // Update is called once per frame
    void Update () {

        if (m_timer.Update()) {

        }
	}
    
    // 終了処理
    void End() {

        //Vanish();

        MonobitEngine.MonobitNetwork.Destroy(gameObject);
        //Destroy(this);
    }

    void OnTriggerEnter(Collider col) {

    }

    // タイマーを設定
    void SetTimer() {

        m_timer = new Timer();
        m_timer.LimitTime = 1.05f;
        m_timer.FireDelegate = End;
    }

    // キャラに応じてエフェクトの大きさを変更
    void ChangeScale(string name) {

        switch (name)
        {

            case "UnityChan":

                SetScale(1.3f, 1.3f, 1.3f);
                break;

            case "Misaki":

                SetScale(1.1f, 1.1f, 1.1f);
                break;

            case "Yuko":

                SetScale(1.7f, 1.7f, 1.7f);
                break;

            default:

                SetScale(1, 1, 1);
                break;
        }
    }
}
