using AutoCar.Entities;
using AutoMapper;
using System.Runtime.InteropServices;
using System;
using AutoCar.Models.DTO;
using AutoCar.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AutoCar.Services
{
    public interface IReportService
    {
        public List<Report> GetReports();
        public void AddNewReport(ReportDto dto);
        public bool ChangeReady(int repId);
        public int CountActiveReports();
    }

    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public ReportService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService) 
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public List<Report> GetReports()
        {
            var list = _context.Reports.ToList();

            if (list == null)
                throw new NotFoundException("Empty list of Reports");

            return list;
        }

        public void AddNewReport(ReportDto dto)
        {
            var user = (int)_userContextService.GetUserId;

            Report report = new Report
            {
                Message = dto.Message,
                UserId = user,
                Ready = false
            };

            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public bool ChangeReady(int repId)
        {
            var report = _context.Reports.FirstOrDefault(x => x.Id == repId);

            if (report  == null)
                throw new NotFoundException("Report not exists");

            report.Ready = !report.Ready;
            _context.SaveChanges();

            return report.Ready;
        }

        public int CountActiveReports()
        {
            var list = _context.Reports.Where(x => x.Ready == false).ToList();

            return list.Count;
        }
    }
}
