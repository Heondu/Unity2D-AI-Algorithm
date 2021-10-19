using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    private List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int startingCount = 250; //��ü ��
    const float AgentDensity = 0.08f; //��ü ���� �� �е�

    [Range(1f, 100f)]
    public float driveFactor = 10f; //�̵� �ӵ�
    [Range(1f, 100f)]
    public float maxSpeed = 5f; //�ִ� �̵� �ӵ�
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f; //�ֺ� �ݰ� ������
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f; // ȸ�� �ݰ� �¼�

    private float squareMaxSpeed; //�ִ� �̵� �ӵ� ������
    private float squareNeighborRadius; //�ֺ� �ݰ� ������ ������
    private float squareAvoidanceRadius; //ȸ�� �ݰ� ������ ������
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //��ü ���� �� �ʱ�ȭ
        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity, //������ 1�� �ݿ� �ȿ��� �������� ��ǥ�� ���ϰ� ��ü ���� �е��� ���� ��ǥ�� ����Ѵ�.
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f) //�������� ������ ���Ѵ�.
                ), transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent); //��ü ������ ���� ����Ʈ�� �߰�
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent); //��ó ��ü�� �޾ƿ´�.

            Vector2 move = behavior.CalculateMove(agent, context, this); //���� �ൿ(Avoidance, Alignment, Cohesion)�� �̵� ������ ����Ѵ�.
            move *= driveFactor;
            //���� ũ�Ⱑ �ִ� �̵� �ӵ��� ���� ��� �ִ� �̵� �ӵ��� ����
            if (move.SqrMagnitude() > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            //��ü �̵�
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            //�ֺ� ��ü���� �ڽ��� ����
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
