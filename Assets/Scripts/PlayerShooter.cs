using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  public class PlayerShooter : MonoBehaviour
  {
    public GameObject BulletPrefab;
    public GameObject RewindableBulletPrefab;
    public Camera Camera;

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
        Shoot(BulletPrefab);
      }
      
      if (Input.GetKeyDown(KeyCode.E))
      {
        Shoot(RewindableBulletPrefab);
      }
    }

    private void Shoot(GameObject prefab)
    {
      var cameraTransform = Camera.transform;
      
      Instantiate(prefab, cameraTransform.position, Quaternion.LookRotation(cameraTransform.forward));
    }
  }
}
