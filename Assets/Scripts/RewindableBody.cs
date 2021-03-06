using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoundfoxStudios.TimeManipulation
{
  [RequireComponent(typeof(Rigidbody))]
  public class RewindableBody : MonoBehaviour
  {
    private readonly Lazy<TimeManager> _rewindManager = new Lazy<TimeManager>(FindObjectOfType<TimeManager>);
    private Rigidbody _rigidbody;
    private readonly IList<TimeRecord> _timeRecords = new List<TimeRecord>(); // an array based circular ring would be better
    private TimeRecord? _lastRewindedTimeRecord;
    private int _maxTimeRecordsToSave;
    private bool _isRewinding;

    private void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();

      _maxTimeRecordsToSave = (int) (1 / Time.fixedUnscaledDeltaTime * _rewindManager.Value.MaxTimeToRewindInSeconds);
    }

    private void OnEnable()
    {
      _rewindManager.Value.RegisterRewindable(this);
    }

    private void OnDisable()
    {
      _rewindManager.Value.UnregisterRewindable(this);
    }

    public void StartRewindTime()
    {
      if (_rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousDynamic)
      {
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
      }
      
      _rigidbody.isKinematic = true;
      _isRewinding = true;
    }

    public void StopRewindTime(bool applyForceAfterRewind)
    {
      _rigidbody.isKinematic = false;
      _isRewinding = false;

      if (applyForceAfterRewind && _lastRewindedTimeRecord.HasValue)
      {
        _rigidbody.velocity = _lastRewindedTimeRecord.Value.Velocity;
        _rigidbody.angularVelocity = _lastRewindedTimeRecord.Value.AngularVelocity;
      }
      
      if (_rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousSpeculative)
      {
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      }
    }

    private void Record()
    {
      while (_timeRecords.Count >= _maxTimeRecordsToSave)
      {
        _timeRecords.RemoveAt(0);
      }

      _timeRecords.Add(new TimeRecord()
      {
        Position = _rigidbody.transform.position,
        Rotation = _rigidbody.rotation,
        Velocity = _rigidbody.velocity,
        AngularVelocity = _rigidbody.angularVelocity
      });
    }

    private void FixedUpdate()
    {
      if (_isRewinding)
      {
        Rewind();
        return;
      }

      Record();
    }

    private void Rewind()
    {
      if (_timeRecords.Count <= 0)
      {
        return;
      }

      var lastIndex = _timeRecords.Count - 1;
      var timeRecord = _timeRecords[lastIndex];
      _timeRecords.RemoveAt(lastIndex);

      _rigidbody.transform.position = timeRecord.Position;
      _rigidbody.transform.rotation = timeRecord.Rotation;

      _lastRewindedTimeRecord = timeRecord;
    }
  }
}
