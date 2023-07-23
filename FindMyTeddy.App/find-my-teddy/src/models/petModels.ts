import { ICharacteristicModel } from "./characteristicModels";
import { ILastLocationModel } from "./petlastlocationModels";

export interface IPetModel {
  id: string;
  ownerId: string;
  characteristicIds: string[];
  characteristics: ICharacteristicModel[];
  petLastLocationIds: string[];
  petLastLocations: ILastLocationModel[];
  name: string;
  type: string;
  breed: string;
  picture: string;
  lostStatus: boolean;
  description: string;
  isSubscribed: boolean;
  disappearanceDate: string | null;
  zipCode: string;
}

export interface ICreatePetModel {
  id: string;
  ownerId: string;
  characteristicIds: string[];
  name: string;
  type: string;
  breed: string;
  lostStatus: boolean;
  description: string;
  isSubscribed: boolean;
  disappearanceDate: string;
  zipCode: string;
}
