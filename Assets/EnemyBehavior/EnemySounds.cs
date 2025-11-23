using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _patrolClips;
    [SerializeField] private AudioClip[] _chaseClips;
    [SerializeField] private AudioClip[] _deadClips;
    [SerializeField] private float _patrolVolume;
    [SerializeField] private float _chaseVolume;
    [SerializeField] private float _deathVolume;

    public void PlayPatrolAudio()
    {
        int rand = Random.Range(0,_patrolClips.Length);
        _audioSource.clip = _patrolClips[rand];
        _audioSource.volume = _patrolVolume;
        _audioSource.Play();
    }


    public void PlayChaseAudio()
    {
        int rand = Random.Range(0,_chaseClips.Length);
        _audioSource.clip = _chaseClips[rand];
        _audioSource.volume = _chaseVolume;
        _audioSource.Play();
    }


    public void PlayDeadAudio()
    {
        int rand = Random.Range(0,_deadClips.Length);
        _audioSource.clip = _deadClips[rand];
        _audioSource.volume = _deathVolume;
        _audioSource.Play();
    }
}
