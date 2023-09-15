using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float HEALTH_MAX;

    public float speed;
    public float AttackSpeed;
    public float health;
    public float attack;
    public bool isNear = false;
    public GameManager gameManager;
    public GameObject damageText;
    public Image healthBar;

    private Character character;
    Animator animator;
    private float time;
    private int level;


    void Start()
    {
        animator = GetComponent<Animator>();
        character = gameManager.character;

        level = gameManager.Level;
        speed = gameManager.ENSpeed;
        AttackSpeed = gameManager.ENAttackSpeed;
        health = gameManager.ENhealth + 20 * level;
        attack = gameManager.ENattackValue + 5 * level;

        time = 0.5f;
        HEALTH_MAX = health;
        healthBar.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear)
        {
            time -= Time.deltaTime;

            if (time < 0)
            {
                Attack();
                time = AttackSpeed;
            }
        }
        else
        {
            Move();
        }
    }

    public void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Attack()
    {
        if (character.health > 0)
        {
            character.Damage(attack);
        }
        else { animator.SetBool("isNear", false); isNear = false; }
    }

    public void Damage(float getAttack)
    {
        Text text = Instantiate(damageText, transform.position + new Vector3(0, 1, 0), Quaternion.identity ).GetComponent<Text>();
        text.SetText(getAttack);
        health -= getAttack;
        healthBar.fillAmount = health / HEALTH_MAX;

        if (health <= 0) { gameManager.AddMoney(); Die(); }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            isNear = true;
            animator.SetBool("isNear", true);
        }
    }
}
