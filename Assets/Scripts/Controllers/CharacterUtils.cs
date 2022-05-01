using UnityEngine;

namespace NeonRose.Controllers
{
    public static class CharacterUtils
    {
        public static CharacterState GetDefaultState(bool isGrounded)
        {
            return isGrounded ? CharacterState.GROUNDED : CharacterState.AIRBORNE;
        }
    }
}