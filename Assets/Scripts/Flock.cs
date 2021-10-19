using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    private List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 250; //개체 수
    const float AgentDensity = 0.08f; //개체 생성 시 밀도

    [Range(1f, 100f)]
    public float driveFactor = 10f; //이동 속도
    [Range(1f, 100f)]
    public float maxSpeed = 5f; //최대 이동 속도
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f; //주변 반경 반지름
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; // 회피 반경 승수

    private float squareMaxSpeed; //최대 이동 속도 제곱값
    private float squareNeighborRadius; //주변 반경 반지름 제곱값
    private float squareAvoidanceRadius; //회피 반경 반지름 제곱값
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //개체 생성 및 초기화
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity, //반지름 1의 반원 안에서 랜덤으로 좌표를 구하고 개체 수와 밀도를 곱해 좌표를 계산한다.
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f) //랜덤으로 방향을 정한다.
                ), transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent); //개체 관리를 위해 리스트에 추가
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent); //근처 개체를 받아온다.

            Vector2 move = behavior.CalculateMove(agent, context, this); //각각 행동(Avoidance, Alignment, Cohesion)의 이동 방향을 계산한다.
            move *= driveFactor;
            //벡터 크기가 최대 이동 속도를 넘을 경우 최대 이동 속도로 고정
            if (move.SqrMagnitude() > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            //개체 이동
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            //주변 개체에서 자신은 제외
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
