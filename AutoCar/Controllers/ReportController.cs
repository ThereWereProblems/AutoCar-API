using AutoCar.Entities;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.InteropServices;
using System;
using AutoCar.Models.DTO;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetReports()
        {
            var reports = _reportService.GetReports();

            return Ok(reports);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddNewReport([FromBody] ReportDto dto)
        {
            _reportService.AddNewReport(dto);

            return Ok();
        }

        [HttpPut("{repId}")]
        [Authorize]
        public ActionResult ChangeReady([FromRoute] int repId)
        {
            bool value = _reportService.ChangeReady(repId);

            return Ok(value); 
        }

        [HttpGet("{ready}")]
        [Authorize]
        public ActionResult CountActiveReports()
        {
            var value = _reportService.CountActiveReports();

            return Ok(value);
        }
    }
}
