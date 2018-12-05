using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYSTATS : MonoBehaviour {
    public int HP;
    public int maxHP;
    public float invincibilityTime = 0;
    [HideInInspector] public float invincibilityTimeOriginal;

    private void Start()
    {
        invincibilityTimeOriginal = 0.35f;
        invincibilityTime = 0;
        HP = maxHP;
    }
}
