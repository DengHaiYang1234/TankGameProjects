using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    [HeaderAttribute("生命")]
    float HP = 100f;

    private float maxHp = 100f;

    [HeaderAttribute("炮塔")]
    public GameObject Turret;
    [HeaderAttribute("炮")]
    public GameObject MainGun;
    [HeaderAttribute("炮塔Axis")]
    public Vector3 TurretBaseAxis = new Vector3(0, 1, 0);
    [HeaderAttribute("炮Axis")]
    public Vector3 MainGunBaseAxis = new Vector3(1, 0, 0);

    [HeaderAttribute("炮塔旋转速度")]
    public float TurretSpeed = 20;
    [HeaderAttribute("坦克移动速度")]
    public float TankSpeed = 10;
    [HeaderAttribute("坦克转向速度")]
    public float TurnSpeed = 20;
    [HeaderAttribute("坦克大炮最小转向")]
    public float MainGunMinTurnX = -10;
    [HeaderAttribute("坦克大炮最大转向")]
    public float MainGunMaxTurnX = 45;

    private Vector3 turrentEulerAngle;
    private Vector3 mainGunEulerAngle;
    private Vector3 positionTemp;

    private float rotationX = 0;

    //履带
    private TankTrackAnimation tracks;

    [HeaderAttribute("炮弹预设")]
    public GameObject bullet;
    [HeaderAttribute("炮口")]
    public GameObject Muzzle;
    [HeaderAttribute("上一次开炮时间")]
    public float lastShootTime = 0;
    [HeaderAttribute("开炮的时间间隔")]
    public float shootInterval = 0.5f;
    [HeaderAttribute("燃烧特效")]
    public GameObject fireEffect;

    public Texture2D centerSight;
    public Texture2D tankSight;
    public Texture2D killUI;
    private float killUIStartTime = float.MinValue;

    [HeaderAttribute("准心偏差X")]
    public float offestX = 0f;
    [HeaderAttribute("准心偏差Y")]
    public float offestY = 0f;
    public ControllType cType = ControllType.player;
    [HeaderAttribute("血量UI")]
    public GameObject HPObj;

    private Text hpTxt;


    public enum ControllType
    {
        none,
        player,
        computer
    }

    private void Awake()
    {
        tracks = GetComponentInChildren<TankTrackAnimation>();
    }

    private void Start()
    {
        if (Turret != null)
        {
            turrentEulerAngle = Turret.transform.localEulerAngles;
        }

        if (MainGun != null)
            mainGunEulerAngle = MainGun.transform.localEulerAngles;
        hpTxt = HPObj.GetComponent<Text>();
        hpTxt.text = HP.ToString();
        //wheels = transform.Find("wheels");
    }

    //移动
    public void Move(Vector2 moveVector)
    {
        positionTemp = this.transform.position;
        this.transform.Rotate(new Vector3(0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
        this.transform.position += this.transform.forward * moveVector.y * TankSpeed * Time.deltaTime;
        //履带滚动
        if (tracks)
            tracks.MoveTrack(new Vector2(Mathf.Clamp(moveVector.y + moveVector.x, -1, 1), 0));
    }
    //Tank根据炮筒转向
    public void Aim(Vector2 aimVector)
    {
        if (Turret != null)
        {
            float rotationY = Turret.transform.localEulerAngles.y + aimVector.x * TurretSpeed * Time.deltaTime;
            Turret.transform.localEulerAngles =
            new Vector3
                (
                    (TurretBaseAxis.x * rotationY) + ((1 - TurretBaseAxis.x) * turrentEulerAngle.x),
                    (TurretBaseAxis.y * rotationY) + ((1 - TurretBaseAxis.y) * turrentEulerAngle.y),
                    (TurretBaseAxis.z * rotationY) + ((1 - TurretBaseAxis.z) * turrentEulerAngle.z)
                );
        }
        if(MainGun != null)
        {
            rotationX += aimVector.y * TurretSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, MainGunMinTurnX, MainGunMaxTurnX);

            MainGun.transform.localEulerAngles =
                new Vector3(
                    (MainGunBaseAxis.x * -rotationX) + mainGunEulerAngle.x,
                    (MainGunBaseAxis.y * -rotationX) + mainGunEulerAngle.y,
                    (MainGunBaseAxis.z * -rotationX) + mainGunEulerAngle.z
                    );
        }
    }

    //发射炮弹的时间间隔
    public void Shoot()
    {
        if (Time.time - lastShootTime < shootInterval)
            return;

        if (bullet == null)
            return;

        Vector3 pos = Muzzle.transform.position + Muzzle.transform.forward * 5;

        GameObject bulletObj = (GameObject) Instantiate(bullet, pos, Muzzle.transform.rotation);

        Bullet bulletCmp = bulletObj.GetComponent<Bullet>();
        if (bulletCmp != null)
            bulletCmp.attackTank = this.gameObject;

        lastShootTime = Time.time;
    }

    //坦克受到攻击
    public void BeAttacked(float att,GameObject attackTank)
    {
        if (HP <= 0)
            return;

        if (HP > 0)
            HP -= att;

        hpTxt.text = HP.ToString();

        if (HP <= 0)
        {
            GameObject destoryObj = (GameObject) Instantiate(fireEffect);
            destoryObj.transform.SetParent(transform, false);
            destoryObj.transform.localPosition = Vector3.zero;
            cType = ControllType.none;

            if (attackTank != null)
            {
                Tank tankCmp = attackTank.GetComponent<Tank>();
                if (tankCmp != null && tankCmp.cType == ControllType.player)
                    tankCmp.StartDrawKill();
            }
        }
    }



    public Vector3 CalExploadePoint()
    {
        Vector3 hitpoint = Vector3.zero;
        RaycastHit rayCastHit;
        Vector3 pos = Muzzle.transform.position + Muzzle.transform.forward * 5;

        Ray ray = new Ray(pos, Muzzle.transform.forward);

        if (Physics.Raycast(ray, out rayCastHit, 1000f))
        {
            hitpoint = rayCastHit.point;
        }
        else
        {
            hitpoint = ray.GetPoint(1000f);
        }

        return hitpoint;
    }

    public void DrawSight()
    {
        Vector3 explodePoint = CalExploadePoint();
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(explodePoint);

        Rect tankRect = new Rect(screenPoint.x - tankSight.width/2, Screen.height - screenPoint.y - tankSight.height/2,
            tankSight.width, tankSight.height
            );


        GUI.DrawTexture(tankRect, tankSight);

        Rect centerRect = new Rect((Screen.width / 2) - (centerSight.width / 2) + offestX, (Screen.height / 2) - (centerSight.height / 2) + offestY,
            centerSight.width, centerSight.height);


        GUI.DrawTexture(centerRect, centerSight);
    }


    public void StartDrawKill()
    {
        killUIStartTime = Time.time;
    }

    private void DrawKillUI()
    {
        if (Time.time - killUIStartTime < 1f)
        {
            Rect rect = new Rect(Screen.width/2 - killUI.width/2, 30, killUI.width, killUI.height);

            GUI.DrawTexture(rect, killUI);
        }
    }


    private void OnGUI()
    {
        if (cType != ControllType.player)
            return;
        DrawSight();
    }

}


