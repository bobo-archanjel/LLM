using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth {  get; private set; }
    private Animator anim;
    private Boolean dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;



    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
         
        if(currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            //iframe
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");

                //Player
                if (GetComponent<PlayerMovement>() != null )
                    GetComponent<PlayerMovement>().enabled = false;

                //Enemy
                if (GetComponentInParent<EnemyPatrol>() != null )
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleeEnemy>() != null )
                    GetComponent<MeleeEnemy>().enabled = false;

                dead = true;
            }
        }
            
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes) * 2);
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes) * 2);
        }

        Physics2D.IgnoreLayerCollision(7, 8, false);

    }
}
