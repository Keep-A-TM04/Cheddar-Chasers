using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    //movement variables
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    //HUD variables
    public Text timer;
    public Text distGoal;
    public Text distEnemy;

    private float timeLeft = 60.0f;
    public GameObject Player;
    public GameObject Cheese;
    public GameObject Enemy;
    public float distToGoal;
    public float distToEnemy;

    //pause button variables
    public static bool IsPaused = false;
    public GameObject pauseMenuUI;

    //audio variables
    private AudioSource source;
    //public AudioClip panic;
    public AudioClip munch;
    private int sound;
    private bool alreadyPlayed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        //reset player spawn
        //gameObject.transform.position = new Vector3(0, 1, 0);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        //flying is disabled on this server
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        //keep it to yourself
        controller.Move(moveDirection * Time.deltaTime);

        distToGoal = Vector3.Distance(Player.transform.position, Cheese.transform.position);
        distToEnemy = Vector3.Distance(Player.transform.position, Enemy.transform.position);
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Game Over");
        }

        //pause toggle
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown(buttonName: "Pause"))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        //print text
        SetAllText();
    }

    //smack it with your face
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Game Over");
        }

        if (other.gameObject.CompareTag("PickUp")){
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Success");
            sound = 1;
            Sounds();
        }
    }

    //pause functions stuff
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        //Cursor.lockState = CursorLockMode.None;
    }


    //for the world to see
    void SetAllText()
    {
        timer.text = "Time until cheese spoils: " + timeLeft.ToString();
        distGoal.text = "Distance to cheese: " + distToGoal.ToString();
        distEnemy.text = "Distance to cat: " + distToEnemy.ToString();
    }

    //sound function
    void Sounds()
    {
        if(sound == 1 && alreadyPlayed == false)
        {
            source.PlayOneShot(munch);
            alreadyPlayed = true;
        }
    }
}