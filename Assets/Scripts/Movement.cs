using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem leftwingParticle;
    [SerializeField] ParticleSystem rightwingParticle;
    [SerializeField] ParticleSystem thrustersParticle;

    Rigidbody rigidBody;
    AudioSource audioSource;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotate();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
    {
        thrustersParticle.Play();

        rigidBody.AddRelativeForce(mainThrust * Time.fixedDeltaTime * Vector3.up);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void StopThrusting()
    {
        thrustersParticle.Stop();
        audioSource.Stop();
    }

    void ProcessRotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        leftwingParticle.Play();
        ApplyRotation(rotationSpeed);
    }

    private void RotateRight()
    {
        rightwingParticle.Play();
        ApplyRotation(-rotationSpeed);
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(rotationThisFrame * Time.fixedDeltaTime * Vector3.forward);
        rigidBody.freezeRotation = false;
    }
}

