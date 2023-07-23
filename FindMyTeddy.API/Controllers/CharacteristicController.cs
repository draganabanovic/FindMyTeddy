using CryptoTrading.API.Models;
using FindMyTeddy.API.Models;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.PortableExecutable;

namespace FindMyTeddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CharacteristicController : ControllerBase
    {
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicController(ICharacteristicService characteristicService)
        {
            _characteristicService = characteristicService;
     
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _characteristicService.GetAllAsync();

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(result.DataList);
        }


        [AllowAnonymous]
        [HttpGet("{characteristicId:Guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid characteristicId)
        {
            var result = await _characteristicService.GetByIdAsync(characteristicId);
            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(result.Data);
        }

        [HttpGet("pet/{petId:Guid}")]
        public async Task<ActionResult> GetByPetIdAsync(Guid petId)
        {
            var result = await _characteristicService.GetByPetIdAsync(petId);
            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(result.DataList);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCaracteristic([FromBody] CreateCharacteristicModel createCharacteristic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<CharacteristicDomainModel> result;
            try
            {
                result = await _characteristicService.CreateAsync(createCharacteristic.Name);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
               
            }
            return Ok(result.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{characteristicId:Guid}")]
        public async Task<ActionResult> DeleteCharacteristic(Guid characteristicId)
        {
            var result = await _characteristicService.DeleteAsync(characteristicId);

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            return Accepted();

        }

    }
}
