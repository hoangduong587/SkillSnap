using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;
using SkillSnap.Shared;
using System.Security.Claims;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require login
    public class SkillsController : ControllerBase
    {
        private readonly SkillSnapContext _context;

        public SkillsController(SkillSnapContext context)
        {
            _context = context;
        }

        // ----------------------------------------------------
        // GET SKILLS FOR LOGGED-IN USER
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetMySkills()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var skills = await _context.Skills
                .Where(s => s.PortfolioUserId == userId)
                .ToListAsync();

            var dtoList = skills.Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Level = s.Level,
                PortfolioUserId = s.PortfolioUserId
            });

            return Ok(dtoList);
        }

        // ----------------------------------------------------
        // CREATE SKILL (User creates their own skill)
        // ----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<SkillDto>> Create(SkillDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var skill = new Skill
            {
                Name = dto.Name,
                Level = dto.Level,
                PortfolioUserId = userId // string GUID
            };

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            dto.Id = skill.Id;
            dto.PortfolioUserId = skill.PortfolioUserId;

            return CreatedAtAction(nameof(GetMySkills), new { id = skill.Id }, dto);
        }

        // ----------------------------------------------------
        // UPDATE SKILL (User can only update their own skill)
        // ----------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SkillDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found.");

            if (skill.PortfolioUserId != userId)
                return Forbid(); // Prevent editing others' skills

            skill.Name = dto.Name;
            skill.Level = dto.Level;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ----------------------------------------------------
        // DELETE SKILL (User can only delete their own skill)
        // ----------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound("Skill not found.");

            if (skill.PortfolioUserId != userId)
                return Forbid(); // Prevent deleting others' skills

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
