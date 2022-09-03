using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float lvlLoadDelay = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip lvlEnd;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem crashParticle;

    AudioSource audioSource;
    Movement movement;

    bool isTransitioning = true;
    bool collisionTriggering = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isTransitioning || collisionTriggering) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This is our landing zone");
                break;
            case "Finish":
                LevelChangeSeqence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    } 

    void LevelChangeSeqence()
    {
        isTransitioning = false;
        audioSource.Stop();
        movement.enabled = false;
        audioSource.PlayOneShot(lvlEnd);
        successParticle.Play();
        Invoke("LoadNextLevel", lvlLoadDelay);
    }

    void StartCrashSequence()
    {
        SetMeshComponentVisibility(false);
        isTransitioning = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        movement.enabled = false;
        crashParticle.Play();
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void RespondToDebugKeys()
    {
        if (!Debug.isDebugBuild) { return; }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        } 
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionTriggering = !collisionTriggering;
        }
    }
    void SetMeshComponentVisibility(bool isVisible)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }
}
