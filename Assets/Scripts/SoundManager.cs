using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioClip[] enemyHitSounds;
    [SerializeField] private AudioClip[] enemyDieSounds;
    [SerializeField] private AudioClip[] enemyAttackSounds;
    [SerializeField] private AudioClip[] enemyFootstepSounds;

    [SerializeField] private AudioClip[] playerFootstepSounds;

    public void PlayEnemyHit(AudioSource _source) => PlaySound(GetRandomSound(enemyHitSounds), _source);
    public void PlayEnemyDie(AudioSource _source) => PlaySound(GetRandomSound(enemyDieSounds), _source);
    public void PlayEnemyFootstep(AudioSource _source) => PlaySound(GetRandomSound(enemyFootstepSounds), _source);
    public void PlayEnemyAttack(AudioSource _source) => PlaySound(GetRandomSound(enemyAttackSounds), _source);

    public void PlayPlayerFootstep(AudioSource _source) => PlaySound(GetRandomSound(playerFootstepSounds), _source);



    private AudioClip GetRandomSound(AudioClip[] _clips) => _clips[Random.Range(0, _clips.Length)];

    private void PlaySound(AudioClip _clip, AudioSource _source)
    {
        if (_clip == null || _source == null)
            return;

        _source.clip = _clip;
        _source.pitch = Random.Range(0.8f, 1.2f);
        _source.Play();
    }
}
