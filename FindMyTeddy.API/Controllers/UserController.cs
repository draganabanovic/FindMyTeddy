using CryptoTrading.API.Models;
using FindMyTeddy.API.JwtMenager;
using FindMyTeddy.API.Models;
using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Domain.Services;
using FindMyTeddy.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FindMyTeddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public UserController(IUserService userService,IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _userService.GetAllAsync();

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
        [HttpGet("{userId:Guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid userId)
        {
            var result = await _userService.GetByIdAsync(userId);
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
        [AllowAnonymous]
        [HttpGet("petId/{petId:Guid}")]
        public async Task<ActionResult> GetByPetId(Guid petId)
        {
            var result = await _userService.GetByPetIdAsync(petId);
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register([FromForm] FileUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = JsonConvert.DeserializeObject<CreateUserModel>(userModel.UserData);


            string? pictureAddres =  userModel.PictureFile==null ? "Images/Users/default.png" : "Images/Users/" + Guid.NewGuid().ToString() + Path.GetExtension(userModel.PictureFile.FileName);

            GenericDomainModel<UserDomainModel> result;
            try
            {
                result = await _userService.CreateAsync(new UserDomainModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    City = user.City,
                    Street = user.Street,
                    ProfilePicture = pictureAddres,
                    Phone = user.Phone
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

            if (userModel.PictureFile != null)
            {
                string path = Path.Combine("wwwroot", pictureAddres);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    try
                    {
                        userModel.PictureFile.CopyTo(stream);
                    }
                    catch(Exception ex) {
                        ErrorResponseModel errorResponse = new ErrorResponseModel
                        {
                            ErrorMessage = "Account is created, but image was not saved",
                            StatusCode = System.Net.HttpStatusCode.BadRequest
                        };
                        return BadRequest(errorResponse);
                    }
                }
            }
            

            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<UserDomainModel> result;
            try
            {
                result = await _userService.LoginAsync(user.Email,user.Password);
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


            var jwtResult = _jwtAuthManager.GenerateTokens(result.Data, DateTime.Now);


            return Ok(jwtResult);
        }

        [HttpDelete("{userId:Guid}")]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteAsync(userId);

            if (!result.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = result.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            string oldPictureAddres = result.Data.ProfilePicture.Replace(Helper.API_URL, "");
            bool deleteOld = oldPictureAddres != "Images/Users/default.png";
            string path = Path.Combine("wwwroot", oldPictureAddres);

            if (System.IO.File.Exists(path) && deleteOld)
            {
                System.IO.File.Delete(path);
            }

            return Accepted();

        }

        [HttpPut]
        public async Task<ActionResult> Update([FromForm] FileUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = JsonConvert.DeserializeObject<UpdateUserModel>(userModel.UserData);

            string oldPictureAddres = user.ProfilePicture.Replace(Helper.API_URL, "");
            string newPictureAddres = "";
            if (userModel.PictureFile != null)
            {
                newPictureAddres = "Images/Users/" + Guid.NewGuid().ToString() + Path.GetExtension(userModel.PictureFile.FileName);
            }
            bool deleteOld = oldPictureAddres != "Images/Users/default.png";

            GenericDomainModel<UserDomainModel> result;
            try
            {
                result = await _userService.UpdateAsync(new UserDomainModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    City = user.City,
                    Street = user.Street,
                    ProfilePicture = userModel.PictureFile == null ? oldPictureAddres : newPictureAddres,
                    Phone = user.Phone
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
            if (userModel.PictureFile != null)
            {
                string oldPath = Path.Combine("wwwroot", oldPictureAddres);
                string newPath = Path.Combine("wwwroot", newPictureAddres);

                if (System.IO.File.Exists(oldPath) && deleteOld)
                {
                    System.IO.File.Delete(oldPath);
                }

                using (Stream stream = new FileStream(newPath, FileMode.Create))
                {
                    userModel.PictureFile.CopyTo(stream);
                }
            }

            return Ok(result.Data);
        }

    }


    
}
