using System.Drawing;
using UnityEngine;

namespace Code
{
    
    public class CameraController : MonoBehaviour
    {
        public void SetPosition(Vector2Int gridSize)
        {
            transform.position = new Vector3()
            {
                x = gridSize.x / 2f - 0.5f,
                y = gridSize.x,
                z = 0.5f,
            };
        }
    }
}