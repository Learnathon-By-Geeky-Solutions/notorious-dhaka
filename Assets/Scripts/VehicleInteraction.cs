using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    private GameObject currentVehicle; // The vehicle player can enter
    private bool canEnterVehicle = false;
    public bool isInVehicle = false;


    // Reference to the player's movement script (assuming it's named 'PlayerMovement')
    private PlayerMovement playerMovement;

    void Start()
    {
        // Get the player movement script on start
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    void Update()
    {
        if (canEnterVehicle && Input.GetKeyDown(KeyCode.Return)) // Return is the Enter key
        {
            if (!isInVehicle)
            {
                EnterVehicle();
            }
            else
            {
                ExitVehicle();
            }
        }
    }

    void EnterVehicle()
    {
        // Attach player to vehicle
        player.transform.SetParent(currentVehicle.transform);

        // Set player position above the vehicle's collider
        Vector3 seatPosition = currentVehicle.transform.position + new Vector3(0, -1, 0); // Adjust height as needed
        player.transform.position = seatPosition;

        // Match rotation
        player.transform.rotation = currentVehicle.transform.rotation;

        // Disable player movement and physics
        playerMovement.enabled = false;
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true; // Disable physics while in vehicle
        }

        isInVehicle = true;
        Debug.Log("Entered the vehicle!");
    }


    void ExitVehicle()
    {
        // Detach from vehicle
        player.transform.SetParent(null);

        // Place the player beside the vehicle instead of inside it
        Vector3 exitPosition = currentVehicle.transform.position + new Vector3(3, 1, 0); // Adjust height to prevent falling
        player.transform.position = exitPosition;

        // Enable player movement and physics
        playerMovement.enabled = true;
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false; // Re-enable physics when exiting
        }

        isInVehicle = false;
        Debug.Log("Exited the vehicle!");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            currentVehicle = other.gameObject;
            canEnterVehicle = true;
            Debug.Log("Press Enter to get into the vehicle.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            if (currentVehicle == other.gameObject)
            {
                currentVehicle = null;
                canEnterVehicle = false;
            }
        }
    }
}
