using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public static Magnet mgnt = null;

    void Awake()
    {
        if (mgnt == null)
        {
            mgnt = this;
            DontDestroyOnLoad(this);
        }
        else if (this != mgnt)
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] float magnetForce;
    List<Rigidbody> affectedRigidbodies=new List<Rigidbody>();
    Transform magnet;

    void Start()
    {
        magnetForce = 1000;
        magnet = transform;
        affectedRigidbodies.Clear();
    }

    private void FixedUpdate()
    {
        if (transform.GetComponent<MoveHole>().isControllerActive)
        {
            foreach(Rigidbody rb in affectedRigidbodies)
            {
                rb.AddForce((magnet.position - rb.position) * magnetForce * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obtainable")
        {
            ApplyMagnetForce(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obtainable")
        {
            ReleaseMagnetForce(other.attachedRigidbody);
        }
    }

    private void ApplyMagnetForce(Rigidbody rb) {

        affectedRigidbodies.Add(rb);
    }

    public void ReleaseMagnetForce(Rigidbody rb)
    {
        affectedRigidbodies.Remove(rb);
    }
}
