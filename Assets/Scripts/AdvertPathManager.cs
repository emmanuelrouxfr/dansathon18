using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertPathManager : MonoBehaviour {

    public PathManager MyPathManager;
    public bool isLeft;


    private void OnTriggerEnter(Collider other)
    {
        if(isLeft)
            MyPathManager.StartLeftPath(other.gameObject);
        else
            MyPathManager.StartRightPath(other.gameObject);
    }
}
