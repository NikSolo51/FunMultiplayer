using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService 
    {
        Vector2 Axis { get; }
        Vector2 ViewAxis { get; }
        float ScrollAxis { get; }
        bool IsClickButtonUp();
        bool IsClickButtonDown();
        bool IsClickButtonPress();
    };
}