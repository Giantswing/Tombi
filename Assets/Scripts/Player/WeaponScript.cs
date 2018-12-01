using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
    public PlayerMovement master;
    string otherName;
    private MYSTATS otherStats;

    private void OnTriggerEnter(Collider other)
    {
        if (master.canHit)
        {
            print("Puedo darle");
            otherName = other.tag;
            if(otherName == "Enemy")
            {
                print("Es un enemigo");
                otherStats = other.GetComponent<MYSTATS>();
                if(otherStats != null)
                {
                    print("Tiene vida!");
                    otherStats.HP -= 10;

                    if(otherStats.HP <= 0)
                    {
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}
