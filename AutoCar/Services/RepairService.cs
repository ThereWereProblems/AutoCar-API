using AutoCar.Entities;
using AutoCar.Exceptions;
using AutoCar.Migrations;
using AutoCar.Models.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AutoCar.Services
{
    public interface IRepairService
    {
        public void OrderRepair(RepairDto dto);
        public void DoRepair(int id, DoRepairDto dto);
        public List<Repair> GetRepairs();
        public List<Repair> GetToDoRepairs();
        public Repair GetRepair(int id);
        public void EditRepair(int id, RepairDto dto);
        public void EditDoneRepair(int id, DoRepairDto dto);
        public void DeleteRepair(int id);
    }

    public class RepairService : IRepairService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public RepairService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public void OrderRepair(RepairDto dto)
        {
            if (String.IsNullOrEmpty(dto.Title) || String.IsNullOrEmpty(dto.Description))
                throw new BadRequestException("Tile or description is empty");
            Repair repair= new Repair
            {
                Title= dto.Title,
                Description= dto.Description,
                IsDone = false
            };

            _context.Repairs.Add(repair);
            _context.SaveChanges();
        }

        public void DoRepair(int id, DoRepairDto dto)
        {
            Repair repair = _context.Repairs.FirstOrDefault(x => x.Id == id);

            if(repair == null)
                throw new BadRequestException($"Repair with id: {id} not exist");

            if (repair.IsDone)
                throw new BadRequestException($"Repair is already done");

            if(String.IsNullOrEmpty(dto.ServiceNote))
                throw new BadRequestException("Note is empty");

            repair.ServiceNote = dto.ServiceNote;
            repair.Cost= dto.Cost;
            repair.IsDone= true;

            _context.SaveChanges();
        }

        public List<Repair> GetRepairs()
        {
            var list = _context.Repairs.ToList();

            return list;
        }

        public List<Repair> GetToDoRepairs()
        {
            var list = _context.Repairs.Where(x => x.IsDone == false).ToList();

            return list;
        }

        public Repair GetRepair(int id)
        {
            var repair = _context.Repairs.FirstOrDefault(x => x.Id == id);

            if (repair == null)
                throw new NotFoundException($"Repair with id: {id} does not exist");
            
            return repair;
        }

        public void EditRepair(int id, RepairDto dto)
        {
            Repair repair = _context.Repairs.FirstOrDefault(x => x.Id == id);

            if (repair == null)
                throw new BadRequestException($"Repair with id: {id} not exist");

            if (repair.IsDone)
                throw new BadRequestException($"Repair is already done");

            if (String.IsNullOrEmpty(dto.Title) || String.IsNullOrEmpty(dto.Description))
                throw new BadRequestException("Tile or description is empty");

            repair.Title = dto.Title;
            repair.Description = dto.Description;

            _context.SaveChanges();
        }

        public void EditDoneRepair(int id, DoRepairDto dto)
        {
            Repair repair = _context.Repairs.FirstOrDefault(x => x.Id == id);

            if (repair == null)
                throw new BadRequestException($"Repair with id: {id} not exist");

            if (!repair.IsDone)
                throw new BadRequestException($"Repair is not done");

            if (String.IsNullOrEmpty(dto.ServiceNote))
                throw new BadRequestException("Note is empty");

            repair.ServiceNote = dto.ServiceNote;
            repair.Cost = dto.Cost;

            _context.SaveChanges();
        }

        public void DeleteRepair(int id)
        {
            Repair repair = _context.Repairs.FirstOrDefault(x => x.Id == id);

            if (repair == null)
                throw new BadRequestException($"Repair with id: {id} not exist");

            _context.Repairs.Remove(repair);
            _context.SaveChanges();
        }
    }
}
