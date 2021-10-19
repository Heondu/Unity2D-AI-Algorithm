using System.Collections.Generic;
using UnityEngine;

//추상 클래스, 추상 클래스를 상속받는 클래스는 추상 메소드를 반드시 구현해야 한다.
//ScriptableObject를 상속받아서 데이터로 관리한다.
public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
