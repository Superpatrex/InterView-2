using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class Chair : MonoBehaviour
{
    public Transform sitPosition; // Assign a child GameObject to indicate where the player should sit and face
    public float lockTime; // Time in seconds before the player can stand up again
    public float cooldownDuration;
    public InputActionReference moveInputAction;

    private bool isSeated = false;
    private bool isCooldown = false;
    private Vector2 lastMoveInput = Vector2.zero;

    private void OnMovePerformed(InputAction.CallbackContext context) {
        Debug.Log("hey man");
        Debug.Log(context.ReadValue<Vector2>());
        lastMoveInput = context.ReadValue<Vector2>();
    }

    // Call this method to stop listening to the move input action when the player stands up
    private void OnPlayerStoodUp() {
        isSeated = false;
        moveInputAction.action.performed -= OnMovePerformed;
    }

    // when the player sits down
    private void OnTriggerEnter(Collider other)
    {   
        Debug.Log("trying to sit!");
        Debug.Log(isSeated.ToString()+" "+isCooldown);
        if(isSeated || isCooldown) return;
        lastMoveInput = Vector2.zero;
        isSeated = true;
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SitDown(other.transform));
        }
        moveInputAction.action.performed += OnMovePerformed;
    }

    private IEnumerator SitDown(Transform playerTransform)
    {
        // Disable player movement
        moveInputAction.action.Disable();

        // Set player position and rotation to match the sitPosition
        playerTransform.position = sitPosition.position;
        playerTransform.rotation = sitPosition.rotation;

        // Wait for lockTime to pass
        yield return new WaitForSeconds(lockTime);
        while (isSeated) {
            Debug.Log(lastMoveInput.magnitude);
            if (lastMoveInput.magnitude > 0.2f) { // Threshold, adjust as needed
                TeleportPlayer(playerTransform);
                // Break the loop and end the coroutine
                StartCoroutine(SittingCooldown());
                Debug.Log("After cooldown call!");
                OnPlayerStoodUp();
                break;
            }
            yield return null; // Wait until next frame and check again
        }
        // Enable player movement again
        moveInputAction.action.Enable();
    }
    private void TeleportPlayer(Transform playerTransform)
    {
        Vector3 direction = new Vector3(lastMoveInput.x, 0, lastMoveInput.y);
        Debug.Log(direction);
        direction = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0) * direction;
        direction.Normalize();
        playerTransform.position += direction * 0.5f;
    }
    private IEnumerator SittingCooldown()
    {
        Debug.Log("entering cooldown");
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
        Debug.Log("exiting cooldown");
    }
}
