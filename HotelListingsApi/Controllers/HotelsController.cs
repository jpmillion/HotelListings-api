using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingsApi.Data;
using AutoMapper;
using HotelListingsApi.Contracts;
using HotelListingsApi.Dto.Hotel;

namespace HotelListingsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRepository _hotelsRepository;

        public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _hotelsRepository = hotelsRepository;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            List<Hotel> hotels = await _hotelsRepository.GetAllAsync();
            List<HotelDto> hotelDtos = _mapper.Map<List<HotelDto>>(hotels);
            return Ok(hotelDtos);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            HotelDto hotelDto = _mapper.Map<HotelDto>(hotel);

            return hotelDto;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, HotelDto hotelRequest)
        {
            if (id != hotelRequest.Id)
            {
                return BadRequest();
            }

            Hotel hotel =  await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            Hotel updatedHotel = _mapper.Map(hotelRequest, hotel);

            try
            {
                await _hotelsRepository.UpdateAsync(updatedHotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelDto>> AddHotel(AddHotelRequestDto hotelRequest)
        {
            Hotel hotelToAdd = _mapper.Map<Hotel>(hotelRequest);

            Hotel hotel = await _hotelsRepository.AddAsync(hotelToAdd);

            HotelDto hotelDto = _mapper.Map<HotelDto>(hotel);

            return CreatedAtAction("GetHotel", new { id = hotelDto.Id }, hotelDto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            Hotel hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelsRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelsRepository.Exists(id);
        }
    }
}
