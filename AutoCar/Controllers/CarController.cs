using AutoCar.Entities;
using AutoCar.Models.DTO;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Data;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("getCars")]
        [Authorize]
        public List<Car> getCars()
        {
            return _carService.GetCars();
        }

        [HttpGet("{userId}/cars")]
        [Authorize(Roles = "Admin,Manager")]
        public List<Car> GetCarsByUserId([FromRoute] int userId)
        {
            return _carService.GetCarsByUserId(userId);
        }

        [HttpGet("{carId}")]
        [Authorize(Roles = "Admin,Manager")]
        public Optional<Car> GetCar([FromRoute] int carId)
        {
            return _carService.GetCar(carId);
        }

        [HttpGet("{carId}/insuranceDays")]
        [Authorize(Roles = "Admin,Manager")]
        public int InsuranceDays([FromRoute] int carId)
        {
            return _carService.InsuranceDays(carId);
        }

        [HttpGet("{carId}/notificationsNumber")]
        [Authorize(Roles = "Admin,Manager")]
        public int NotificationsNumber([FromRoute] int carId)
        {
            return _carService.NotificationsNumber(carId);
        }

        [HttpPost("addNewCar")]
        [Authorize(Roles = "Admin,Manager")]
        public Car AddNewCar([FromRoute] CarDto dto)
        {
            return _carService.AddNewCar(dto);
        }

        [HttpDelete("{carId}")]
        [Authorize(Roles = "Admin,Manager")]
        public void DeleteCar([FromRoute] int carId)
        {
            _carService.DeleteCar(carId);
        }

        [HttpDelete("{carId}/release")]
        [Authorize(Roles = "Admin,Manager")]
        public void ReleaseCarByCarId([FromRoute] int carId)
        {
            _carService.ReleaseCarByCarId(carId);
        }

        [HttpPut("{carId}")]
        [Authorize(Roles = "Admin,Manager")]
        public Car UpdateCar([FromRoute] int carId,
                              UpdateCarDto dto)
        {
            return _carService.UpdateCar(carId, dto);
        }

        [HttpPut("open/{carId}")]
        [Authorize]
        public bool ChangeOpened([FromRoute] int carId)
        {
            return _carService.ChangeOpened(carId);
        }
    }
}