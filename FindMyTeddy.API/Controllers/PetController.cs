using CryptoTrading.API.Models;
using FindMyTeddy.API.Models;
using FindMyTeddy.Data;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Domain.Services;
using FindMyTeddy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FindMyTeddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetController : ControllerBase
    {
        private readonly IServer _server;
        private IPetService _petService;
        public PetController(IPetService petService, IServer server)
        {
            _petService = petService;
            _server = server;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _petService.GetAllAsync();

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
        [HttpGet("{petId:Guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid petId)
        {
            var result = await _petService.GetByIdAsync(petId);
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

        [HttpGet("owner/{ownerId:Guid}")]
        public async Task<ActionResult> GetByOwnerId(Guid ownerId)
        {
            var result = await _petService.GetByOwnerIdAsync(ownerId);
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
        [HttpGet("lost/fromdate/{date:DateTime}")]
        public async Task<ActionResult> GetLostFromDate( DateTime date)

        {
            var result = await _petService.GetLostFromDateAsync(date);
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
        [HttpGet("lost/zipcode/{zipCode}")]
        public async Task<ActionResult> GetLostForZipCode(string zipCode)

        {
            var result = await _petService.GetLostForZipCode(zipCode);
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
        [HttpGet("lost")]
        public async Task<ActionResult> GetLost()
        {
            var result = await _petService.GetAllLostAsync();
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

        [HttpPost]
        public async Task<ActionResult> AddPet([FromForm] FilePetModel petModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pet = JsonConvert.DeserializeObject<CreatePetModel>(petModel.PetData);
            string pictureAddres = petModel.PictureFile == null ? "Images/Pets/default.png" : "Images/Pets/" + Guid.NewGuid().ToString() + Path.GetExtension(petModel.PictureFile.FileName);

            GenericDomainModel<PetDomainModel> result;
            try
            {
                result = await _petService.CreateAsync(new PetDomainModel
                {
                    Name = pet.Name,
                    Type = pet.Type,
                    Breed = pet.Breed,
                    Picture = pictureAddres,
                    ZipCode = pet.ZipCode,  
                    Description = pet.Description,
                    DisappearanceDate = pet.DisappearanceDate,
                    IsSubscribed = pet.IsSubscribed,
                    LostStatus = pet.LostStatus,
                    OwnerId = new Guid(pet.OwnerId),
                    CharacteristicIds = pet.CharacteristicIds.IsNullOrEmpty() ?null : pet.CharacteristicIds.Select(Guid.Parse).ToList(),
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

            if (petModel.PictureFile != null)
            {
                string path = Path.Combine("wwwroot", pictureAddres);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    petModel.PictureFile.CopyTo(stream);
                }
            }
               
            return Ok();
        }

        [HttpDelete("{petId:Guid}")]
        public async Task<ActionResult> DeletePet(Guid petId)
        {
            var result = await _petService.DeleteAsync(petId);

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            string oldPictureAddres = result.Data.Picture.Replace(Helper.API_URL, "");
            bool deleteOld = oldPictureAddres != "Images/Pets/default.png";
            string path = Path.Combine("wwwroot", oldPictureAddres);
   
            if (System.IO.File.Exists(path) && deleteOld)
            {
                System.IO.File.Delete(path);
            }

            return Accepted();

        }

        [HttpPut]
        public async Task<ActionResult> UpdatePet([FromForm] FilePetModel petModel)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            var changePet = JsonConvert.DeserializeObject<UpdatePetModel>(petModel.PetData);

            string oldPictureAddres = changePet.Picture.Replace(Helper.API_URL, "");
            string newPictureAddres= "";
            if (petModel.PictureFile != null)
            {
                newPictureAddres = "Images/Pets/" + Guid.NewGuid().ToString() + Path.GetExtension(petModel.PictureFile.FileName);
            }
            bool deleteOld = oldPictureAddres != "Images/Pets/default.png";

            GenericDomainModel<PetDomainModel> result;
            try
            {
                result = await _petService.ChangeAsync(new PetDomainModel
                {
                    Name = changePet.Name,
                    Picture = petModel.PictureFile == null? oldPictureAddres : newPictureAddres,
                    Type = changePet.Type,
                    Breed = changePet.Breed,
                    Description = changePet.Description,
                    DisappearanceDate = changePet.DisappearanceDate,
                    IsSubscribed = changePet.IsSubscribed,
                    LostStatus = changePet.LostStatus,
                    OwnerId = new Guid(changePet.OwnerId),
                    CharacteristicIds = changePet.CharacteristicIds.IsNullOrEmpty() ? null : changePet.CharacteristicIds.Select(Guid.Parse).ToList(),
                    Id = new Guid(changePet.Id),
                    ZipCode =changePet.ZipCode
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

            if (petModel.PictureFile != null)
            {
                string oldPath = Path.Combine("wwwroot", oldPictureAddres);
                string newPath = Path.Combine("wwwroot", newPictureAddres);

                if (System.IO.File.Exists(oldPath) && deleteOld)
                {
                    System.IO.File.Delete(oldPath);
                }

                using (Stream stream = new FileStream(newPath, FileMode.Create))
                {
                    petModel.PictureFile.CopyTo(stream);
                }
            }
        
            return Ok(result.Data);

        }


        [HttpPut("status")]
        public async Task<ActionResult> ChangeStatus([FromBody] UpdatePetStatusModel changestatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            GenericDomainModel<PetDomainModel> result;
            try
            {
                result = await _petService.ChangeStatusAsync(changestatus.Id,changestatus.LostStatus);
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
    }







}



