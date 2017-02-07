using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OriginMgr<Type> where Type : Origin {

    int _size = 0;
    GameObject _prefab = null;
    List<Type> _pool = null;

    public delegate void FuncT(Type t);

    /// コンストラクタ
    /// プレハブは必ず"Resources/Prefabs/"に配置すること
    public OriginMgr(string prefabName,int size = 0) {

        _size = size;
        _prefab = Resources.Load("Prefabs/" + prefabName) as GameObject;
        //_prefab = Resources.Load(prefabName) as GameObject;

        if (_prefab == null) {

            Debug.Log("Not found prefab. name = " + prefabName);
        }

        _pool = new List<Type>();

        if(size > 0) {

            // サイズ指定があれば固定アロケーションとする
            for(int i = 0; i < size; i++) {

                GameObject g = GameObject.Instantiate(_prefab, new Vector3(), Quaternion.identity) as GameObject;
                //GameObject g = MonobitEngine.MonobitNetwork.Instantiate(_prefab.name, new Vector3(), Quaternion.identity, 0) as GameObject;
                

                Type obj = g.GetComponent<Type>();
                obj.vanishCannnotOverride();
                _pool.Add(obj);


            }
        }
    }

    /// オブジェクトを再利用する
    Type _Recycle(Type obj,float x,float y ,float z,float direction,float angle,float speed) {

        // 復活
        obj.Revive();
        obj.SetPosion(x, y, z);

        if(obj.RigidBody != null) {

            // Rigidbody があるときのみ速度を設定する
            //obj.SetVelocityXYZ()
        }
        obj.Angle = angle;
        obj.DirectionX = direction;

        return obj;

    }

    /// インスタンスを取得する
    public Type Add(float x,float y,float z,float direction = 0.0f,float angle = 0.0f,float speed = 0.0f) {


        foreach(Type obj in _pool) {

            if(obj.Exists == false) {

                return _Recycle(obj, x, y, z, direction, angle, speed);

            }
        }

        if(_size == 0) {

            // 自動で拡張
            GameObject g = GameObject.Instantiate(_prefab, new Vector3(), Quaternion.identity) as GameObject;
            Type obj = g.GetComponent<Type>();
            _pool.Add(obj);
            return _Recycle(obj, x, y, z, direction, angle, speed);
        }
        return null;
    }

    /// 生存するインスタンスに対してラムダ式を実行する
    public void ForEachExitst(FuncT func){


        foreach (var obj in _pool){

            if (obj.Exists){

                func(obj);
            }
        }
    }

    /// 生存しているインスタンスをすべて破棄する
    public void Vanish(){


        ForEachExitst(t => t.Vanish());
    }

    /// インスタンス生存数を取得する
    public int Count()
    {

        int ret = 0;
        ForEachExitst(t => ret++);

        return ret;
    }
}