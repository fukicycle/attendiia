using Attendiia.Models;

namespace Attendiia.Stores;

public sealed class UserGroupContainer
{
    public List<Group> Groups { get; private set; } = new List<Group>();
}
