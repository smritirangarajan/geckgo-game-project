
using UnityEngine;

public class PuddleManager : MonoBehaviour
{

    public GameObject puddle;
    #region randomPos
    UnityEngine.Vector3 leftBottom;
    UnityEngine.Vector3 topRight;
    UnityEngine.Vector2 spawnPosition;
    float randomX;
    float yAboveMedium=3;
    float yAbove;

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    #region timer
    float spawnTime=3f;
    float timer=0;
    #endregion

    void Start()
    {
        topRight=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0));
        leftBottom=Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));//�õ�����������
        yAbove=yAboveMedium+topRight.y;
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>=spawnTime){
        randomX=Random.Range(leftBottom.x,topRight.x);//�������һ��x
        spawnPosition=new Vector2(randomX,yAbove);//�����������λ��
        Instantiate(puddle,spawnPosition,Quaternion.identity);//������ɡ�
        timer=0;
        }
    }
}
