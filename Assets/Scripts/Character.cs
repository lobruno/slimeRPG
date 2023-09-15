using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float HEALTH_MAX;
    public float AttackSpeed;
    public float health;
    public float attackValue;
    Animator animator;
    public bool isNear = false;
    public GameManager gameManager;
    public GameObject attackBall;
    public GameObject damageText;
    public Image healthBar;

    private float time;
    void Start()
    {
        animator = GetComponent<Animator>();
        time = 0.5f;
        HEALTH_MAX = gameManager.CHhealth;
        AttackSpeed = gameManager.CHAttackSpeed;
        attackValue = gameManager.CHattackValue;
        health = HEALTH_MAX;
        healthBar.fillAmount = 1f;
    }


    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0 && gameManager.enemies.Count > 0)
        {
            Attack();
            time = AttackSpeed;
        }
    }

    public void Move()
    {
        
    }

    public void Attack()
    {
        Attack ball = Instantiate(attackBall, transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Attack>();
        ball.attack = attackValue;
        ball.gameManager = gameManager;
    }

    public void Damage(float getAttack)
    {
        Text text = Instantiate(damageText, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<Text>();
        text.SetText(getAttack);
        health -= getAttack;
        healthBar.fillAmount = health/HEALTH_MAX;

        print(health / HEALTH_MAX);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        gameManager.Restart();
        healthBar.fillAmount = health / HEALTH_MAX;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
