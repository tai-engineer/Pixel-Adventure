using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    protected abstract void Move(Vector2 nextMovement);
}
