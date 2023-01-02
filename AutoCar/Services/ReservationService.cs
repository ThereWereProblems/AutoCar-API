using AutoCar.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System;
using AutoCar.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoCar.Models.DTO;
using AutoCar.Migrations;

namespace AutoCar.Services
{
    public interface IReservationService
    {
        public List<Reservation> GetReservations();
        public List<Reservation> GetReservationsByUserId(int userId);
        public List<Reservation> GetReservationsByUserIdAndCarId(int userId, int carId);
        public bool IsCarFree(int carId, DateTime dos);
        public int AddNewReservation(ReservationDto reservation);
        public bool ChangeFinished(int resvId);
        public void UpdateReservation(int id, UpdateReservationDto dto);
        public void DeleteReservation(int id);
    }

    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;


        public ReservationService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public List<Reservation> GetReservations()
        {
            var list = _context.Reservations.ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Reservations");

            return list;
        }

        public List<Reservation> GetReservationsByUserId(int userId)
        {
            var list = _context.Reservations.Where(x => x.UserId == userId).ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Reservations");

            return list;
        }

        public List<Reservation> GetReservationsByUserIdAndCarId(int userId, int carId)
        {
            var list = _context.Reservations.Where(x => x.UserId == userId && x.CarId == carId).ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Reservations");

            return list;
        }

        public bool IsCarFree(int carId, DateTime dos)
        {
            Car car = _context.Cars.Include(x => x.Reservations).FirstOrDefault(x => x.Id == carId);
            if (car == null)
            {
                throw new NotFoundException($"Car {carId} does not exist");
            }
            foreach (var item in car.Reservations)
            {
                if (dos > item.Dos && dos < item.Doe)
                    return false;
            }
            return true;
        }

        public int AddNewReservation(ReservationDto reservation)
        {
            if (reservation.Dos > reservation.Doe)
            {
                throw new BadRequestException("Date of start is after the date of end!");
            }
            if (!IsCarFree(reservation.CarId, reservation.Dos))
            {
                throw new BadRequestException("Car is already taken!");
            }

            Reservation resv = new Reservation
            {
                Dos = reservation.Dos,
                Doe = reservation.Doe,
                CarId = reservation.CarId,
                UserId = (int)_userContextService.GetUserId
            };

            _context.Reservations.Add(resv);
            _context.SaveChanges();

            return resv.Id;
        }

        public bool ChangeFinished(int resvId)
        {
            Reservation reservation = _context.Reservations.FirstOrDefault(x => x.Id == resvId);

            if (reservation == null)
            {
                throw new NotFoundException($"Reservation {resvId} does not exist");
            }

            reservation.Finished = !reservation.Finished;
            _context.SaveChanges();

            return reservation.Finished;
        }

        public void UpdateReservation(int id, UpdateReservationDto dto)
        {
            Reservation reservation = _context.Reservations.FirstOrDefault(x => x.Id == id);

            if (reservation == null)
            {
                throw new NotFoundException($"Reservation {id} does not exist");
            }

            if (dto.Dos != null)
                reservation.Dos = dto.Dos.Value;

            if (dto.Doe != null)
                reservation.Doe = dto.Doe.Value;

            if (dto.UserId != null)
                reservation.UserId = dto.UserId.Value;
            if (dto.CarId != null)
                reservation.CarId = dto.CarId.Value;

            _context.SaveChanges();
        }

        public void DeleteReservation(int id)
        {
            Reservation reservation = _context.Reservations.FirstOrDefault(x => x.Id == id);

            if (reservation == null)
            {
                throw new NotFoundException($"Reservation {id} does not exist");
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }
    }
}
