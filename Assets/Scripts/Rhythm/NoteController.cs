// Filename: NoteController.cs
using UnityEngine;

namespace IdleShopkeeping.Rhythm
{
    /// <summary>
    /// Controls the movement of a single note in the rhythm game.
    /// </summary>
    public class NoteController : MonoBehaviour
    {
        public float Speed { get; set; } = 5f;

        private void Update()
        {
            // Move the note down the screen.
            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }
    }
}
