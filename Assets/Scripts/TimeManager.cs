using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  public class TimeManager : MonoBehaviour
  {
    public int MaxTimeToRewindInSeconds = 10;
    public bool ApplyForceAfterRewind = true;
    public float BulletTimeStrength = 8;

    private readonly IList<RewindableBody> _rewindableBodies = new List<RewindableBody>();
    private bool _isInBulletTime;

    public void RegisterRewindable(RewindableBody body)
    {
      _rewindableBodies.Add(body);
    }

    public void UnregisterRewindable(RewindableBody body)
    {
      _rewindableBodies.Remove(body);
    }

    private void StartRewindTime()
    {
      Debug.Log("Start Rewind Time");

      foreach (var rewindableBody in _rewindableBodies)
      {
        rewindableBody.StartRewindTime();
      }
    }

    private void StopRewindTime()
    {
      Debug.Log("Stop rewind time");

      foreach (var rewindableBody in _rewindableBodies)
      {
        rewindableBody.StopRewindTime(ApplyForceAfterRewind);
      }
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.R))
      {
        StartRewindTime();
      }

      if (Input.GetKeyUp(KeyCode.R))
      {
        StopRewindTime();
      }

      if (Input.GetKeyDown(KeyCode.F))
      {
        StartBulletTime();
      }
    }

    private IEnumerator BulletTime()
    {
      Debug.Log("Starting Bullet Time...");

      _isInBulletTime = true;
      
      var slowMotionSpeed = 1 / BulletTimeStrength;
      var originalDeltaTime = Time.fixedDeltaTime;
      var originalTimeScale = Time.timeScale;

      Time.timeScale = slowMotionSpeed; 
      Time.fixedDeltaTime = slowMotionSpeed * originalDeltaTime;
      // ODER Interpolate

      yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.F));

      Time.timeScale = originalTimeScale;
      Time.fixedDeltaTime = originalDeltaTime;

      _isInBulletTime = false;

      Debug.Log("Bullet Time ended.");
    }

    private void StartBulletTime()
    {
      if (!_isInBulletTime)
      {
        StartCoroutine(BulletTime());
      }
    }
  }
}
