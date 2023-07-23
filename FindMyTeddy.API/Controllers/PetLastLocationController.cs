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

namespace FindMyTeddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetLastLocationController : ControllerBase
    {
        private IPetLastLocationService _petLastLocationService;
        public PetLastLocationController(IPetLastLocationService petLastLocationService)
        {
           _petLastLocationService= petLastLocationService;
        }

        [HttpGet("pet/{petId:Guid}")]
        public async Task<ActionResult> GetByPetId(Guid petId)
        {
            var result = await _petLastLocationService.GetByPetIdAsync(petId);
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
        [HttpPost]
        public async Task<ActionResult> AddLocation([FromBody] CreatePetLastLocationModel createLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<PetLastLocationDomainModel> result;
            try
            {
                result = await _petLastLocationService.CreateAsync(new PetLastLocationDomainModel
                {
                    Latitude = createLocation.Latitude,
                    Longitude = createLocation.Longitude,
                    LastLocationDateTime = createLocation.LastLocationDateTime,
                    IsRelevant = createLocation.IsRelevant,
                    PetId = createLocation.PetId
                });
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

        [HttpPut("deactivate-previous/pet/{petId:Guid}")]
        public async Task<ActionResult> DeactivateLocation(Guid petId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<PetLastLocationDomainModel> result;
            try
            {
                result = await _petLastLocationService.DeactivatePreviousLastLocationsAsync(petId);
                    
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
            return Accepted();

        }

    }

       

      
    }

