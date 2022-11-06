﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingLotDbContext _parkingLotDbContext;
        public ParkingLotService(ParkingLotDbContext parkingLotDbContext)
        {
            _parkingLotDbContext = parkingLotDbContext;
        }

        public List<ParkingLotDto> GetAll()
        {
            var parkingLots= _parkingLotDbContext.ParkingLots;
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public int AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = parkingLotDto.ToEntity();
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Name == parkingLotEntity.Name);
            if (foundParkingLotEntity != null)
            {
                return -1;
            }
            _parkingLotDbContext.AddAsync(parkingLotEntity);
            _parkingLotDbContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public ParkingLotDto GetParkingLotById(int id)
        {
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Id == id);
            if (foundParkingLotEntity == null)
            {
                return null;
            }

            return new ParkingLotDto(foundParkingLotEntity);
        }

        public ParkingLotDto DeleteParkingLotById(int id)
        {
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Id == id);
            if (foundParkingLotEntity == null)
            {
                return null;
            }

            _parkingLotDbContext.Remove(foundParkingLotEntity);
            _parkingLotDbContext.SaveChangesAsync();
            return new ParkingLotDto(foundParkingLotEntity);
        }
    }
}
