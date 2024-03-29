﻿using AutoMapper;
using HardwareReview.Dto;
using HardwareReview.Models;
using HardwareReview.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HardwareReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController: Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int Id)
        {
            if (!_countryRepository.CountryExists(Id))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryById(Id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("Company/{CompanyId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfCompany(int CompanyId)
        {
           var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByCompany(CompanyId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() ==
            countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country is not null)
            {
                ModelState.AddModelError("", "country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updateCountry)
        {
            if (updateCountry == null)
                return BadRequest(ModelState);

            if (countryId != updateCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(updateCountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Somthing went wrong while Updating");
                return StatusCode(500, ModelState);
            }

            return Ok("Udated successfully");
        }


    }
}
