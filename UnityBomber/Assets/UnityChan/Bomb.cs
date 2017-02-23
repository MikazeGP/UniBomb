using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

    public GameObject pbomb; 

    public static int count; // 発射間隔

    int frame; // フレーム

    // スクリプトが有効になったとき一回だけ呼ばれます
    void Start()
    {
        frame = 0;
        count = 10;
    }
    // 毎フレーム呼ばれます
    void Update()
    {
        frame++; // フレームをカウント

        //Zキーを押しているときかつ10フレームごと
        if (Input.GetKey(KeyCode.Z) && frame % count == 0)
        {
            Instantiate(pbomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // プレイヤーの位置に弾を生成します 
        }
    }
}
