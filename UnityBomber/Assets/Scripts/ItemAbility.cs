using UnityEngine;
using System.Collections;
using System;

namespace ItemAbility{

    // 基底クラス
    public abstract class ItemAbility{

        public abstract void SetAbility(Player plr);

        protected int m_itemNum(){

            int random = UnityEngine.Random.Range(0, 100);
            if (random < 31) { return 1; }
            else if (30 < random && random < 64) { return 2; }
            else if (63 < random && random < 96) { return 3; }
            else if (95 < random && random < 99) { return 4; }
            else { return 5; }
        }

        protected int m_itemNum2(){

            int random = UnityEngine.Random.Range(0, 100);
            if (random < 24) { return 2; }
            else if (23 < random && random < 48) { return 2; }
            else if (47 < random && random < 72) { return 2; }
            else if (72 < random && random < 94) { return 4; }
            else if (95 < random && random < 97) { return 5; }
            else { return 6; }
        }
    }
    // 抽象クラス
    // プレイヤーBuffクラス
    public class UpAbility : ItemAbility
    {

        public override void SetAbility(Player plr){

            AudioManager.Instance.PlaySE(AUDIO.SE_BUFF);

            switch (m_itemNum()){

                case 1:
                    // 移動速度buff
                    plr.m_moveSpeed = plr.m_moveSpeed * 1.1f;
                    Debug.Log("スピードアップ");
                    break;
                case 2:
                    // ボムの残機を+1する
                    plr.m_stockBomb++;
                    Debug.Log("ボム数アップ");
                    break;
                case 3:
                    // ボムの火力をbuff
                    plr.m_bombPower = plr.m_bombPower * 1.1f;
                    Debug.Log("火力アップ");
                    break;
                case 4:
                    // なし(はずれ)
                    Debug.Log("はずれ");
                    break;
                case 5:
                    // 全ステータスBuff
                    plr.m_moveSpeed = plr.m_moveSpeed * 1.1f;
                    plr.m_stockBomb++;
                    plr.m_bombPower = plr.m_bombPower * 1.1f;
                    Debug.Log("全ステータスアップ");
                    break;
            }
        }
    }
    // 抽象クラス
    // プレイヤーNuffクラス
    public class DownAbility : ItemAbility{

        private Timer m_timer;
        private Player m_plr;
        private float m_backUpSpeed;
        private int m_num;

        public override void SetAbility(Player plr){

            AudioManager.Instance.PlaySE(AUDIO.SE_NUFF);

            m_plr = plr;
            switch (m_itemNum2()){

                case 1:
                    //移動速度Nuff
                    m_plr.m_moveSpeed = m_plr.m_moveSpeed * 0.9f;
                    Debug.Log("スピードダウン");
                    break;
                case 2:
                    // ボム残機を1減らす
                    m_plr.m_stockBomb--;
                    // ボムは1より少なくならない
                    if (m_plr.m_stockBomb < 1) { m_plr.m_stockBomb = 1; }
                    Debug.Log("ボム数ダウン");
                    break;
                case 3:
                    // ボムの火力をNuff
                    m_plr.m_bombPower = m_plr.m_bombPower * 0.9f;
                    Debug.Log("火力ダウン");
                    break;
                case 4:
                    m_num = 4;
                    // 移動方向を反転する
                    Debug.Log("移動反転");
                    m_plr.m_moveSpeed = -m_plr.m_moveSpeed;
                    break;
                case 5:
                    m_num = 5;
                    Debug.Log("あたり");
                    break;
                case 6:
                    // 全ステータスをNuff
                    m_plr.m_moveSpeed = m_plr.m_moveSpeed * 0.9f;
                    m_plr.m_stockBomb--;
                    m_plr.m_bombPower = m_plr.m_bombPower * 0.9f;
                    Debug.Log("全ステータスダウン");
                    break;
            }
        }
    }
}
