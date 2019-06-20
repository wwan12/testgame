using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour
{
    private float NextFire;
    private float FireRate;
    private float BulletSpeed;
    private GameObject Bullet;
    public AttackMode attackMode;
    public bool isContact;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Luanch()
    {
        NextFire += Time.fixedDeltaTime;
        //如果按下了鼠标左键且计时器大于发射间隙
        if (Input.GetMouseButton(0) && NextFire > FireRate)
        {
            //记录鼠标位置
            Vector3 direction = Input.mousePosition;

            //计时器归零
            NextFire = 0;

            //生成子弹
            GameObject b = Instantiate(Bullet, gameObject.transform.position, Quaternion.identity) as GameObject;

            //子弹速度由鼠标点击的位置减去屏幕的宽高的1/2得到
            //主要就是坐标的转换
            b.GetComponent<Rigidbody2D>().velocity = (new Vector3(direction.x - Camera.main.pixelWidth / 2, direction.y - Camera.main.pixelHeight / 2, 0).normalized * BulletSpeed);
            //将所有子弹放在一个父物体下，方便操作
            b.transform.SetParent(GameObject.Find("Bullets").transform);

        }
    }

    public enum AttackMode
    {

    }
}
