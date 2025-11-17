using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int artifactsCollected = 0;
    private int totalArtifacts;
    public TextMeshProUGUI artifactText;

    public AudioClip artifactSound;
    private AudioSource audioSource;

    void Start()
    {
        // Count total coins in the scene at the start
        totalArtifacts = GameObject.FindGameObjectsWithTag("Artifact").Length;

        // Get AudioSource on the same object
        audioSource = GetComponent<AudioSource>();

        UpdateArtifactText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Artifact"))
        {
            artifactsCollected++;
            Destroy(other.gameObject);

            // Play coin sound if available
            if (artifactSound != null && audioSource != null)
                audioSource.PlayOneShot(artifactSound);

            UpdateArtifactText();
        }
    }

    void UpdateArtifactText()
    {
        float percent = (float)artifactsCollected / totalArtifacts * 100f;
        artifactText.text = $"Artifacts: {artifactsCollected}/{totalArtifacts} ({percent:F1}%)";
    }
}
