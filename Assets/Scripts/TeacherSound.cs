using UnityEngine;

public class TeacherSound : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    [Header("Distance Alarm")]
    [SerializeField] private AudioSource distanceAlarmSound;
    [SerializeField] private float minAlarmDistance;
    [SerializeField] private float maxAlarmVolume = 1f;
    [SerializeField] private float alarmDistanceAmplifier = 2f;
    [SerializeField] private float baseVolume = 1f;

    private float DistanceToPlayer => Vector3.Distance(transform.position, player.position);

    private void Start()
    {
        distanceAlarmSound.loop = true;
        distanceAlarmSound.volume = baseVolume;
        if (distanceAlarmSound.isPlaying) distanceAlarmSound.Stop();
    }

    private void Update()
    {
        if (!distanceAlarmSound.isPlaying && ShouldPlayAlarm())
        {
            distanceAlarmSound.Play();
        }
        else if (distanceAlarmSound.isPlaying && !ShouldPlayAlarm())
        {
            distanceAlarmSound.Stop();
        }

        if (distanceAlarmSound.isPlaying)
        {
            distanceAlarmSound.volume = Mathf.Min(baseVolume - DistanceToPlayer * 0.01f * alarmDistanceAmplifier, maxAlarmVolume);
        }
    }

    private bool ShouldPlayAlarm()
    {
        return DistanceToPlayer <= minAlarmDistance;
    }
}
