using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public Node currentNode;
    public List<Node> path = new List<Node>();
    public Node end;

    private void Update()
    {
        CreatePath();
    }

    private Node FindNearestNodeToBird()
    {
        // Tìm object với tag "Bird"
        GameObject bird = GameObject.FindGameObjectWithTag("Player");
        if (bird == null)
        {
            Debug.LogWarning("Không tìm thấy object với tag 'Bird'");
            return null;
        }

        // Lấy tất cả nodes trong scene
        Node[] nodes = FindObjectsOfType<Node>();
        if (nodes.Length == 0)
        {
            Debug.LogWarning("Không tìm thấy node nào trong scene");
            return null;
        }

        // Tìm node gần nhất
        Node nearestNode = null;
        float minDistance = float.MaxValue;

        foreach (Node node in nodes)
        {
            float distance = Vector2.Distance(node.transform.position, bird.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    public void CreatePath()
    {
        if (path.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2f),
                3f * Time.deltaTime);
            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
        else
        {
            end = FindNearestNodeToBird();

            if (end != null && currentNode != null)
            {
                path = AStarManager.instance.GeneratePath(currentNode, end);
                if (path == null || path.Count == 0)
                {
                    Debug.LogWarning("Không thể tạo path đến Bird");
                }
            }
            else
            {
                Debug.LogWarning("CurrentNode hoặc End node là null");
            }
        }
    }
}