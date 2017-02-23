using UnityEngine;
using System.Collections;
using MonobitEngine;

public class BoxCreateManager : Origin {

    //========================================================
    // 定数
    //========================================================
    // パス
    private const string BOX_PASS = "Prefabs/cardboardBox_01";
    private const string RBOX_PASS = "Prefabs/cardboardBox_01R";
    private const string FANCE_PASS = "Prefabs/chainlink_group-1";

    // 高さ
    private const float BOX_HEIGHT = -0.185f;
    private const float FANCE_HEIGHT = -0.2f;
    //========================================================
    // リテラル
    //========================================================
    // ボックス配置パターン
    public int m_putton;
	// Use this for initialization
	void Start () {

        if (!MonobitEngine.MonobitNetwork.isHost) { return; }

        BoxCreate();
	}

    void BoxCreate() {

        switch (m_putton) {

            case 0:
                GameObject boxObj0 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj1 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(2, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj2 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(4, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj3 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-2, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj4 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-4, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj5 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, -2), Quaternion.identity, 0, null);
                GameObject boxObj6 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, -4), Quaternion.identity, 0, null);
                GameObject boxObj7 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, 2), Quaternion.identity, 0, null);
                GameObject boxObj8 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, 4), Quaternion.identity, 0, null);
                // ここから壊れない箱
                GameObject rboxObj0 = MonobitNetwork.Instantiate(RBOX_PASS, new Vector3(-2, BOX_HEIGHT, 2), Quaternion.identity, 0, null);
                GameObject rboxObj1 = MonobitNetwork.Instantiate(RBOX_PASS, new Vector3(2, BOX_HEIGHT, 2), Quaternion.identity, 0, null);
                GameObject rboxObj2 = MonobitNetwork.Instantiate(RBOX_PASS, new Vector3(2, BOX_HEIGHT, -2), Quaternion.identity, 0, null);
                GameObject rboxObj3 = MonobitNetwork.Instantiate(RBOX_PASS, new Vector3(-2, BOX_HEIGHT, -2), Quaternion.identity, 0, null);
                break;
            case 1:
                GameObject boxObj9 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj10 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-2, BOX_HEIGHT, 4), Quaternion.identity, 0, null);
                GameObject boxObj11 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, 4), Quaternion.identity, 0, null);
                GameObject boxObj12 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(2, BOX_HEIGHT, 4), Quaternion.identity, 0, null);
                GameObject boxObj13 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-4, BOX_HEIGHT, 2), Quaternion.identity, 0, null);
                GameObject boxObj14 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(4, BOX_HEIGHT, 2), Quaternion.identity, 0, null);
                GameObject boxObj15 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-4, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj16 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(4, BOX_HEIGHT, 0), Quaternion.identity, 0, null);
                GameObject boxObj17 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-4, BOX_HEIGHT, -2), Quaternion.identity, 0, null);
                GameObject boxObj18 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(4, BOX_HEIGHT, -2), Quaternion.identity, 0, null);
                GameObject boxObj19 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(-2, BOX_HEIGHT, -4), Quaternion.identity, 0, null);
                GameObject boxObj20 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(0, BOX_HEIGHT, -4), Quaternion.identity, 0, null);
                GameObject boxObj21 = MonobitNetwork.Instantiate(BOX_PASS, new Vector3(2, BOX_HEIGHT, -4), Quaternion.identity, 0, null);

                // ここからフェンス
                GameObject Fance0 = MonobitNetwork.Instantiate(FANCE_PASS, new Vector3(0.04f, FANCE_HEIGHT, -2), Quaternion.identity, 0, null);
                GameObject Fance1 = MonobitNetwork.Instantiate(FANCE_PASS, new Vector3(2, FANCE_HEIGHT, -0.05f), Quaternion.Euler(0,90,0), 0, null);
                GameObject Fance2 = MonobitNetwork.Instantiate(FANCE_PASS, new Vector3(0.04f, FANCE_HEIGHT, 2), Quaternion.identity, 0, null);
                break;
        }
    }
}
