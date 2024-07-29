using System.Security.Claims;

namespace Attendiia.Services.Interface;

public interface IClaimService
{
    List<Claim> Claims { get; }

    void AddClaim(Claim claim);
    void DeleteClaim(Claim claim);
    bool IsExsitsClaim(Claim claim);
    string GetClaimValue(Claim claim);
    void UpdateClaim(Claim claim);
}
