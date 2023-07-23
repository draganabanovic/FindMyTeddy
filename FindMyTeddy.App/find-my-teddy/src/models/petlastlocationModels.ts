export interface ILastLocationModel {
  id: string;
  petId: string;
  latitude: number;
  longitude: number;
  lastLocationDateTime: Date;
  isRelevant: boolean;
}
