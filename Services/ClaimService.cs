using System.Security.Claims;
using Attendiia.Services.Interface;
using Newtonsoft.Json;

namespace Attendiia.Services;

public sealed class ClaimService : IClaimService
{
    public List<Claim> Claims { get; } = new List<Claim>();

    public void AddClaim(Claim claim)
    {
        if (IsExsitsClaim(claim))
        {
            throw new Exception($"{claim.Type} is already added.");
        }
        Claims.Add(claim);
    }

    public void DeleteClaim(Claim claim)
    {
        if (IsExsitsClaim(claim))
        {
            Claims.Remove(claim);
        }
    }

    public string GetClaimValue(Claim claim)
    {
        return claim.Value;
    }

    public bool IsExsitsClaim(Claim claim)
    {
        return Claims.Any(a => a.Type == claim.Type);
    }

    public void UpdateClaim(Claim claim)
    {
        if (!IsExsitsClaim(claim))
        {
            throw new Exception($"{claim.Type} is not found.");
        }
        DeleteClaim(claim);
        AddClaim(claim);
    }
}
