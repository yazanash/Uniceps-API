﻿using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineController : ControllerBase
    {
        private readonly IDataService<Routine> _dataService;
        private readonly IEntityQueryDataService<Routine> _entityQueryDataService;
        private ILogger<RoutineController> _logger;
        IMapperExtension<Routine, RoutineDto, RoutineCreationDto> _mapper;
        public RoutineController(ILogger<RoutineController> logger, IMapperExtension<Routine, RoutineDto, RoutineCreationDto> mapper, IDataService<Routine> dataService, IEntityQueryDataService<Routine> entityQueryDataService)
        {
            _logger = logger;
            _mapper = mapper;
            _dataService = dataService;
            _entityQueryDataService = entityQueryDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Routine> routines = await _dataService.GetAll();
            return Ok(routines.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Routine routine = await _dataService.Get(id);
            return Ok(_mapper.ToDto(routine));
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoutineCreationDto routineCreationDto)
        {
            if (routineCreationDto == null)
                return BadRequest("Exercise data is missing.");

            Routine routine = _mapper.FromCreationDto(routineCreationDto);
            var result = await _dataService.Create(routine);
            _logger.LogInformation("Created Successfully");
            return Ok(_mapper.ToDto(routine));
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoutineCreationDto routineCreationDto)
        {
            Routine routine = _mapper.FromCreationDto(routineCreationDto);
            routine.NID = id;
            var result = await _dataService.Update(routine);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
