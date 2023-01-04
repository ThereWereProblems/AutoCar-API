using AutoCar.Entities;
using AutoCar.Exceptions;
using AutoCar.Models.DTO;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace AutoCar.Services
{
    public interface IMarkService
    {
        public List<Mark> GetMarks();
        public List<Mark> GetMarksByCarId(int carId);
        public void AddNewMark(MarkDto dto);
    }

    public class MarkService : IMarkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public MarkService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public List<Mark> GetMarks()
        {
            var list = _context.Marks.ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Marks");

            return list;
        }

        public List<Mark> GetMarksByCarId(int carId)
        {
            var list = _context.Marks.Where(x => x.CarId == carId).ToList();

            return list;
        }

        public void AddNewMark(MarkDto dto)
        {
            Mark mark = new Mark
            {
                CarId = dto.CarId,
                Value = dto.Value,
                UserId = (int)_userContextService.GetUserId
            };

            _context.Marks.Add(mark);
            _context.SaveChanges();
        }
    }
}
