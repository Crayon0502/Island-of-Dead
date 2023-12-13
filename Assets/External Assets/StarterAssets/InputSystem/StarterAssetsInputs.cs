using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool aim;
        public bool shoot;
        public bool reload;
        public bool interaction;
        public bool gun;
        public bool hand;
        public bool punching;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }

        public void OnInteraction(InputValue value)
        {
            InteractionInput(value.isPressed);
        }

        public void OnGun(InputValue value)
        {
            GunInput(value.isPressed);
        }

        public void OnHand(InputValue value)
        {
            HandInput(value.isPressed);
        }

        public void OnPunching(InputValue value)
        {
            PunchingInput(value.isPressed);
        }

#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        public void ReloadInput(bool newReloadState)
        {
            reload = newReloadState;
        }

        public void InteractionInput(bool newInteractionState)
        {
            interaction = newInteractionState;
        }

        public void GunInput(bool newGunState)
        {
            gun = newGunState;
        }

        public void HandInput(bool newHandState)
        {
            hand = newHandState;
        }

        public void PunchingInput(bool newPunchigState)
        {
            punching = newPunchigState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}