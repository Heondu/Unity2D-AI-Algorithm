using System.Collections.Generic;
using UnityEngine;

//�߻� Ŭ����, �߻� Ŭ������ ��ӹ޴� Ŭ������ �߻� �޼ҵ带 �ݵ�� �����ؾ� �Ѵ�.
//ScriptableObject�� ��ӹ޾Ƽ� �����ͷ� �����Ѵ�.
public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
