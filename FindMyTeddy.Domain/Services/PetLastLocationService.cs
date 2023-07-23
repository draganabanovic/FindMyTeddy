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
    public class PetLastLocationService : IPetLastLocationService
    {
        private readonly IPetLastLocationRepository _petLastLocationRepository;
        private readonly IPetRepository _petRepository;

        public PetLastLocationService(IPetLastLocationRepository petLastLocationRepository, IPetRepository petRepository )
        {
            _petLastLocationRepository = petLastLocationRepository;
            _petRepository = petRepository;
        }

        public async Task<GenericDomainModel<PetLastLocationDomainModel>> CreateAsync(PetLastLocationDomainModel newLastLocation)
        {
            var pet = await _petRepository.GetByIdAsync( newLastLocation.PetId );
            if (pet == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pet not found!"
                };
            }

            var createLocation = new PetLastLocation
            {
                Latitude = newLastLocation.Latitude,
                Longitude= newLastLocation.Longitude,
                LastLocationDateTime = newLastLocation.LastLocationDateTime,
                IsRelevant = newLastLocation.IsRelevant,
                Pet = pet
            };

            var createdlocation = await _petLastLocationRepository.InsertAsync(createLocation);
            if (createdlocation == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed!"
                };
            }

            await _petLastLocationRepository.SaveAsync();

            return new GenericDomainModel<PetLastLocationDomainModel>
            {
                IsSuccessful = true,
                Data = new PetLastLocationDomainModel
                {
                    Id = newLastLocation.Id,
                    Latitude=createdlocation.Latitude,
                    Longitude=createdlocation.Longitude,
                    LastLocationDateTime=createdlocation.LastLocationDateTime,
                    IsRelevant=createdlocation.IsRelevant,

                }
            };

        }

        public async Task<GenericDomainModel<PetLastLocationDomainModel>> DeactivatePreviousLastLocationsAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"

                };
            }

            var relevantLastLocations = await _petLastLocationRepository.GetRelevantByPetIdAsync(petId);
            if (relevantLastLocations == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "No relevant last locations!"
                };
            }

            var updateFailed = false;

            foreach (var location in relevantLastLocations) {
                location.IsRelevant = false;
                var update = _petLastLocationRepository.Update(location);

                if (update == null)
                {
                    updateFailed = true;
                }
            }

            if (updateFailed == true)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Update failed!"
                };
            }

            await _petLastLocationRepository.SaveAsync();

            return new GenericDomainModel<PetLastLocationDomainModel>
            {
                IsSuccessful = true,
            };
        }

        public async Task<GenericDomainModel<PetLastLocationDomainModel>> GetByPetIdAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "PET NOT FOUND!!"

                };
            }
            var locations = await _petLastLocationRepository.GetRelevantByPetIdAsync(petId);
            if (locations == null)
            {
                return new GenericDomainModel<PetLastLocationDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "LOCATION NOT FOUND!!"

                };
            }

            return new GenericDomainModel<PetLastLocationDomainModel>
            {
                IsSuccessful = true,
                DataList = locations.Select(location =>
                    new PetLastLocationDomainModel
                    {
                        Longitude = location.Longitude,
                        Latitude = location.Latitude,
                        IsRelevant = location.IsRelevant,
                        LastLocationDateTime = location.LastLocationDateTime,
                        Id = location.Id
                    }).ToList()
            };
        }
    }
}
