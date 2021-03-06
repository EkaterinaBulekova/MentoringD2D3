﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncUserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.User.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllusers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "userById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var users = await _repository.User.GetUserByIdAsync(id);

                if (users.Count == 0)
                {
                    _logger.LogError($"user with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var user = users.Single();

                _logger.LogInfo($"Returned user with id: {id}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetuserById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

         [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            try
            {
                if (user.IsObjectNull())
                {
                    _logger.LogError("user object sent from client is null.");
                    return BadRequest("user object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }

                await _repository.User.CreateUserAsync(user);

                return CreatedAtRoute("userById", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Createuser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody]User user)
        {
            try
            {
                if (user.IsObjectNull())
                {
                    _logger.LogError("user object sent from client is null.");
                    return BadRequest("user object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var DbUsers = await _repository.User.GetUserByIdAsync(id);

                if (DbUsers.Count == 0)
                {
                    _logger.LogError($"user with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                
                var DbUser = DbUsers.Single();

                await _repository.User.UpdateUserAsync(DbUser, DbUser);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Updateuser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var users = await _repository.User.GetUserByIdAsync(id);

                if (users.Count == 0)
                {
                    _logger.LogError($"user with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var user = users.Single();

                await _repository.User.DeleteUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Deleteuser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}