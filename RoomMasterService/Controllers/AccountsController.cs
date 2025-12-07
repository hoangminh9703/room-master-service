using Microsoft.AspNetCore.Mvc;
using RoomMasterService.DTOs;
using RoomMasterService.Services;

namespace RoomMasterService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var (response, error) = await _accountService.AuthenticateAsync(request.Username, request.Password);
            if (error != null)
                return Unauthorized(new ApiResponse { Success = false, Message = error });

            return Ok(new ApiResponse<object> { Success = true, Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<object>>> Refresh([FromBody] RefreshRequest request)
    {
        try
        {
            var (response, error) = await _accountService.RefreshTokenAsync(request.RefreshToken);
            if (error != null)
                return Unauthorized(new ApiResponse { Success = false, Message = error });

            return Ok(new ApiResponse<object> { Success = true, Data = response });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
    }
}
