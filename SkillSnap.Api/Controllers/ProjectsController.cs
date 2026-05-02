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
    public class ProjectsController : ControllerBase
    {
        private readonly SkillSnapContext _context;

        public ProjectsController(SkillSnapContext context)
        {
            _context = context;
        }

        // ----------------------------------------------------
        // GET PROJECTS FOR LOGGED-IN USER
        // ----------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetMyProjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var projects = await _context.Projects
                .Where(p => p.PortfolioUserId == userId)
                .ToListAsync();

            var dtoList = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                PortfolioUserId = p.PortfolioUserId
            });

            return Ok(dtoList);
        }

        // ----------------------------------------------------
        // CREATE PROJECT (User creates their own project)
        // ----------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(ProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                PortfolioUserId = userId // string GUID
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            dto.Id = project.Id;
            dto.PortfolioUserId = project.PortfolioUserId;

            return CreatedAtAction(nameof(GetMyProjects), new { id = project.Id }, dto);
        }

        // ----------------------------------------------------
        // UPDATE PROJECT (User can only update their own project)
        // ----------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound("Project not found.");

            if (project.PortfolioUserId != userId)
                return Forbid(); // Prevent editing others' projects

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ----------------------------------------------------
        // DELETE PROJECT (User can only delete their own project)
        // ----------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound("Project not found.");

            if (project.PortfolioUserId != userId)
                return Forbid(); // Prevent deleting others' projects

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
