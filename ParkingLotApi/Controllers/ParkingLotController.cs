﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController : ControllerBase
    {
        private IParkingLotService _parkingLotService;

        public ParkingLotController(IParkingLotService parkingLotService)
        {
            _parkingLotService = parkingLotService;
        }

        [HttpGet]
        public async Task<List<ParkingLotDto>> List()
        {
            return await _parkingLotService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            if (parkingLotDto.Capacity < 0)
            {
                return Accepted();
            }

            var id = await _parkingLotService.AddParkingLot(parkingLotDto);
            if (id == -1)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(GetParkingLotById), new { id = id }, id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotById([FromRoute]int id)
        {
            var parkingLotDt = await _parkingLotService.GetParkingLotById(id);
            if (parkingLotDt == null)
            {
                return NotFound();
            }

            return Ok(parkingLotDt);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ParkingLotDto>> DeleteParkingLotById([FromRoute] int id)
        {
            var parkingLotDtFound = await _parkingLotService.GetParkingLotById(id);
            if (parkingLotDtFound == null)
            {
                return NotFound();
            }

            var parkingLotDto = await _parkingLotService.DeleteParkingLotById(id);
            return Ok(parkingLotDto);
        }

        [HttpGet]
        [Route("byPage")]
        public async Task<ActionResult<List<ParkingLotDto>>> GetParkingLotsByPage([FromQuery] int page)
        { 
            var parkingLotDtos = await _parkingLotService.GetParkingLotByPage(page);
            if (parkingLotDtos == null)
            {
                return Accepted();
            }

            return parkingLotDtos;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotsCapacityById([FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
        {
            var parkingLotDtoReturn = await _parkingLotService.UpdateParkingLot(id, parkingLotDto);
            if (parkingLotDtoReturn == null)
            {
                return NotFound();
            }

            return Ok(parkingLotDtoReturn);
        }
    }
}
