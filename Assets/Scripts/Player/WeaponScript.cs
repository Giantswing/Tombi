using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
    public PlayerMovement master;
    string otherName;
    private MYSTATS otherStats;
    private Vector3 myColliderPosition;
    private float myColliderSize = 0.5f;
    private Vector3 hitBoxSize;
    private Collider[] enemyHits;
    private int ignoreLayer = 11;

    private void Start()
    {
        otherStats = null;
        Physics.IgnoreLayerCollision(0, ignoreLayer);
        //hitBoxSize = new Vector3(f, 1f, 1f);
    }

    private void FixedUpdate()
    {
        myColliderPosition = transform.position;

        if (master.canHit)
        {
            enemyHits = Physics.OverlapSphere(myColliderPosition, myColliderSize);
            for (int i = 0; i < enemyHits.Length; i++)
            {
                if (enemyHits[i].CompareTag("Enemy"))
                {
                    
                    otherStats = enemyHits[i].GetComponent<MYSTATS>();
                    if(otherStats != null)
                    {
                        if (otherStats.invincibilityTime <= 0)
                        {
                            otherStats.invincibilityTime = otherStats.invincibilityTimeOriginal;
                            otherStats.HP -= (int)master.attacks[master.attackIndex - 1].AttackDamange;
                        }
                    }
                    
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireSphere(myColliderPosition, myColliderSize);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        print("He tocado algo: " + other.name);
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
    */
}
