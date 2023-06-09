

using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetUsers()
        {
            var users = await _userRepository.GetProfilesAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDto>> GetUser(int id)
        {
            return await _userRepository.GetProfileAsync(id);

        }

    }
}