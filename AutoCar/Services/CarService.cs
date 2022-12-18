using AutoCar.Entities;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.Runtime.InteropServices;
using System;
using AutoCar.Exceptions;
using AutoCar.Models.ViewModels;
using AutoCar.Models.DTO;

namespace AutoCar.Services
{
    public interface ICarService
    {
        public List<Car> GetCars();
        public Car GetCar(int carId);
        public List<Car> GetCarsByUserId(int userId);
        public Car AddNewCar(CarDto dto);
        public void DeleteCar(int carId);
        public void ReleaseCarByCarId(int carId);
        public int InsuranceDays(int carId);
        public int RegistrationReviewDays(int carId);
        public int TireChangeDays(int carId);
        public int NotificationsNumber(int carId);
        public bool ChangeOpened(int carId);
        public Car UpdateCar(int id, UpdateCarDto dto);
    }

    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CarService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public List<Car> GetCars()
        {
            var list = _context.Cars.ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Users");

            return list;
        }

        public Car GetCar(int carId)
        {
            Car car = _context.Cars.FirstOrDefault(x => x.Id == carId);
            if (car == null)
            {
                throw new NotFoundException($"Car {carId} does not exist");
            }
            return car;
        }

        public List<Car> GetCarsByUserId(int userId)
        {
            var user = _context.Users
              .FirstOrDefault(c => c.Id == userId);
            if (user is null)
                throw new NotFoundException($"User with id {userId} not exist");
            var cars = _context.Cars.Where(x => x.UserId == userId).ToList();
            return cars;
        }

        public Car AddNewCar(CarDto dto)
        {
            Car car = _context.Cars.FirstOrDefault(x => x.LicensePlate == dto.LicensePlate);
            if (car != null)
            {
                throw new BadRequestException("Car with this license plate number already exists!");
            }

            Car newCar = new Car
            {
                LicensePlate = dto.LicensePlate,
                Brand = dto.Brand,
                Model = dto.Model,
                FuelLevel = dto.FuelLevel,
                FuelType = dto.FuelType,
                RegistrationReview = dto.RegistrationReview,
                Insurance = dto.Insurance,
                TireChange = dto.TireChange
            };

            int userId = (int)_userContextService.GetUserId;
            newCar.UserId = userId;

            _context.Cars.Add(car);
            _context.SaveChanges();
            return newCar;
        }
        
        public void DeleteCar(int carId)
        {
            var car = _context.Cars
               .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");

            _context.Cars.Remove(car);
            _context.SaveChanges();
        }

        public void ReleaseCarByCarId(int carId)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");

            car.UserId = null;
            _context.SaveChanges();
        }

        public int InsuranceDays(int carId)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");

            TimeSpan time = car.Insurance - DateTime.Now;
            return time.Days;
        }
        
        public int RegistrationReviewDays(int carId)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");

            TimeSpan time = car.RegistrationReview - DateTime.Now;
            return time.Days;
        }
        
        public int TireChangeDays(int carId)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");

            TimeSpan time = car.TireChange - DateTime.Now;
            return time.Days;
        }
        
        public int NotificationsNumber(int carId)
        {
            int cnt = 0;
            if (InsuranceDays(carId) <= 0) 
                cnt++;
            if (RegistrationReviewDays(carId) <= 0) 
                cnt++;
            if (TireChangeDays(carId) <= 0) 
                cnt++;
            return cnt;
        }
        
        public bool ChangeOpened(int carId)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == carId);
            if (car is null)
                throw new NotFoundException($"Car with id {carId} not exist");
            car.Opened = !car.Opened;
            _context.SaveChanges();
            return car.Opened;
        }
        
        public Car UpdateCar(int id, UpdateCarDto dto)
        {
            var car = _context.Cars
              .FirstOrDefault(c => c.Id == id);
            if (car is null)
                throw new NotFoundException($"Car with id {id} not exist");

            if (dto.LicensePlate != null && dto.LicensePlate.Length > 0)
                car.LicensePlate = dto.LicensePlate;

            if (dto.Brand != null && dto.Brand.Length > 0)
                car.Brand = dto.Brand;

            if (dto.Model != null && dto.Model.Length > 0)
                car.Model = dto.Model;

            if (dto.FuelLevel != null && dto.FuelLevel >= 0 && dto.FuelLevel <= 4)
                car.FuelLevel = dto.FuelLevel.Value;
            else if (dto.FuelLevel != null)
                throw new BadRequestException("Fuel level format is invalid! (0-4)");

            if (dto.FuelType != null && dto.FuelType.Length > 0)
                car.FuelType = dto.FuelType;

            if (dto.RegistrationReview != null)
                car.RegistrationReview = (DateTime)dto.RegistrationReview;

            if (dto.Insurance != null)
                car.Insurance = (DateTime)dto.Insurance;

            if (dto.TireChange != null)
                car.TireChange = (DateTime)dto.TireChange;
            
            if (dto.UserId != null)
            {
                User user = _context.Users.FirstOrDefault(x => x.Id == dto.UserId);
                if (user == null)
                    throw new NotFoundException($"User with id {dto.UserId} not exist");
                car.UserId = dto.UserId;
            }

            _context.SaveChanges();
            return car;
        }
    }
}
