using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] testClips;

    void Start()
    {
        if (testClips.Length > 0 && testClips[0] != null)
        {
            audioSource.PlayOneShot(testClips[0]);
            Debug.Log("Playing test clip");
        }
        else
        {
            Debug.LogWarning("Test audio clip not found or array index out of bounds.");
        }
    }
}
