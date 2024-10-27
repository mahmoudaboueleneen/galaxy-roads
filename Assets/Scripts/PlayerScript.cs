using TMPro;
using UnityEngine;
using static Shared;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;

    [Header("Player Movement")]
    [SerializeField] private float laneSwapSpeed = 5f;
    [SerializeField] private float jumpForce = 25f;
    public Vector3 jump;
    private bool isGrounded;

    private bool disableControls;

    private Vector3 leftLanePosition;
    private Vector3 middleLanePosition;
    private Vector3 rightLanePosition;

    private Lane currentLane;

    private const int MAX_FUEL = 50;
    private const int DEFAULT_FUEL_DECREMENT_AMOUNT = 1;
    private const int BURNING_FUEL_DECREMENT_AMOUNT = 10;
    private int fuelDecrementAmount;
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

        jump = new Vector3(0.0f, 2.0f, 0.0f);
        isGrounded = true;

        disableControls = false;

        // Initialize lane positions based on the player's starting position
        middleLanePosition = tr.position;
        leftLanePosition = tr.position + Vector3.left * LANE_WIDTH;
        rightLanePosition = tr.position + Vector3.right * LANE_WIDTH;

        currentLane = Lane.Middle;

        scoreText.text = "Score: " + score;
        finalScoreText.text = "Final Score: " + score;

        fuelDecrementAmount = DEFAULT_FUEL_DECREMENT_AMOUNT;
        fuel = MAX_FUEL;
        fuelText.text = "Fuel: " + fuel;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (!disableControls && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("isGrounded: " + isGrounded);
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        HandleLaneChange();
        MoveToLanePosition();
        DecrementFuelEverySecond();
        IncreaseScoreEverySecond();
        CheckForFuelRefillCheat();
        CheckForFalling();
    }

    private void CheckForFalling()
    {
        if (tr.position.y < -1.2)
        {
            disableControls = true;
            AudioSource.PlayClipAtPoint(AudioManager.Instance.fallingSfxClip, tr.position);
        }
        if (tr.position.y < -5)
        {
            GameManager.Instance.EndGame();
            return;
        }
    }

    private void HandleLaneChange()
    {
        if (!disableControls && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            MoveLeft();
        }
        else if (!disableControls && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
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
        else if (currentLane == Lane.Left)
        {
            Debug.Log("Already in left lane");
            AudioSource.PlayClipAtPoint(AudioManager.Instance.errorSfxClip, tr.position);
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
        else if (currentLane == Lane.Right)
        {
            Debug.Log("Already in right lane");
            AudioSource.PlayClipAtPoint(AudioManager.Instance.errorSfxClip, tr.position);
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

    private void DecrementFuelEverySecond()
    {
        timeSinceLastFuelDecrement += Time.deltaTime;

        if (timeSinceLastFuelDecrement >= 1f)
        {
            fuel -= fuelDecrementAmount;
            timeSinceLastFuelDecrement = 0f;

            if (fuel <= 0)
            {
                fuel = 0;
                GameManager.Instance.EndGame();
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
        if (collision.gameObject.CompareTag("BurningTile"))
        {
            fuelDecrementAmount = BURNING_FUEL_DECREMENT_AMOUNT;
            AudioSource.PlayClipAtPoint(AudioManager.Instance.burningSfxClip, collision.transform.position);
        }

        if (collision.gameObject.CompareTag("SuppliesTile"))
        {
            RefillFuel();
            AudioSource.PlayClipAtPoint(AudioManager.Instance.suppliesSfxClip, collision.transform.position);
        }

        if (collision.gameObject.CompareTag("BoostTile"))
        {
            TileManager.Instance.BoostTileSpeed();
            AudioSource.PlayClipAtPoint(AudioManager.Instance.boostSfxClip, collision.transform.position);
        }

        if (collision.gameObject.CompareTag("StickyTile"))
        {
            TileManager.Instance.ResetTileSpeed();
            AudioSource.PlayClipAtPoint(AudioManager.Instance.stickySfxClip, collision.transform.position);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AudioSource.PlayClipAtPoint(AudioManager.Instance.obstacleHitSfxClip, collision.transform.position);
            GameManager.Instance.EndGame();
        }

        if (collision.gameObject.CompareTag("EmptyTile"))
        {
            // Stop Tile Movement
            TileManager.Instance.StopTiles();
            // Remove any forces acting on the player and make him fall instantly
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
            rb.velocity.Normalize();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // TODO: Fix this stupid bug...... For some reason this func is triggered even when the player is mid-air, some tile is colliding with it midair
        // This is a temporary fix
        // Check if the player is not mid air
        if (rb.velocity.y == 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("BurningTile"))
        {
            fuelDecrementAmount = DEFAULT_FUEL_DECREMENT_AMOUNT;
        }
    }
}
