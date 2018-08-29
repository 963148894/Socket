using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    Client c;
	// Use this for initialization
	void Start () {
        c = new Client();

        c.StarConnect();
        c.Send("aaaa");
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Client cv = new Client();//注意：局部变量，过一段时间大概2s会被gc释放掉，socket也会被销毁，
                                     //服务器Socket.receive将不再阻塞，返回长度为0,表示该socket已关闭
            cv.StarConnect();
            cv.Send("aaaa");
        }
       
    }
}
