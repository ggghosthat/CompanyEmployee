using Entities.DTO;

using System;
using System.Threading.Tasks;
namespace Contracts.Interfaces;
public interface IAuthenticationManager
{
    public Task<bool> ValidateUser(UserAuthenticationDto userAuthDto);
    public Task<string> CreateToken();
}
