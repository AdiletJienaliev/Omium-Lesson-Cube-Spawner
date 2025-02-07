using UnityEngine;

namespace Cube
{
    public class UIHandler : MonoBehaviour
    {
        public void ActivateCubes(bool condition)
        {
            CubeGenerator.ActivateCubes(condition);
        }
        public void ActivateMove(bool condition)
        {
            CubeMover.startMoving(condition);
        }
        public void ActivateHunting(bool condition)
        {
            CubeMover.startHunting(condition);
            CubeMover.startMoving(true);
        }
        public void ActivateShooting(bool condition)
        {
            CubeMover.startShoot();
        }
    }
}