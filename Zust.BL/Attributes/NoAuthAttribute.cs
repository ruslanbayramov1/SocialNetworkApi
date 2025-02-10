namespace Zust.BL.Attributes;

/// <summary>
/// Overrides the [Auth] attribute and allow anonymous users to access to the endpoint.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NoAuthAttribute : Attribute
{
}
