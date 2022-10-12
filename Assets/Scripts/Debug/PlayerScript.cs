using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public HealthBarScript healthBar;

    [SerializeField] private Image damageImage = null;
    [SerializeField] private float damageTimer = 0.1f;

    [SerializeField] private AudioClip damageSound = null;
    private AudioSource damageAudioSource = null;

    public CharacterController characterController;
    public float speed = 12.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    bool isAttacking = false;
    [SerializeField] private GameObject sword;

    // Start is called before the first frame update
    void Start()
    {
        damageAudioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Utils
        if (Input.GetKeyDown(KeyCode.LeftControl))
            TakeDamage(20);
        if (Input.GetKeyDown(KeyCode.F))
            TakeDamage(5);

        // Movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2.0f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(speed * Time.deltaTime * move);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        // Controls input
        if (Input.GetMouseButtonDown(0) && !isAttacking) {
            Debug.Log("Pressed primary button.");
            StartCoroutine(SwordSlash());
        }

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }

    private void OnTriggerEnter(Collider other) {
        var damager = other.gameObject.GetComponent<Damager>();
        if (damager == null || damager.damages == Entities.ENEMY)
            return;
        TakeDamage(damager.damage);
    }

    IEnumerator SwordSlash() {
        isAttacking = true;
        const float animDuration = 0.35f;
        float elapsedTime = 0.0f;
        Quaternion from = sword.transform.localRotation;
        Quaternion to = Quaternion.Euler(45.0f, 0.0f, 75.0f) * from;
        Vector3 fromPos = sword.transform.localPosition;
        Vector3 toPos = fromPos + new Vector3(-1.97f, 0.0f, 0.26f);
        //Vector3 toPos = fromPos + new Vector3(-0.97f, 0.0f, 0.5f);
        while (elapsedTime < animDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animDuration;
            sword.transform.localRotation = Quaternion.Slerp(from, to, t);
            sword.transform.localPosition = Vector3.Slerp(fromPos, toPos, t);
            yield return null;
        }
        sword.transform.localRotation = from;
        sword.transform.localPosition = fromPos;
        isAttacking = false;
    }

    IEnumerator HurtFlash() {
        //damageImage.enabled = true;
        damageAudioSource.PlayOneShot(damageSound);

        Color damageColor = damageImage.color;
        const float fadeDuration = 0.1f;
        float elapsedTime = 0.0f;
        // Fade in
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            damageColor.a = t;
            damageImage.color = damageColor;
            yield return null;
        }
        damageColor.a = 1.0f;
        damageImage.color = damageColor;

        yield return new WaitForSeconds(damageTimer);

        // Fade out
        elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            damageColor.a = 1 - t;
            damageImage.color = damageColor;
            yield return null;
        }
        damageColor.a = 0.0f;
        damageImage.color = damageColor;

        //damageImage.enabled = false;
    }

    void TakeDamage(int damage) {
        StartCoroutine(HurtFlash());
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            StopAllCoroutines();
            currentHealth = 0;
            healthBar.SetHealth(0);
            Color damageColor = damageImage.color;
            damageColor.a = 1.0f;
            damageImage.color = damageColor;
        }
    }
}
