using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to play sounds that are made from other objects but would be unable to completely
 * play them as they are destroyed during the clip's lifetime
 */

public class PlayGlobalSound : MonoBehaviour {

	public AudioSource[] sources = new AudioSource[0];
	public AudioClip[] deathClips = new AudioClip[0];
	public AudioClip[] hitClips = new AudioClip[0];

	void Start () {
		sources[0] = gameObject.AddComponent<AudioSource> (); //mutant death
		sources[1] = gameObject.AddComponent<AudioSource> (); //drone/missile death
		sources[2] = gameObject.AddComponent<AudioSource> (); //pickup
		sources[3] = gameObject.AddComponent<AudioSource> (); //new wave

		sources[4] = gameObject.AddComponent<AudioSource> (); //drone hit 1
		sources[5] = gameObject.AddComponent<AudioSource> (); //drone hit 2
		sources[6] = gameObject.AddComponent<AudioSource> (); //drone hit 3
		sources[7] = gameObject.AddComponent<AudioSource> (); //mutant hit

		deathClips [0] = Resources.Load ("Sounds/mutantdeath1") as AudioClip;
		deathClips [1] = Resources.Load ("Sounds/explosion1") as AudioClip;
		deathClips [2] = Resources.Load ("Sounds/pickup") as AudioClip;
		deathClips [3] = Resources.Load ("Sounds/nextWave") as AudioClip;

		hitClips [0] = Resources.Load ("Sounds/hitbydrone1") as AudioClip;
		hitClips [1] = Resources.Load ("Sounds/hitbydrone2") as AudioClip;
		hitClips [2] = Resources.Load ("Sounds/hitbydrone3") as AudioClip;
		hitClips [3] = Resources.Load ("Sounds/hitbymutant") as AudioClip;

		sources[0].clip = deathClips [0];
		sources[1].clip = deathClips [1];
		sources[2].clip = deathClips [2];
		sources[3].clip = deathClips [3];

		sources[4].clip = hitClips [0];
		sources[5].clip = hitClips [1];
		sources[6].clip = hitClips [2];
		sources[7].clip = hitClips [3];



}
}
