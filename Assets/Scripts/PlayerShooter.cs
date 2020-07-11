using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  public class PlayerShooter : MonoBehaviour
  {
    public GameObject ExplosionPrefab;
    public Camera Camera;

    private void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        Shoot();
      }
    }

    private void Shoot()
    {
      if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out var hit))
      {
        Instantiate(ExplosionPrefab, hit.point, Quaternion.LookRotation(hit.normal));
      }
    }
  }
}
