using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;

    private CinemachineConfiner2D confiner;

    [SerializeField] private Direction direction;

    [SerializeField] float additivePos = 2;

    enum Direction { Up, Down, Left, Right };

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (confiner != null)
            {
                confiner.BoundingShape2D = mapBoundary;
                confiner.InvalidateBoundingShapeCache();
                UpdatePlayerPosition(collision.gameObject);
            }
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos; // Adjust as needed
                break;
            case Direction.Down:
                newPos.y -= additivePos; // Adjust as needed
                break;
            case Direction.Left:
                newPos.x -= additivePos; // Adjust as needed
                break;
            case Direction.Right:
                newPos.x += additivePos; // Adjust as needed
                break;
        }

        player.transform.position = newPos;
    }
}
