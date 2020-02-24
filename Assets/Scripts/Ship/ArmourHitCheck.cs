using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ArmourHitCheck : MonoBehaviour
{
    public bool beenHit = false;

    public List<AudioClip> hitSoundList;

    private AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = transform.parent.GetComponent<AudioSource>();
        if (myAudioSource == null)
        {
            Debug.LogError($"Cannot find Audio Source for {gameObject.name}");
            this.enabled = false;
            return;
        }

        if (hitSoundList.Count <= 0)
        {
            Debug.LogError($"No hitsounds configured for {gameObject.name}");
            this.enabled = false;
            return;
        }
    }

    private void PlayRandomHitSound()
    {
        int i = UnityEngine.Random.Range(0, hitSoundList.Count);
        AudioClip audioClip = hitSoundList[i];
        myAudioSource.PlayOneShot(audioClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        ShipDamage shipDamageScript = other.GetComponent<ShipDamage>();
        if (shipDamageScript == null)
            shipDamageScript = other.transform.parent.GetComponent<ShipDamage>();

        if (shipDamageScript != null)
        {
            PlayRandomHitSound();

            if (!beenHit)
            {
                Debug.Log($"Hit on {gameObject.name}!");
                beenHit = true;
            } else
            {
                GameManager.Instance.GameOver();
            }
            Destroy(shipDamageScript.gameObject);
        }
    }
}
