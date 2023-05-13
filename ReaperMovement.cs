using UnityEngine;

public class ReaperMovement : MonoBehaviour
{
    public GameObject[] followers;  // Array of follower objects
    public float lerpDuration = 1f;  // Duration of the lerping animation
    public float delayPerFollower = 1f;  // Delay per follower in the array
    public float baseMinDistance = 2.0f;  // Base minimum distance for the first follower
    public float maxSpeed = 5f;  // Maximum speed of the followers
    public float lerpSpeed = 1f;  // Speed of the lerping movement

    private Transform playerTransform;  // Reference to the player's transform
    private Vector3[] initialPositions;  // Initial positions of the followers
    private float[] delays;  // Delays for each follower
    private float[] minDistances;  // Minimum distances for each follower

    private void Start()
    {
        playerTransform = transform;  // Assign the player's transform

        // Initialize arrays
        initialPositions = new Vector3[followers.Length];
        delays = new float[followers.Length];
        minDistances = new float[followers.Length];

        // Calculate the total delay for all followers
        float totalDelay = delayPerFollower * (followers.Length - 1);

        // Calculate the minimum distance for each follower
        for (int i = 0; i < followers.Length; i++)
        {
            minDistances[i] = baseMinDistance + i;  // Increase the base minimum distance by 1.0 for each follower
        }

        // Store initial positions and calculate delays
        for (int i = 0; i < followers.Length; i++)
        {
            initialPositions[i] = followers[i].transform.position;
            delays[i] = i * delayPerFollower - totalDelay;
        }
    }

    private void Update()
    {
        // Move the player based on input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * maxSpeed * Time.deltaTime;
        playerTransform.position += movement;

        for (int i = 0; i < followers.Length; i++)
        {
            float t = Mathf.Clamp01((Time.time - delays[i]) / lerpDuration);  // Calculate the interpolation parameter

            // Calculate the distance between the follower and the player
            float distance = Vector3.Distance(followers[i].transform.position, playerTransform.position);

            // Calculate the speed based on the distance
            float speed = maxSpeed;

            Vector3 targetPosition;

            if (distance < minDistances[i])
            {
                speed = 0f;
                targetPosition = followers[i].transform.position; // Set current position as the target
            }
            else
            {
                targetPosition = playerTransform.position; // Set player's position as the target
            }

            // Lerp the follower towards the target position with the adjusted speed
            followers[i].transform.position = Vector3.Lerp(followers[i].transform.position, targetPosition, t * lerpSpeed * speed);
        }
    }
}
