﻿using AutoMapper;
using HardwareReview.Dto;
using HardwareReview.Models;
using HardwareReview.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HardwareReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController: Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var category = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int Id)
        {
            if (!_categoryRepository.CategoryExists(Id))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategoryById(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("Hardware/{CategoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Hardware>))]
        [ProducesResponseType(400)]
        public IActionResult GetHardwareByCategory(int CategoryId)
        {
            var hardwares = _mapper.Map<List<HardwareDto>>(_categoryRepository.GetHardwaresByCategory(CategoryId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(hardwares);
        }


    }
}
