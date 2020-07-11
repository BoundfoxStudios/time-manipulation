using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  public class Explosion : MonoBehaviour
  {
    public float Radius = 3;
    public float Force = 750;
    public float UpwardsModifier = 3;

    private void Start()
    {
      Debug.Log("Trigger explosion...");
      
      var hits = Physics.OverlapSphere(transform.position, Radius);
      
      Debug.Log($"Found {hits.Length} colliders in range.");

      foreach (var hit in hits)
      {
        var body = hit.GetComponent<Rigidbody>();

        if (body)
        {
          body.AddExplosionForce(Force, transform.position, Radius, UpwardsModifier);
        }
      }
      
      Destroy(gameObject, 1);
    }
  }
}
