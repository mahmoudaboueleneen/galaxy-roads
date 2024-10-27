using TMPro;
using UnityEngine;
using static Shared;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;

    [Header("Player Movement")]
    [SerializeField] private float laneSwapSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private bool isGrounded;

    private Vector3 leftLanePosition;
    private Vector3 middleLanePosition;
    private Vector3 rightLanePosition;

    private Lane currentLane;

    private const int MAX_FUEL = 50;
    private int fuel;
    private float timeSinceLastFuelDecrement;

    private int score;
    private float timeSinceLastScoreIncrement;

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI fuelText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        isGrounded = true;

        // Initialize lane positions based on the player's starting position
        middleLanePosition = tr.position;
        leftLanePosition = tr.position + Vector3.left * LANE_WIDTH;
        rightLanePosition = tr.position + Vector3.right * LANE_WIDTH;

        currentLane = Lane.Middle;

        scoreText.text = "Score: " + score;
        finalScoreText.text = "Final Score: " + score;

        fuel = MAX_FUEL;
        fuelText.text = "Fuel: " + fuel;
    }

    void Update()
    {
        HandleLaneChange();
        HandleJump();
        MoveToLanePosition();
        DecrementFuelEverySecond();
        IncreaseScoreEverySecond();
        CheckForFuelRefillCheat();
    }

    private void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        if (currentLane == Lane.Middle)
        {
            currentLane = Lane.Left;
        }
        else if (currentLane == Lane.Right)
        {
            currentLane = Lane.Middle;
        }
    }

    private void MoveRight()
    {
        if (currentLane == Lane.Middle)
        {
            currentLane = Lane.Right;
        }
        else if (currentLane == Lane.Left)
        {
            currentLane = Lane.Middle;
        }
    }

    private void MoveToLanePosition()
    {
        // Get the target position based on the current lane
        Vector3 targetPosition = currentLane == Lane.Left ? leftLanePosition :
                                 currentLane == Lane.Right ? rightLanePosition :
                                 middleLanePosition;

        // Smoothly move the player to the target lane position
        tr.position = Vector3.MoveTowards(tr.position, targetPosition, laneSwapSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void DecrementFuelEverySecond()
    {
        timeSinceLastFuelDecrement += Time.deltaTime;

        if (timeSinceLastFuelDecrement >= 1f)
        {
            fuel -= 1;
            timeSinceLastFuelDecrement = 0f;

            if (fuel < 0)
            {
                fuel = 0;
            }

            fuelText.text = "Fuel: " + fuel;
        }
    }

    private void IncreaseScoreEverySecond()
    {
        timeSinceLastScoreIncrement += Time.deltaTime;

        if (timeSinceLastScoreIncrement >= 1f)
        {
            score += 1;
            timeSinceLastScoreIncrement = 0f;
            scoreText.text = "Score: " + score;
            finalScoreText.text = "Final Score: " + score;
        }
    }

    private void CheckForFuelRefillCheat()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RefillFuel();
        }
    }

    private void RefillFuel()
    {
        fuel = MAX_FUEL;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NormalTile")
            || collision.gameObject.CompareTag("BurningTile")
            || collision.gameObject.CompareTag("StickyTile")
            || collision.gameObject.CompareTag("SuppliesTile")
            || collision.gameObject.CompareTag("BoostTile")
            || collision.gameObject.CompareTag("EmptyTile")
            )
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("NormalTile")
            || collision.gameObject.CompareTag("BurningTile")
            || collision.gameObject.CompareTag("StickyTile")
            || collision.gameObject.CompareTag("SuppliesTile")
            || collision.gameObject.CompareTag("BoostTile")
            || collision.gameObject.CompareTag("EmptyTile")
        )
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EmptyTile"))
        {
            
        }
    }
}
