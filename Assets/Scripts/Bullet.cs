using System;
using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  [RequireComponent(typeof(Rigidbody))]
  public class Bullet : MonoBehaviour
  {
    public GameObject ExplosionPrefab;
    public float Force = 3f;

    private void Start()
    {
      var rigidbody = GetComponent<Rigidbody>();
      
      rigidbody.AddForce(transform.forward * Force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        return;
      }
      
      Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
      
      Destroy(gameObject);
    }
  }
}
