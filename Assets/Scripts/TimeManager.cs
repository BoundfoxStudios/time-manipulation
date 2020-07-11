using System.Collections.Generic;
using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  public class TimeManager : MonoBehaviour
  {
    public int MaxTimeToRewindInSeconds = 10;
    public bool ApplyForceAfterRewind = true;
    
    private readonly IList<RewindableBody> _rewindableBodies = new List<RewindableBody>();

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
    }
  }
}
