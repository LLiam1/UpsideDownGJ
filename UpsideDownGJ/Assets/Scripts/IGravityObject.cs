using System;
public interface IGravityObject
{
    bool UsingGravity();
    void DisableGravity();
    void EnableGravity();
    void SetGravityScale(float gravityScale);
    float GetGravityScale();
}
