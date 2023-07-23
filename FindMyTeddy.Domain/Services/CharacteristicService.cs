using FindMyTeddy.Data.Entities;
using FindMyTeddy.Domain.Interfaces;
using FindMyTeddy.Domain.Models;
using FindMyTeddy.Repositories;
using FindMyTeddy.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace FindMyTeddy.Domain.Services
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly IPetRepository _petRepository;

        public CharacteristicService(ICharacteristicRepository characteristicRepository, IPetRepository petRepository)
        {
            _characteristicRepository = characteristicRepository;
            _petRepository = petRepository;
        }

        public async Task <GenericDomainModel<CharacteristicDomainModel>> CreateAsync(string characteristicName)
        {
            var createCharacteristic = new Characteristic
            {
               Name = characteristicName,
            };

            var createdCharacteristic = await _characteristicRepository.InsertAsync(createCharacteristic);
            if (createdCharacteristic == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Creation failed!"
                };
            }

            await _characteristicRepository.SaveAsync();

            return new GenericDomainModel<CharacteristicDomainModel>
            {
                IsSuccessful = true,
                Data = new CharacteristicDomainModel
                {
                   Name=createdCharacteristic.Name,
                   Id=createdCharacteristic.Id
                }
            };
        }

        public async Task<GenericDomainModel<CharacteristicDomainModel>> DeleteAsync(Guid characteristicId)
        {
            var characteristic = await _characteristicRepository.GetByIdAsync(characteristicId);
            if (characteristic == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Characteristic not found!"
                };
            }

            var deleted = _characteristicRepository.Delete(characteristicId);
            if (deleted == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Delete failed!"
                };

            }

            await _characteristicRepository.SaveAsync();
            return new GenericDomainModel<CharacteristicDomainModel>
            {
                IsSuccessful = true
            };
        }

        public async Task<GenericDomainModel<CharacteristicDomainModel>> GetAllAsync()
        {

            var characteristics = await _characteristicRepository.GetAllAsync();
            if (characteristics == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Characteristics not found!"

                };
            }

            return new GenericDomainModel<CharacteristicDomainModel>
            {
                IsSuccessful = true,
                DataList = characteristics.Select(characteristic =>
                    new CharacteristicDomainModel
                    {
                        Name = characteristic.Name,
                        Id = characteristic.Id,

                    }).ToList()
            };
        }

  

        public async Task<GenericDomainModel<CharacteristicDomainModel>> GetByIdAsync(Guid characteristicId)
        {
            var characteristic = await _characteristicRepository.GetByIdAsync(characteristicId);
            if (characteristic == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Characteristic not found!"

                };
            }
            var characteristicModel = new CharacteristicDomainModel
            {
                Id = characteristic.Id,
                Name = characteristic.Name,

            };
            return new GenericDomainModel<CharacteristicDomainModel>
            {
                IsSuccessful = true,
                Data = characteristicModel
            };
        }

        public async Task<GenericDomainModel<CharacteristicDomainModel>> GetByPetIdAsync(Guid petId)
        {
            var pet = await _petRepository.GetByIdAsync(petId);
            if (pet == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pet not found!"

                };
            }

            var characteristics = await _characteristicRepository.GetByPetId(petId);
            if (characteristics == null)
            {
                return new GenericDomainModel<CharacteristicDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Characteristics not found!"

                };
            }
          
            return new GenericDomainModel<CharacteristicDomainModel>
            {
                IsSuccessful = true,
                DataList = characteristics.Select(characteristic =>
                    new CharacteristicDomainModel
                    {
                        Name = characteristic.Name,
                        Id = characteristic.Id,

                    }).ToList()
            };
        }
    }
}
