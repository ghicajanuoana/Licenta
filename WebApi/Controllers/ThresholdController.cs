using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Enums;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThresholdController : ControllerBase
    {
        private readonly IThresholdService _thresholdService;
        private readonly ILogger<ThresholdController> _logger;

        CommonStrings common = new CommonStrings();

        public ThresholdController(IThresholdService thresholdService, ILogger<ThresholdController> logger)
        {
            this._thresholdService = thresholdService;
            _logger = logger;
        }

        [HttpPost]
        [Route("addThreshold")]
        public async Task<IActionResult> AddThresholdAsync([FromBody] ThresholdDto thresholdDto)
        {
            if (thresholdDto == null)
            {
                _logger.LogError(common.NullThreshold);
                return BadRequest(common.NullThreshold);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(thresholdDto, Formatting.Indented);
                _logger.LogInformation($"AddThreshold for {serializedContent}");
                var thresholdAdded = await _thresholdService.AddThresholdAsync(thresholdDto);

                if (thresholdAdded)
                {
                    return Ok(thresholdDto);
                }
                _logger.LogError(common.UniqueNameCombination);
                return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.UniqueNameCombination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("getAllThresholds")]
        public async Task<IActionResult> GetAllThresholdsAsync()
        {
            try
            {
                _logger.LogInformation("GetAllThresholdsAsync");
                var thresholdDtos = await _thresholdService.GetAllThresholdsAsync();

                return Ok(thresholdDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{thresholdId}")]
        public async Task<IActionResult> DeleteThresholdByIdAsync(int thresholdId)
        {
            if (thresholdId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"DeleteThresholdById by threshold id: {thresholdId}");
                await _thresholdService.DeleteThresholdByIdAsync(thresholdId);

                return Ok(common.RemovedThreshold);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateThreshold")]
        public async Task<IActionResult> UpdateThresholdAsync([FromBody] ThresholdDto thresholdDto)
        {
            if (thresholdDto == null)
            {
                _logger.LogError(common.NullThreshold);
                return BadRequest(common.NullThreshold);
            }

            try
            {
                var serializedContent = JsonConvert.SerializeObject(thresholdDto, Formatting.Indented);
                _logger.LogInformation($"UpdateThreshold for {serializedContent}");
                var updateResponse = await _thresholdService.UpdateThresholdAsync(thresholdDto);

                if (updateResponse == ValidationResult.Null)
                {
                    _logger.LogError(common.NullThreshold);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundThreshold);
                }
                else if (updateResponse == ValidationResult.InUse)
                {
                    _logger.LogError(common.UniqueNameCombination);
                    return this.StatusCode(SpecialCodes.Status700HasUniqueName, common.UniqueNameCombination);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{thresholdId}")]
        public async Task<IActionResult> GetThresholdByIdAsync(int thresholdId)
        {
            if (thresholdId == 0)
            {
                _logger.LogError(common.InvalidId);
                return BadRequest(common.InvalidId);
            }

            try
            {
                _logger.LogInformation($"GetThresholdById by threshold id: {thresholdId}");
                var threshold = await _thresholdService.GetThresholdByIdAsync(thresholdId);

                if (threshold == null)
                {
                    _logger.LogError(common.NotFoundThreshold);
                    return this.StatusCode(SpecialCodes.Status702NotFound, common.NotFoundThreshold);
                }

                return Ok(threshold);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}