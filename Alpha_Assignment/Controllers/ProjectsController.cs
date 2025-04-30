using Alpha_Assignment.ViewModels;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Assignment.Controllers;



[Route("Projects")]
[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IStatusService _statusService;
    private readonly IClientService _clientService;


    public ProjectsController(IProjectService projectService, IStatusService statusService, IClientService clientService)
    {
        _projectService = projectService;
        _statusService = statusService;
        _clientService = clientService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Projects()
    {
        var statusResult = await _statusService.GetStatusesAsync();
        var clientResult = await _clientService.GetClientsAsync();

        var vm = new ProjectsViewModel
        {
            Projects = await _projectService.GetAllProjectsAsync(),
            AddProjectForm = new AddProjectForm
            {
                Statuses = statusResult.Result ?? [],
                Clients = clientResult.Result ?? [],

            },
        };

       
        return View(vm);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProjects(AddProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.CreateProjectAsync(form);

        if (result.Succeeded)
        {
            return RedirectToAction("Projects");
        }
        else
        {
            return Problem("Unable to submit data");
        }
    }


    //[HttpGet("editmodal/{id}")]
    //public async Task<IActionResult> GetEditModal(string id)
    //{
    //    var form = await _projectService.GetProjectByIdAsync(id);
    //    if (form == null)
    //        return NotFound();

    //    ViewBag.Clients = await _clientService.GetClientNamesAsync();    
    //    ViewBag.Statuses = await _statusService.GetStatusNamesAsync(); 

    //    return PartialView("_EditProjectPartial", form);
    //}


    [HttpPost("edit")]
    public async Task<IActionResult> EditProjects(EditProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.UpdateProjectAsync(form);

        if (result.Succeeded)
        {
            return Ok(new { Success = true });
        }
        else
        {
            return Problem("Unable to submit data");
        }
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteProject(string id)
    {
       

        var result = await _projectService.DeleteProjectAsync(id);

        if (result.Succeeded)
        {
            return Ok(new { Success = true });
        }
        else
        {
            return BadRequest(new { Success = false, error = result.Error });
        }
    }


   

}




