using UnityEngine;

public class CharacterControllerManager : MonoBehaviour
{
    public Player player;
    public Robot robot;
    public float interactionDistance = 2f;
    public CameraFollow cameraFollow;

    private bool isControllingPlayer = true;
    private bool isNearRobot = false;

    SceneController sceneController;
    public bool haveRobot = false;

    void Start()
    {
        player.enabled = true;
        robot.enabled = false;
        robot.gameObject.SetActive(false);
        sceneController = SceneController.instance;
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
        }
    }

    void Update()
    {
        haveRobot = sceneController.haveRobot;
        if (sceneController.haveRobot)
        {
            robot.gameObject.SetActive(true);
        }
        if (isControllingPlayer)
        {
            float distance = Vector2.Distance(player.transform.position, robot.transform.position);
            isNearRobot = distance <= interactionDistance;
        }
        else
        {
            isNearRobot = true;
        }

        if (isNearRobot && Input.GetKeyDown(KeyCode.E))
        {
            SwitchControl();
        }
    }

    void SwitchControl()
    {
        if (isControllingPlayer)
        {
            player.enabled = false;
            player.gameObject.SetActive(false);
            robot.enabled = true;
            robot.transform.position = player.transform.position; // Robot xuất hiện tại vị trí Player
            isControllingPlayer = false;

            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(robot.transform);
            }
        }
        else
        {
            robot.enabled = false;
            player.enabled = true;
            player.gameObject.SetActive(true);
            player.transform.position = robot.transform.position; // Player xuất hiện tại vị trí Robot
            isControllingPlayer = true;

            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(player.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (robot != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(robot.transform.position, interactionDistance);
        }
    }
}