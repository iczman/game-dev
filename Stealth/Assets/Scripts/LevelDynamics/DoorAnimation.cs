﻿using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
	public bool requireKey;
	public AudioClip doorSwishClip;
	public AudioClip accessDeniedClip;

	private Animator anim;
	private HashIDs hash;
	private GameObject player;
	private PlayerInventory playerInventory;
	private int count;
	private AudioSource doorAudio;

	void Awake()
	{
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<PlayerInventory>();
		doorAudio = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			if (requireKey)
			{
				if (playerInventory.hasKey)
				{
					count++;
				}
				else
				{
					doorAudio.clip = accessDeniedClip;
					doorAudio.Play();
				}
			}
			else
			{
				count++;
			}
		}
		else if (other.gameObject.tag == Tags.enemy)
		{
			if (other is CapsuleCollider)
			{
				count++;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player ||
		    (other.gameObject.tag == Tags.enemy && other is CapsuleCollider))
		{
			count = Mathf.Max (0, count - 1);
		}
	}

	void Update()
	{
		anim.SetBool (hash.openBool, count > 0);
		if (anim.IsInTransition(0) && !doorAudio.isPlaying)
		{
			doorAudio.clip = doorSwishClip;
			doorAudio.Play();
		}
	}
}
