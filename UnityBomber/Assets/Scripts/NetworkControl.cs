using UnityEngine;
using System.Collections;
using MonobitEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


class NetworkControl : Origin
{

    /** プレイヤーキャラクタ. */
    private GameObject playerObject = null;

    /** プレイヤーキャラクタ名. */
    private string m_playerName = "";

    /** プレイヤーNo */
    private int m_playerNo = 0;

    public GameMgr m_gameMgr;

    // 初期化処理
    void Start()
    {
        print(PlayerId);
        /*
        if(m_gameMgr.m_useCharaName[PlayerId] != "") {

            m_playerName = m_gameMgr.m_useCharaName[PlayerId];

        }else {
            m_playerName = GrobalData.Instance._useCharaName[PlayerId];
        }*/
        m_playerName = GrobalData.Instance._plrCharaName;

        MonobitEngine.MonobitNetwork.sendRate = 60;
        MonobitEngine.MonobitNetwork.updateStreamRate = 600;
    }

    // 更新処理
    void Update()
    {
        // MUNサーバに接続しており、かつルームに入室している場合
        if (MonobitNetwork.isConnect && MonobitNetwork.inRoom)
        {
            //if(GrobalData.Instance._plrStock[PlayerId] == 0) { return; }
            // プレイヤーキャラクタが未登場の場合に登場させる
            if (playerObject == null && m_gameMgr.m_dieFlag[PlayerId] != true){

                // グローバルデータからIDを読み込む
                m_playerNo = MonobitNetwork.player.ID;
                // プレイヤーを生成
                GameObject m_plrObj = MonobitNetwork.Instantiate("Prefabs/" + m_playerName, SpawnPos(m_playerNo), SpawnQuat(m_playerNo), 0);

                playerObject = m_plrObj;

            }
        }
    }

    /// <summary>
    /// プレイヤーの初期配置を設定
    /// </summary>
    /// <param name="num">プレイヤーID</param>
    /// <returns></returns>
    Vector3 SpawnPos(int num)
    {
        // 初期位置を原点にする
        Vector3 Pos = Vector3.zero;

        switch (num)
        {

            case 1:
                Pos = new Vector3(-4.8f, -0.17f, 4.3f);
                break;

            case 2:
                Pos = new Vector3(4.8f, -0.17f, -4.3f);
                break;

            case 3:
                Pos = new Vector3(4.8f, -0.17f, 4.3f);
                break;

            case 4:
                Pos = new Vector3(-4.8f, -0.17f, -4.3f);
                break;

            default:
                break;
        }

        return Pos;
    }

    /// <summary>
    /// プレイヤーの初期の向きを設定
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    Quaternion SpawnQuat(int num)
    {

        Quaternion Pos = new Quaternion(0, 0, 0, 0);

        switch (num)
        {

            case 1:
                Pos = Quaternion.Euler(0, 135, 0);
                break;

            case 2:
                Pos = Quaternion.Euler(0, 315, 0);
                break;

            case 3:
                Pos = Quaternion.Euler(0, 225, 0);
                break;

            case 4:
                Pos = Quaternion.Euler(0, 45, 0);
                break;

            default:
                break;
        }

        return Pos;
    }
}