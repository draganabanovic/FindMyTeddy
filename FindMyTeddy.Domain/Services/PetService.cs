using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Repositories;
using FindMyTeddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Services
{
   public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly IPetLastLocationRepository _petLastLocationRepository;


        public PetService(IPetRepository petRepository, 
            IUserRepository userRepository, 
            ICharacteristicRepository characteristicRepository,
            IPetLastLocationRepository petLastLocationRepository)
        {
            _petRepository = petRepository;
            _userRepository = userRepository;
            _characteristicRepository = characteristicRepository;
            _petLastLocationRepository=petLastLocationRepository;
        }

        public async Task<GenericDomainModel<PetDomainModel>> ChangeAsync(PetDomainModel changedPet)
        {
            var pet = await _petRepository.GetByIdAsync(changedPet.Id);
            if (pet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"
                };
            }

            List<Characteristic> characteristics = new List<Characteristic>();

            if (changedPet.CharacteristicIds != null)
            {
                foreach (var cId in changedPet.CharacteristicIds)
                {
                    var characteristic = await _characteristicRepository.GetByIdAsync(cId);
                    if (characteristic == null)
                    {
                        return new GenericDomainModel<PetDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = "Characteristic not found!"
                        };
                    }

                    characteristics.Add(characteristic);
                }
            }
            if (pet.LostStatus != changedPet.LostStatus && changedPet.LostStatus == false)
            {
               await _petLastLocationRepository.DeactivateAllPreviousAsync(pet.Id);
            }
            pet.Name = changedPet.Name;
            pet.Picture = changedPet.Picture;
            pet.Type = changedPet.Type;
            pet.Breed = changedPet.Breed;
            pet.Description = changedPet.Description;
            pet.DisappearanceDate = (DateTime)(changedPet.DisappearanceDate == null ? DateTime.MinValue : changedPet.DisappearanceDate);
            pet.IsSubscribed = changedPet.IsSubscribed;
            pet.LostStatus = changedPet.LostStatus;
            pet.ZipCode = changedPet.ZipCode;
            pet.Characteristics = characteristics;
          

            var updatedPet = _petRepository.Update(pet);
            if (updatedPet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Update failed!"
                };
            }

            await _petRepository.SaveAsync();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                Data = new PetDomainModel
                {
                    Name = updatedPet.Name,
                    Picture = Helper.API_URL + updatedPet.Picture,
                    Type = updatedPet.Type,
                    Breed = updatedPet.Breed,
                    Description = updatedPet.Description,
                    DisappearanceDate = updatedPet.DisappearanceDate,
                    Id = updatedPet.Id,
                    IsSubscribed = updatedPet.IsSubscribed,
                    LostStatus = updatedPet.LostStatus,
                    ZipCode = updatedPet.ZipCode,
                    
                }
            };

        }

        public async Task<GenericDomainModel<PetDomainModel>> ChangeStatusAsync(Guid petId, bool status)
        {

            var pet= await _petRepository.GetByIdAsync(petId);
            if(pet==null) 
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"
                };
            }

            if (pet.LostStatus != status && status == false)
            {
                await _petLastLocationRepository.DeactivateAllPreviousAsync(pet.Id);
            }

            pet.LostStatus = status;
            pet.DisappearanceDate = status? DateTime.Now: DateTime.MinValue;

            var updatedPet= _petRepository.Update(pet);
            if (updatedPet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Update failed!"
                };
            }

            await _petRepository.SaveAsync();

      
            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                Data = new PetDomainModel
                {
                    Name = updatedPet.Name,
                    Picture = Helper.API_URL + updatedPet.Picture,
                    Type = updatedPet.Type,
                    Breed = updatedPet.Breed,
                    Description = updatedPet.Description,
                    DisappearanceDate = updatedPet.DisappearanceDate,
                    Id = updatedPet.Id,
                    IsSubscribed = updatedPet.IsSubscribed,
                    LostStatus = updatedPet.LostStatus,
                    ZipCode = updatedPet.ZipCode,
                }
            };

        }

        public async Task<GenericDomainModel<PetDomainModel>> CreateAsync(PetDomainModel newPet)
        {
            var owner = await _userRepository.GetByIdAsync(newPet.OwnerId);
            if (owner == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Owner not found!"

                };
            }

            List<Characteristic> characteristics = new List<Characteristic>(); //prazna lista u koju se kasnije dodaju karakteristike

            if (newPet.CharacteristicIds != null)
            {
                foreach (var cId in newPet.CharacteristicIds)
                {
                    var characteristic = await _characteristicRepository.GetByIdAsync(cId);
                    if (characteristic == null)
                    {
                        return new GenericDomainModel<PetDomainModel>
                        {
                            IsSuccessful = false,
                            ErrorMessage = "Characteristic not found!"
                        };
                    }

                    characteristics.Add(characteristic);
                }
            }
           

            var createPet = new Pet
            {
                Name = newPet.Name,
                Picture = newPet.Picture,
                Type = newPet.Type,
                Breed = newPet.Breed,
                Description = newPet.Description,
                DisappearanceDate = (DateTime)(newPet.DisappearanceDate == null ?DateTime.MinValue: newPet.DisappearanceDate),
                IsSubscribed = newPet.IsSubscribed,
                LostStatus = newPet.LostStatus,
                Owner= owner,
                ZipCode = newPet.ZipCode,
                Characteristics= characteristics,
            };

            var createdPet = await _petRepository.InsertAsync(createPet);
            if (createdPet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed!"
                };
            }

            await _petRepository.SaveAsync();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                Data = new PetDomainModel
                {
                    Id = createdPet.Id,
                    Name = createdPet.Name,
                    Picture = Helper.API_URL + createdPet.Picture,
                    Type = createdPet.Type,
                    Breed = createdPet.Breed,
                    Description = createdPet.Description,
                    DisappearanceDate = createdPet.DisappearanceDate,
                    IsSubscribed = createdPet.IsSubscribed,
                    LostStatus = createdPet.LostStatus,
                    ZipCode= createdPet.ZipCode,
                    
                }
            };
          
        }

        public async Task<GenericDomainModel<PetDomainModel>> DeleteAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"
                };
            }

            var deleted = _petRepository.Delete(petId);
            if (deleted == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Delete failed!"
                };
            }

            await _petRepository.SaveAsync();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                 Data = new PetDomainModel
                 {
                     Id = deleted.Id,
                     Name = deleted.Name,
                     Picture = Helper.API_URL + deleted.Picture,
                     Type = deleted.Type,
                     Breed = deleted.Breed,
                     Description = deleted.Description,
                     DisappearanceDate = deleted.DisappearanceDate,
                     IsSubscribed = deleted.IsSubscribed,
                     LostStatus = deleted.LostStatus,
                     ZipCode = deleted.ZipCode,
                 }
            };
        }

        public async Task<GenericDomainModel<PetDomainModel>> GetAllAsync()
        {
            var pets = await _petRepository.GetAllAsync();
            if (pets == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PETS NOT FOUND!!"

                };
            }

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                DataList = pets.Select(pet =>
                            new PetDomainModel
                            {
                                Name = pet.Name,
                                Picture = Helper.API_URL + pet.Picture,
                                Type = pet.Type,
                                Breed = pet.Breed,
                                Description = pet.Description,
                                DisappearanceDate = pet.DisappearanceDate,
                                Id = pet.Id,
                                ZipCode = pet.ZipCode,
                                IsSubscribed = pet.IsSubscribed,
                                LostStatus = pet.LostStatus,
                                OwnerId = pet.Owner.Id,
                                CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(), 
                                Characteristics = pet.Characteristics.Select(ch 
                                                                => new CharacteristicDomainModel 
                                                                { 
                                                                    Id = ch.Id, 
                                                                    Name = ch.Name 
                                                                }).ToList(),
                                PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                                PetLastLocations = pet.PetLastLocations.Select(ll 
                                                                => new PetLastLocationDomainModel 
                                                                { 
                                                                    Id = ll.Id,
                                                                    IsRelevant = ll.IsRelevant,
                                                                    LastLocationDateTime = ll.LastLocationDateTime,   
                                                                    Latitude = ll.Latitude,   
                                                                    Longitude = ll.Longitude
                                                                }).ToList(),
                            }).ToList()
        };
        }

        public async Task<GenericDomainModel<PetDomainModel>> GetAllLostAsync()
        {
            var pets = await _petRepository.GetLost();
            if (pets == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PETS NOT FOUND!!"

                };
            }

            var petsModelList = pets.Select(pet =>
                new PetDomainModel
                {
                    Name = pet.Name,
                    Picture = Helper.API_URL + pet.Picture,
                    Type = pet.Type,
                    Breed = pet.Breed,
                    Description = pet.Description,
                    DisappearanceDate = pet.DisappearanceDate,
                    Id = pet.Id,
                    IsSubscribed = pet.IsSubscribed,
                    LostStatus = pet.LostStatus,
                    ZipCode = pet.ZipCode,
                    OwnerId = pet.Owner.Id,
                    CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(),
                    Characteristics = pet.Characteristics.Select(ch
                                                    => new CharacteristicDomainModel
                                                    {
                                                        Id = ch.Id,
                                                        Name = ch.Name
                                                    }).ToList(),
                    PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                    PetLastLocations = pet.PetLastLocations.Select(ll
                                                    => new PetLastLocationDomainModel
                                                    {
                                                        Id = ll.Id,
                                                        IsRelevant = ll.IsRelevant,
                                                        LastLocationDateTime = ll.LastLocationDateTime,
                                                        Latitude = ll.Latitude,
                                                        Longitude = ll.Longitude
                                                    }).ToList(),
                }).ToList();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                DataList = petsModelList
            };
        }

        public async Task<GenericDomainModel<PetDomainModel>> GetByIdAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"

                };
            }
            var petModel = new PetDomainModel
            {
                Name = pet.Name,
                Picture = Helper.API_URL + pet.Picture,
                Type = pet.Type,
                Breed = pet.Breed,
                Description = pet.Description,
                DisappearanceDate = pet.DisappearanceDate,
                Id = pet.Id,
                IsSubscribed = pet.IsSubscribed,
                LostStatus = pet.LostStatus,
                ZipCode = pet.ZipCode,
                OwnerId = pet.Owner.Id,
                CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(),
                Characteristics = pet.Characteristics.Select(ch
                                                => new CharacteristicDomainModel
                                                {
                                                    Id = ch.Id,
                                                    Name = ch.Name
                                                }).ToList(),
                PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                PetLastLocations = pet.PetLastLocations.Select(ll
                                                => new PetLastLocationDomainModel
                                                {
                                                    Id = ll.Id,
                                                    IsRelevant = ll.IsRelevant,
                                                    LastLocationDateTime = ll.LastLocationDateTime,
                                                    Latitude = ll.Latitude,
                                                    Longitude = ll.Longitude
                                                }).ToList(),
            };
            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                Data = petModel
            };
        }

        public async Task<GenericDomainModel<PetDomainModel>> GetByOwnerIdAsync(Guid ownerId)
        {
            var user = await _userRepository.GetByIdAsync(ownerId);
            if (user == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "USER NOT FOUND!!"

                };
            }

            var pets = await _petRepository.GetByUserIdAsync(ownerId);
            if (pets == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PETS NOT FOUND!!"

                };
            }

            var petsModelList = pets.Select(pet =>
              new PetDomainModel
              {
                  Name = pet.Name,
                  Picture = Helper.API_URL + pet.Picture,
                  Type = pet.Type,
                  Breed = pet.Breed,
                  Description = pet.Description,
                  ZipCode = pet.ZipCode,
                  DisappearanceDate = pet.DisappearanceDate == default(DateTime) ? null: pet.DisappearanceDate,
                  Id = pet.Id,
                  IsSubscribed = pet.IsSubscribed,
                  LostStatus = pet.LostStatus,
                  OwnerId = pet.Owner.Id,
                  CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(),
                  Characteristics = pet.Characteristics.Select(ch
                                                  => new CharacteristicDomainModel
                                                  {
                                                      Id = ch.Id,
                                                      Name = ch.Name
                                                  }).ToList(),
                  PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                  PetLastLocations = pet.PetLastLocations.Select(ll
                                                  => new PetLastLocationDomainModel
                                                  {
                                                      Id = ll.Id,
                                                      IsRelevant = ll.IsRelevant,
                                                      LastLocationDateTime = ll.LastLocationDateTime,
                                                      Latitude = ll.Latitude,
                                                      Longitude = ll.Longitude
                                                  }).ToList(),
              }).ToList();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                DataList = petsModelList
            };
        }


        public async Task<GenericDomainModel<PetDomainModel>> GetLostFromDateAsync(DateTime fromDate)
        {
            var pets = await _petRepository.GetLostFromDate(fromDate);
            if (pets == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PETS NOT FOUND!!"

                };
            }

            var petsModelList = pets.Select(pet =>
                new PetDomainModel
                {
                    Name = pet.Name,
                    Picture = Helper.API_URL + pet.Picture,
                    Type = pet.Type,
                    Breed = pet.Breed,
                    Description = pet.Description,
                    DisappearanceDate = pet.DisappearanceDate,
                    Id = pet.Id,
                    IsSubscribed = pet.IsSubscribed,
                    LostStatus = pet.LostStatus,
                    ZipCode = pet.ZipCode,
                    OwnerId = pet.Owner.Id,
                    CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(),
                    Characteristics = pet.Characteristics.Select(ch
                                                    => new CharacteristicDomainModel
                                                    {
                                                        Id = ch.Id,
                                                        Name = ch.Name
                                                    }).ToList(),
                    PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                    PetLastLocations = pet.PetLastLocations.Select(ll
                                                    => new PetLastLocationDomainModel
                                                    {
                                                        Id = ll.Id,
                                                        IsRelevant = ll.IsRelevant,
                                                        LastLocationDateTime = ll.LastLocationDateTime,
                                                        Latitude = ll.Latitude,
                                                        Longitude = ll.Longitude
                                                    }).ToList(),
                }).ToList();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                DataList = petsModelList
            };
        }
        public async Task<GenericDomainModel<PetDomainModel>> GetLostForZipCode(string zipCode)
        {
            var pets = await _petRepository.GetLostForZipCode(zipCode);
            if (pets == null)
            {
                return new GenericDomainModel<PetDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PETS NOT FOUND!!"

                };
            }

            var petsModelList = pets.Select(pet =>
                new PetDomainModel
                {
                    Name = pet.Name,
                    Picture = Helper.API_URL + pet.Picture,
                    Type = pet.Type,
                    Breed = pet.Breed,
                    Description = pet.Description,
                    DisappearanceDate = pet.DisappearanceDate,
                    Id = pet.Id,
                    IsSubscribed = pet.IsSubscribed,
                    LostStatus = pet.LostStatus,
                    ZipCode = pet.ZipCode,
                    OwnerId = pet.Owner.Id,
                    CharacteristicIds = pet.Characteristics.Select(ch => ch.Id).ToList(),
                    Characteristics = pet.Characteristics.Select(ch
                                                    => new CharacteristicDomainModel
                                                    {
                                                        Id = ch.Id,
                                                        Name = ch.Name
                                                    }).ToList(),
                    PetLastLocationIds = pet.PetLastLocations.Select(ll => ll.Id).ToList(),
                    PetLastLocations = pet.PetLastLocations.Select(ll
                                                    => new PetLastLocationDomainModel
                                                    {
                                                        Id = ll.Id,
                                                        IsRelevant = ll.IsRelevant,
                                                        LastLocationDateTime = ll.LastLocationDateTime,
                                                        Latitude = ll.Latitude,
                                                        Longitude = ll.Longitude
                                                    }).ToList(),
                }).ToList();

            return new GenericDomainModel<PetDomainModel>
            {
                IsSuccessful = true,
                DataList = petsModelList
            };
        }
    }
}
