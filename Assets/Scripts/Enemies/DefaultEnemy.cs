using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : MonoBehaviour {

    public MYSTATS stats;

	// Use this for initialization
	void Start () {
        stats = GetComponent<MYSTATS>();
	}
	
	// Update is called once per frame
	void Update () {
        if (stats.invincibilityTime > 0)
            stats.invincibilityTime -= Time.deltaTime;
        //print(stats.invincibilityTime);

		if(stats.HP < 0)
        {
            Die();
        }
	}

    void Die()
    {
        Destroy(gameObject);
    }
}
