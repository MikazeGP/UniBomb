using UnityEngine;
using System.Collections;
using MonobitEngine;

public class Origin : MonobitEngine.MonoBehaviour {

    //========================================================
    // 定数
    //========================================================
    // インプット
    public const string AXIS_HORIZONTAL = "Horizontal";
    public const string AXIS_VERTICAL = "Vertical";
    public const string FIRE1_BUTTON = "Fire1";
    public const string FIRE2_BUTTON = "Fire2";
    public const string FIRE3_BUTTON = "Fire3";

    /// シーン
    public const string TITLE_SCENE = "Title";
    public const string MODESELECT_SCENE = "ModeSelect";
    public const string OPSION_SCENE = "Opsion";
    public const string CHATROOM_SCENE = "ChatRoom";
    public const string CHARA_SELECT_SCENE = "CharaSelect";
    public const string STAGE_SELECT_SCENE = "StageSelect";
    public const string STAGE1_SCENE = "Stage1";
    public const string STAGE1_1_SCENE = "Stage1.1";
    public const string STAGE1_2_SCENE = "Stage1.2";
    public const string STAGE2_SCENE = "Stage2";
    public const string RESULT_SCENE = "Result";

    //　タグ
    public const string BOMB_TAG = "Bomb";
    public const string EXPLOSION_TAG = "Explosion";
    public const string PLAYER_TAG = "Player";

    // カラー
    // 透明
    public Color TRANSPARENT_COLOR = new Color(1, 1, 1, 0);
    // 不透明
    public Color OPAQUE_COLOR = new Color(1, 1, 1, 1);
    

    //========================================================
    // 定数はここまで
    //========================================================

    /// プレハブ取得.
    /// プレハブは必ず"Resources/Prefabs/"に配置すること.
    public static GameObject GetPrefab(GameObject prefab, string name)
    {

        //「??」は「NULL合体演算子」左の項に指定された変数が NULLの場合は
        // 右の項を返す演算子


        return prefab ?? (prefab = Resources.Load("Prefabs/" + name) as GameObject);
    }

    /// インスタンスを生成してスクリプトを返す
    public static Type CreateInstance<Type>(GameObject prefab, Vector3 p, float direction = 0.0f, float speed = 0.0f) where Type : Origin
    {

        GameObject g = Instantiate(prefab, p, Quaternion.identity) as GameObject;
        Type obj = g.GetComponent<Type>();
        obj.SetVelocity(direction, speed);
        return obj;

    }

    public static Type CreateInstance2<Type>(GameObject prefab, float x, float y, float z,float direction = 0.0f, float speed = 0.0f) where Type : Origin
    {

        Vector3 pos = new Vector3(x, y, z);
        return CreateInstance<Type>(prefab, pos, direction, speed);
    }

    /// 生存フラグ
    bool _exists = true;

    /// 生存プロパティ
    public bool Exists
    {

        get { return _exists; }
        set { _exists = value; }
    }
    /// 座標(X)
    public float X
    {
        set
        {

            Vector3 pos = transform.position;
            pos.x = value;
            transform.position = pos;
        }
        get { return transform.position.x; }
    }

    /// 座標(Y)
    public float Y
    {
        set
        {

            Vector3 pos = transform.position;
            pos.y = value;
            transform.position = pos;
        }
        get { return transform.position.y; }
    }

    /// 座標(Z)
    public float Z {

        set {
            Vector3 pos = transform.position;
            pos.z = value;
            transform.position = pos;
        }
        get { return transform.position.z; }
    }


    /// 座標を足しこむ。
    public void AddPosion(float dx, float dy,float dz)
    {

        X += dx;
        Y += dy;
        Z += dz;
    }

    /// 座標を設定する
    public void SetPosion(float x, float y,float z)
    {

        Vector3 pos = transform.position;
        pos.Set(x, y, z);
        transform.position = pos;
    }

