﻿using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class EnemyBehaviour : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float lifePoints = 10;

    private Rigidbody2D _Rb;
    private bool _CanMove = true;
    private string _EnemyName;

    //GameObject[] organ = new GameObject[6];

    bool coroutineStarted = false;

    AudioSource audioSource;
    public AudioClip spawn;
    public AudioClip death;
    public AudioClip hurt;
    // public AudioClip attack;
    public AudioClip civilianScream;
    public float volume;

    SpriteRenderer spriteRenderer;

    Animator animator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void OnBecameVisible()
    {
        audioSource.PlayOneShot(spawn, volume);
    }

    private void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
        _EnemyName = gameObject.name;

        //for (int i = 0; i < organ.Length; i++)
        //{
        //    organ[i] = transform.GetChild(i).gameObject;
        //    //organ[i].SetActive(false);
        //}
    }

    private void FixedUpdate()
    {
        if (_CanMove)
        {
            _Rb.velocity = new Vector2(-movementSpeed, 0);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            var _BulletDmg = Player.instance.currentWeapon.GetComponent<Weapon>().damage;
            lifePoints -= _BulletDmg;
            audioSource.PlayOneShot(hurt, volume);
            StartCoroutine(Flickering());
            

            if (lifePoints <= 0 && coroutineStarted == false)
            {
                lifePoints = 0;
                coroutineStarted = true;
                _CanMove = false;
                DropOrgans(_EnemyName);
            }        
           
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.tag == "Bullet")
        {
            var _BulletDmg = Player.instance.currentWeapon.GetComponent<Weapon>().damage;

            lifePoints -= _BulletDmg;
            audioSource.PlayOneShot(hurt, volume);

            if (lifePoints <= 0 && coroutineStarted == false)
            {
                lifePoints = 0;
                coroutineStarted = true;
                _CanMove = false;
                DropOrgans(_EnemyName);
            }
            
        }

        if(collision.tag == "Civilian")
        {
            StartCoroutine(EatingCivilian());
            audioSource.PlayOneShot(civilianScream, volume);
        }
    }


    private void OnDestroy()
    {
        if (SpawnManager.instance.LastEnemiesStanding > 0)
            SpawnManager.instance.LastEnemiesStanding--;
    }

    private void DropOrgans(string enemyName)
    {
        if (enemyName.Contains("Flying"))
        {
            if (_EnemyName.Contains("V2"))
            {
                //drop 4
                StartCoroutine(SpawnOrgan(4));
            }
            else if (_EnemyName.Contains("V3"))
            {
                //drop 5
                StartCoroutine(SpawnOrgan(5));
            }
            else
            {
                //drop 3
                StartCoroutine(SpawnOrgan(3));
            }
        }
        else if (enemyName.Contains("Heavy"))
        {

            if (_EnemyName.Contains("V2"))
            {
                //drop 5
                StartCoroutine(SpawnOrgan(5));
            }
            else if (_EnemyName.Contains("V3"))
            {
                //drop 6
                StartCoroutine(SpawnOrgan(6));
            }
            else
            {
                //drop 4
                StartCoroutine(SpawnOrgan(4));
            }
        }
        else if (enemyName.Contains("Light"))
        {

            if (_EnemyName.Contains("V2"))
            {
                //drop 3
                StartCoroutine(SpawnOrgan(3));
            }
            else if (_EnemyName.Contains("V3"))
            {
                //drop 4
                StartCoroutine(SpawnOrgan(4));
            }
            else
            {
                //drop 2
                StartCoroutine(SpawnOrgan(2));
            }
        }
        else if (enemyName.Contains("Boss"))
        {

            if (_EnemyName.Contains("V2"))
            {
                //drop 25
                StartCoroutine(SpawnOrgan(25));
            }
            else if (_EnemyName.Contains("V3"))
            {
                //drop 30
                StartCoroutine(SpawnOrgan(30));
            }
            else
            {
                //drop 20
                StartCoroutine(SpawnOrgan(20));
            }
        }
    }

    IEnumerator SpawnOrgan(int maxDrop)
    {
        for (int i = 0; i < maxDrop; i++)
        {
            int random = Random.Range(0, 100);
            int randomforceX = Random.Range(-500, 500);
            int randomforceY = Random.Range(300, 500);

            if (random >= 0 && random < 5)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Heart") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            else if(random >= 5 && random < 15)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Brain") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            else if(random >= 15 && random < 30)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Lungs") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            else if(random >= 30 && random < 50)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Kidney") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            else if(random >= 50 && random < 75)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Intestine") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            else if(random >= 75 && random < 100)
            {
                GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Liver") as GameObject, transform.position, Quaternion.identity);
                SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
            }
            yield return null;
        }
        animator.SetBool("imDeath", true);
        audioSource.PlayOneShot(death, volume);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator EatingCivilian()
    {
        CivilianManager.instance.CiviliansReamins--;

        if(CivilianManager.instance.CiviliansReamins <= 0)
        {
            GameManager.GameOver();
        }
        
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private IEnumerator Flickering()
    {
        
        Color noColor;
        Color yesColor;
        noColor = spriteRenderer.color;
        yesColor = spriteRenderer.color;
        noColor.a = 0f;
        yesColor.a = 1f;


        for (int i = 0; i < 10; i++)
        {
            int randomforceX = Random.Range(-300, 300);
            int randomforceY = Random.Range(100, 300);
            GameObject SpawnedOrgan = Instantiate(Resources.Load("Organs/Blood") as GameObject, transform.position, Quaternion.identity);
            SpawnedOrgan.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomforceX, randomforceY));
        }


        if (lifePoints <= 0)
        {
            spriteRenderer.color = yesColor;
            yield break;
        }
        spriteRenderer.color = noColor;
        yield return new WaitForSeconds(0.1f);

        if (lifePoints <= 0)
        {
            spriteRenderer.color = yesColor;
            yield break;
        }
        spriteRenderer.color = yesColor;
        
        yield return new WaitForSeconds(0.1f);

        if (lifePoints <= 0)
        {
            spriteRenderer.color = yesColor;
            yield break;
        }

        spriteRenderer.color = noColor;
        yield return new WaitForSeconds(0.1f);

        if (lifePoints <= 0)
        {
            spriteRenderer.color = yesColor;
            yield break;
        }

        spriteRenderer.color = yesColor;

    }
}

