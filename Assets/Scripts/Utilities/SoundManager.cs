using UnityEngine;
using System.Collections;

public class SoundManager : SingletonBehaviour<SoundManager>
{
	public static AudioSource PlaySound( AudioSource audioSource )
	{
		audioSource.CreatePool();

		AudioSource spawnedAudio = audioSource.Spawn<AudioSource>();
		instance.StartCoroutine( instance.PlayAudioCoroutine( spawnedAudio ) );

		return spawnedAudio;
	}

	IEnumerator PlayAudioCoroutine( AudioSource audioSource )
	{
		audioSource.Play();
		yield return new WaitForSeconds( audioSource.clip.length );
		audioSource.Stop();
		audioSource.Recycle();
	}
}
