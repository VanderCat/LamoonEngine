using Lamoon.Engine;
using NekoLib.Core;

namespace FlappyBird; 

public class ObstacleGenerator : Behaviour {
    public float Timer = 0f;
    public float TimeOut = 2.5f;
    public float gapSize = 190f;
    public float offset = 180f;
    public GameObject canvas;

    void Update() {
        Timer += Time.DeltaF;
        if (Timer >= TimeOut) {
            Timer = 0;
            var obstacleUp = Obstacle.SpawnObstacle(canvas);
            var obstacleDown = Obstacle.SpawnObstacle(canvas);
            var offsetr = (float)Random.Shared.NextDouble()*offset;
            offsetr -= offset / 2;
            obstacleUp.Transform.LocalPosition = obstacleUp.Transform.LocalPosition with 
                { X = 1280, Y = -gapSize/2+offsetr };
            obstacleDown.Transform.LocalPosition = obstacleDown.Transform.LocalPosition with 
                { X = 1280, Y = 360f+gapSize/2+offsetr };
            obstacleUp.Initialize();
            obstacleDown.Initialize();
        }
    }
}