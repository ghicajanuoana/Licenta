using DataAccessLayer.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTPController : ControllerBase
    {
        [HttpGet("get")]
        public IActionResult GenerateOTP()
        {
            OTPRepository otpRepository = new OTPRepository();
            var otp = otpRepository.CreateOTP();
            return Ok(otp);
        }

        [HttpGet("getOTP")]
        public IActionResult getOTP()
        {
            OTPRepository otpRepository = new OTPRepository();
            var otp = otpRepository.getOTP();
            return Ok(otp);
        }
    }

}
