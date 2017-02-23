using UnityEngine;
using System.Collections;
using FixedPhrase;
using MonobitEngine;

/// <summary>
/// チャットで
/// 定型文を管理するクラス
/// </summary>
public class FixedPhraseManager : Origin {

    //========================================================
    // 定数
    //========================================================
    // RPC
    private const string RPC_RECV_FIXEDPHRASE= "RecvFixedPhrase";

    //========================================================
    // リテラル
    //========================================================
    // 定型文オブジェクト
    public GameObject m_fixedPhraseUI;
    // チャットマネージャー
    public ChatScript m_chat;
    // 定型文管理
    FixedPhrase.FixedPhrase m_fixedPhrase = null;

    // Use this for initialization
    void Start () {

        // 非表示にする
        m_fixedPhraseUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        // ルームに入っている時は
        if (MonobitNetwork.inRoom) {

            // 表示する
            m_fixedPhraseUI.SetActive(true);
        // 入ってない時は
        }else {

            //　非表示にする
            m_fixedPhraseUI.SetActive(false);
        }
	}
    // 定型文の設定
    void SetFixedPhrase(FixedPhrase.FixedPhrase fixedPhrase) {

        this.m_fixedPhrase = fixedPhrase;
    }
    // 定型文の呼び出し
    string GetFixedPhrase() {

        return m_fixedPhrase.Phrase();
    }
    // 呼び出したい定型文を引数で設定
    FixedPhrase.FixedPhrase SetFixedPhrase(int i) {

        FixedPhrase.FixedPhrase result = null;

        switch (i) {

            case 0:
                result = new Greeting1();
                break;
            case 1:
                result = new Greeting2();
                break;
            case 2:
                result = new Greeting3();
                break;
            case 3:
                result = new Greeting4();
                break;
            case 4:
                result = new Greeting5();
                break;
            case 5:
                result = new Greeting6();
                break;
            case 6:
                result = new Greeting7();
                break;
            case 7:
                result = new Greeting8();
                break;
            case 8:
                result = new Greeting9();
                break;
            case 9:
                result = new Greeting10();
                break;
            case 10:
                result = new Greeting11();
                break;
            case 11:
                result = new Voice();
                break;
            default:
                break;
        }
        return result;
    }
    /// <summary>
    /// 定型文の呼び出し
    /// </summary>
    /// <param name="i">呼び出したい定型文の番号</param>
    public void Speak(int i) {
        // 定型文を設定
        this.SetFixedPhrase(SetFixedPhrase(i));
        // SEを再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
        // 定型文を送信
        monobitView.RPC(RPC_RECV_FIXEDPHRASE, MonobitTargets.All, MonobitNetwork.player.name, this.GetFixedPhrase());
    }

    [MunRPC]
    /// <summary>
    /// 定型文を受信
    /// </summary>
    /// <param name="senderName">送り主</param>
    /// <param name="chatWord">内容</param>
    void RecvFixedPhrase(string senderName,string chatWord) {

        m_chat.chatLog.Add(senderName + " : " + chatWord);
        if(m_chat.chatLog.Count > 10) {

            m_chat.chatLog.RemoveAt(0);
        }
    }
}
