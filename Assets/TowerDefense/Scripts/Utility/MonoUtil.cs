using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoUtil : MonoBehaviour
{
    public UnityEvent onAwake;
    public UnityEvent onStart;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public UnityEvent onDestroy;
    public UnityEvent onTriggerEnter;
    public float minCollisionForce;
    public UnityEvent onCollisionEnter;
    public UnityEvent onMouseDown;
    public UnityEvent onBecameVisible;
    public UnityEvent onBecameInvisible;

    //events
    private void Awake()
    {
        onAwake.Invoke();
    }

    void Start()
    {
        onStart.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }

    private void OnMouseDown()
    {
        onMouseDown.Invoke();
    }

    private void OnEnable()
    {
        onEnable.Invoke();
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }

    private void OnDestroy()
    {
        onDestroy.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > minCollisionForce)
        {
            onCollisionEnter.Invoke();
        }
    }

    private void OnBecameVisible()
    {
        onBecameVisible.Invoke();
    }

    private void OnBecameInvisible()
    {
        onBecameInvisible.Invoke();
    }
    //utils
    public void Spawn(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
    public void PlayClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
