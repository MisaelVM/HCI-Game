using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour
{
    private Animator animator;
    private static readonly int GotHit = Animator.StringToHash("GotHit");
    private static readonly int HitSourceRight = Animator.StringToHash("HitSourceRight");


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        var damager = other.gameObject.GetComponent<Damager>();
        if (damager == null)
            return;
        animator.SetFloat(HitSourceRight, FromRight(other.ClosestPoint(transform.position)));
        animator.SetTrigger(GotHit);
    }

    private void OnTriggerExit(Collider other) {
        animator.ResetTrigger(GotHit);
    }

    private float FromRight(Vector3 other) {
        var right = transform.right;
        var dot = Vector3.Dot(right.normalized, other.normalized);
        dot = (dot / 2.0f) + 0.5f; // Clamp
        Debug.Log($"from right: {dot}");
        return dot;
    }
}