    /// スケール値(X)
    public float ScaleX
    {
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;

        }
        get { return transform.localScale.x; }
    }

    /// スケール値(Y)
    public float ScaleY
    {
        set
        {
            Vector3 scale = transform.localScale;
            scale.y = value;
            transform.localScale = scale;

        }
        get { return transform.localScale.y; }
    }

    /// スケール値(Z)
    public float ScaleZ {

        set {
            Vector3 scale = transform.localScale;
            scale.z = value;
            transform.localScale = scale;
                
        }
        get { return transform.localScale.z; }
    }

    /// スケール値を設定
    public void SetScale(float x, float y,float z)
    {

        Vector3 scale = transform.localScale;
        scale.Set(x, y, z);
        transform.localScale = scale;
    }

    /// スケールを取得
    public Vector3 GetScale
    {

        get { return transform.localScale; }
    }

    /// スケール値(X/Y/Z)
    public float Scale
    {
        get
        {

            Vector3 scale = transform.localScale;
            return (scale.x + scale.y + scale.z) / 2.0f;
        }
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            scale.y = value;
            scale.z = value;
            transform.localScale = scale;
        }
    }

    /// スケール値を足しこむ
    public void AddScale(float d)
    {

        Vector3 scale = transform.localScale;
        scale.x += d;
        scale.y += d;
        scale.z += d;
        transform.localScale = scale;
    }

    /// スケール値をかける
    public void MulScale(float d)
    {

        transform.localScale *= d;
    }

    /// 剛体
    Rigidbody _rigidBody = null;

    public Rigidbody RigidBody
    {
        get { return _rigidBody ?? (_rigidBody = gameObject.GetComponent<Rigidbody>()); }
    }

    /// 移動量を設定
    public void SetVelocity(float direction, float speed)
    {

        Vector3 v;
        v.x = Util.CosEx(direction) * speed;
        v.y = 0.0f;
        v.z = Util.SinEx(direction) * speed;
        RigidBody.velocity = v;

    }

    /// 移動量を設定(X/Y) 
    public void SetVelocityXYZ(float vx, float vy,float vz)
    {

        Vector3 v;
        v.x = vx;
        v.y = vy;
        v.z = vz;
        RigidBody.velocity = v;
    }

    /// 移動量をかける
    public void MulVelocity(float d)
    {

        RigidBody.velocity *= d;
    }

    /// 移動量(X)
    public float VX
    {

        get { return RigidBody.velocity.x; }
        set
        {
            Vector3 v = RigidBody.velocity;
            v.x = value;
            RigidBody.velocity = v;
        }
    }

    ///　移動量(Y)
    public float VY
    {

        get { return RigidBody.velocity.y; }
        set
        {
            Vector3 v = RigidBody.velocity;
            v.y = value;
            RigidBody.velocity = v;
        }
    }

    /// 移動量(Z)
    public float VZ {

        get { return RigidBody.velocity.z; }
        set {
            Vector3 v = RigidBody.velocity;
            v.z = value;
            RigidBody.velocity = v;
        }
    }

    /// 方向
    public float Direction
    {
        get
        {

            Vector3 v = RigidBody.velocity;
            // Atan2(float y, float x)...Tanがy/xになる角度をラジアンで返す
            // Rad2Deg...ラジアンから度に変換する定数
            return Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
        }
    }

    /// 速度
    public float Speed
    {

        get
        {

            Vector3 v = RigidBody.velocity;
            // Sqrt(float x)...fの平方根を返す
            return Mathf.Sqrt(v.x * v.x + v.z * v.z);
        }
    }

    /// 回転角度
    public float Angle
    {

        // eulerAngles...x、y、z の値により、Z軸で z 度、X軸で x 度、Y軸で y 度 回転
        set { transform.eulerAngles = new Vector3(0, 0, value); }
        get { return transform.eulerAngles.z; }

    }

    public float DirectionX {

        set { transform.eulerAngles = new Vector3(value, 0, 0); }
        get { return transform.eulerAngles.x; }
    }

    public float DirectionY
    {

        set { transform.eulerAngles = new Vector3(0, value, 0); }
        get { return transform.eulerAngles.y; }
    }

    /// コリジョン(円)
    SphereCollider _sphereCollider = null;

    public SphereCollider SphereCollider
    {

        get { return _sphereCollider ?? (_sphereCollider = GetComponent<SphereCollider>()); }
    }

    /// 円コリジョンを取り付ける
    public void AddCircleCollider()
    {

        gameObject.AddComponent<SphereCollider>();
    }

    // 円コリジョンの半径
    public float CollisionRadius
    {

        get { return SphereCollider.radius; }
        set { SphereCollider.radius = value; }
    }

    // 円コリジョンの有効無効を設定する
    public bool SphereColliderEnabled
    {

        get { return SphereCollider.enabled; }
        set { SphereCollider.enabled = value; }
    }

    /// 円コリジョンのトリガーを設定する
    public bool SphereColliderIsTrigger
    {

        get { return SphereCollider.isTrigger; }
        set { SphereCollider.isTrigger = value; }
    }

    /// コリジョン(矩形)
    BoxCollider _boxCollider = null;

    public BoxCollider BoxCollider
    {

        get { return _boxCollider ?? (_boxCollider = GetComponent<BoxCollider>()); }
    }

    /// 矩形コリジョンを取り付ける
    public void AddBoxCollider()
    {

        gameObject.AddComponent<BoxCollider>();
    }

    /// 矩形コリジョンの幅.
    public float BoxColliderX
    {

        get { return BoxCollider.size.x; }
        set
        {
            var size = BoxCollider.size;
            size.x = value;
            BoxCollider.size = size;
        }
    }

    /// 矩形コリジョンの高さ
    public float BoxColliderY
    {

        get { return BoxCollider.size.y; }
        set
        {
            var size = BoxCollider.size;
            size.y = value;
            BoxCollider.size = size;
        }
    }

    public float BoxColliderZ
    {

        get { return BoxCollider.size.z; }
        set
        {
            var size = BoxCollider.size;
            size.z = value;
            BoxCollider.size = size;
        }
    }

    // 箱コリジョンのサイズを設定
    public void SetBoxColliderSize(float x, float y,float z)
    {

        BoxCollider.size.Set(x,y,z);
    }

    // 箱コリジョンのオフセットを設定
    public void SetBoxColliderOffset(float x, float y,float z)
    {

        BoxCollider.center.Set(x,y,z);
    }

    /// 箱コリジョンの有効無効を設定する
    public bool BoxColliderEnabled
    {

        get { return BoxCollider.enabled; }
        set { BoxCollider.enabled = value; }
    }

    /// 箱コリジョンのトリガーを設定する
    public bool BoxColliderIsTrigger
    {

        get { return BoxCollider.isTrigger; }
        set { BoxCollider.isTrigger = value; }
    }

    /// 移動して画面内に収めるようにする.
    public void ClampScreenAndMove(Vector3 v, Vector3 minPos, Vector3 maxPos)
    {
        Vector3 min = minPos;
        Vector3 max = maxPos;
        Vector3 pos = transform.position;
        pos += v;

        // 画面内に収まるように制限をかける.
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        pos.z = Mathf.Clamp(pos.z, min.z, max.z);

        // プレイヤーの座標を反映.
        transform.position = pos;
    }

    ///　消滅 (メモリから削除)
    public void DestroyObj()
    {

        Destroy(gameObject);
    }

    /// アクティブにする
    public virtual void Revive()
    {

        gameObject.SetActive(true);
        Exists = true;
    }

    /// 消滅する(オーバーライド可能)
    /// ただし base.Vanish()を呼ばないと消滅しなくなることに注意
    public virtual void Vanish()
    {

        vanishCannnotOverride();
    }

    /// 消滅する(オーバーライド禁止)
    public void vanishCannnotOverride()
    {

        gameObject.SetActive(false);
        Exists = false;
    }

    /// アニメーターの設定
    Animator _animator = null;

    public Animator Animator
    {

        get { return _animator ?? (_animator = gameObject.GetComponent<Animator>()); }
    }

    // フレームカウント
    public int FrameCount;


    /// プレイヤーのIDを取得
    /// ルームに接続しているときのみ使用可能
    /// 1～4
    public int GetPlayerId {

        get { return MonobitNetwork.player.ID; }
    }

    /// 配列用プレイヤーのIDを取得
    /// ただしIDは1から始まるので
    /// -1して配列の0番目が使えるようにする
    /// 本来のプレイヤーIDを取得したい場合は
    /// 上のを使用すること
    public int PlayerId {

        get { int id = MonobitNetwork.player.ID - 1;

            return id;
        }

        private set { }
    }

    /// プレイヤー名を取得 
    /// サーバーに接続しているときのみ使用可能
    public string GetPlayerName {

        get { return MonobitNetwork.player.name; }
    }
}
