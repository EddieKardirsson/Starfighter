using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    //[SerializeField] float moveSpeed = 2f;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start(){
        InitWaypoints();
    }

    // Update is called once per frame
    void Update(){
        Move();
    }   

    private void InitWaypoints(){
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    private void Move(){
        if (waypointIndex <= waypoints.Count - 1)
        {
            Vector3 targetPosition = MoveToPoint();

            if (transform.position == targetPosition)
                waypointIndex++;
        }
        else
            Destroy(gameObject);
    }

    private Vector3 MoveToPoint()
    {
        var targetPosition = waypoints[waypointIndex].transform.position;
        var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            movementThisFrame);
        return targetPosition;
    }

    public void SetWaveConfig(WaveConfig waveConfig){
        this.waveConfig = waveConfig;
    }
}
