using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Repositories;
using FindMyTeddy.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;
using System.Security.Claims;

namespace FindMyTeddy.Domain.Services
{
    public class UserService : IUserService

    {
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, IPetRepository petRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _petRepository=petRepository;
            _userManager = userManager;
        }
        

        public async Task<GenericDomainModel<UserDomainModel>> CreateAsync(UserDomainModel newUser)
        {

            var checkUniqueEmail = await _userManager.FindByEmailAsync(newUser.Email);
            if (checkUniqueEmail != null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Email already exists"
                };
            }

            var user = new User
            {
               FirstName = newUser.FirstName,
               LastName = newUser.LastName,
               Email = newUser.Email,
               Role = "User",
               City = newUser.City,
               Street = newUser.Street,
               ProfilePicture = newUser.ProfilePicture,
               PhoneNumber = newUser.Phone,
               UserName = newUser.Email
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed!"
                };
            }

            var registeredUserToFetchId = await _userManager.FindByEmailAsync(user.Email);

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   Role = user.Role,
                   City = user.City,
                   Street = user.Street,
                   ProfilePicture = Helper.API_URL + user.ProfilePicture,
                   Id = registeredUserToFetchId.Id,
                   Phone = user.PhoneNumber,
                }
            };

        }

        public async Task<GenericDomainModel<UserDomainModel>> UpdateAsync(UserDomainModel newUser)
        {
            var user = await _userManager.FindByIdAsync(newUser.Id.ToString());
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "User not found!"

                };
            }

            var userByEmail = await _userManager.FindByEmailAsync(newUser.Email);
            if (userByEmail != null && userByEmail.Id != user.Id)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Email already exists"
                };
            }


            if (newUser.Password != null && newUser.Password!= "")
            {
                var resultDelete = await _userManager.RemovePasswordAsync(user);

                if (!resultDelete.Succeeded)
                { 
                    return new GenericDomainModel<UserDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Password change failed!"
                    };
                }

                var resultNewPassword = await _userManager.AddPasswordAsync(user, newUser.Password);

                if (!resultNewPassword.Succeeded)
                {
                    return new GenericDomainModel<UserDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Password change failed!"
                    };
                }
            }

            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            user.City = newUser.City;
            user.Street = newUser.Street;
            user.PhoneNumber = newUser.Phone;
            user.ProfilePicture = newUser.ProfilePicture;


            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed!"
                };
            }

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    City = user.City,
                    Street = user.Street,
                    ProfilePicture = Helper.API_URL + user.ProfilePicture,
                    Id = user.Id,
                    Phone = user.PhoneNumber,
                }
            };

        }

        public async  Task<GenericDomainModel<UserDomainModel>> DeleteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "USER NOT FOUND!!"
                };
            }

            var deleted = _userRepository.Delete(userId);
            if (deleted == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Delete failed!"
                };
            }

            await _userRepository.SaveAsync();

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "User not fount!"

                };
            }

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                DataList = users.Select(user =>
                    new UserDomainModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id,
                        Email = user.Email,
                        Street = user.Street,
                        City = user.City,
                        ProfilePicture = Helper.API_URL + user.ProfilePicture,
                        Role = user.Role,
                        Phone = user.PhoneNumber,

                    }).ToList()
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "USER NOT FOUND!!"

                };
            }
            var userModel = new UserDomainModel
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                Id = user.Id,
                Email = user.Email,
                Street=user.Street,
                City = user.City,
                ProfilePicture= Helper.API_URL + user.ProfilePicture,
                Role = user.Role,
                Phone = user.PhoneNumber,


            };
            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = userModel
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> GetByPetIdAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"

                };
            }
            var user = await _userRepository.GetByPetIdAsync(petId);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "USER NOT FOUND!!"

                };
            }

            var userModelList =
              new UserDomainModel
              {
                  FirstName = user.FirstName,
                  LastName = user.LastName,
                  Id = user.Id,
                  Email = user.Email,
                  Street = user.Street,
                  City = user.City,
                  ProfilePicture = Helper.API_URL + user.ProfilePicture,
                  Role = user.Role,
                  Phone = user.PhoneNumber,
              };

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = userModelList
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Incorrect email or password"
                };
            }

            var result = await _userManager.CheckPasswordAsync(user,password);

            if (!result)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Incorrect email or password"
                };
            }


            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = new UserDomainModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    Email = user.Email,
                    Street = user.Street,
                    City = user.City,
                    ProfilePicture = Helper.API_URL + user.ProfilePicture,
                    Role = user.Role,
                    Phone = user.PhoneNumber,
                }
        };
        }

       
    }
}
