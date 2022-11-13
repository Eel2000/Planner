using Microsoft.EntityFrameworkCore;
using Planner.core.Data;
using Planner.core.DTOs;
using Planner.core.Models;

namespace Planner.core.Services;

public class PlannerMainService
{
    private readonly ApplicationDbContext _context;

    public PlannerMainService(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Workspace

    public async Task<Response<WorkSpace>> CreateWorkSpaceAsync(WorkspaceDTO workspace)
    {
        if (await _context.WorkSpaces.AnyAsync(x => x.UserId == workspace.UserId && x.Name.ToUpper() == workspace.Name.ToUpper()))
        {
            return new Response<WorkSpace>(Status.Error, "You already have a workspace with an identical name");
        }

        var space = new WorkSpace()
        {
            UserId = workspace.UserId,
            Name = workspace.Name,
            Description = workspace.Description,
        };
        await _context.WorkSpaces.AddAsync(space);
        await _context.SaveChangesAsync();
        //getting the latest created workspace
        var createdWorkspace = await _context.WorkSpaces
            .Include(w => w.Projects)
            .OrderByDescending(w => w.CreateAt)
            .FirstAsync();
        return new Response<WorkSpace>(Status.Success, "Your workspace has been created please we are loading it", createdWorkspace);
    }

    public async Task<Response<IReadOnlyList<WorkSpace>>> GetUserWorkspacesAsync(string userId)
    {
        if (userId == null)
        {
            return new Response<IReadOnlyList<WorkSpace>>(Status.Failure, "Please specify the user");
        }
        var workspaces = await _context.WorkSpaces
            .Include(w => w.User)
            .Include(w => w.Projects)
                .ThenInclude(p => p.Tag)
            .ToListAsync();
        return new Response<IReadOnlyList<WorkSpace>>(Status.Success, $"User {userId} workspaces", workspaces);
    }

    public async Task<Response<string>> RemoveWorkSpaceAsync(string userId, string workspaceId)
    {
        var toRemove = await _context.WorkSpaces.FirstOrDefaultAsync(w => w.Id == workspaceId && w.UserId == userId);
        if (toRemove == null)
        {
            return new Response<string>(Status.Error, "Error on deleting the workspace. Please try again later.");
        }
        _context.WorkSpaces.Remove(toRemove);
        var projects = await _context.Projects.Where(p => p.WorkSpaceId == workspaceId).ToListAsync();
        if (projects.Any())
        {
            _context.Projects.RemoveRange(projects);
            var pTodos = new List<ToDo>();
            projects.ForEach(async projects =>
            {
                var pTodos = await _context.ToDos.Where(t => t.ProjectId == projects.Id).ToListAsync();
                pTodos.AddRange(pTodos);
            });

            if (pTodos.Any())
            {
                _context.ToDos.RemoveRange(pTodos);
            }
        }
        await _context.SaveChangesAsync();

        return new Response<string>(Status.Success, $"The workspace {toRemove.Name} has been deleted");
    }

    public async Task<Response<WorkSpace>> GetUserWorkingSpaceAsync(string userId, string workspaceId)
    {
        var workspace = await _context.WorkSpaces.Where(w => w.UserId == userId && w.Id == workspaceId)
            .Include(w => w.Projects)
                .ThenInclude(p=>p.Tag)
            .FirstOrDefaultAsync();

        return new Response<WorkSpace>(Status.Success, "working space retreived", workspace);
    }
    #endregion

    #region Project

    public async Task<Response<IReadOnlyList<Project>>> GetWorkSpaceProjectAsync(string id)
    {
        var projects = await _context.Projects
            .Where(p => p.WorkSpaceId == id)
            .Include(p => p.Tag)
            .Include(p => p.ToDos)
            .Include(p => p.Collaborators)
            .OrderBy(x=>x.ProjectName)
            .ToListAsync();

        return new Response<IReadOnlyList<Project>>(Status.Success, "Workspace project loaded", projects);
    }

    public async Task<Response<Project>> AddProjectToWorkSpaceAsync(ProjectDTO project)
    {
        if(!await _context.Projects.AnyAsync(p => p.ProjectName == project.ProjectName))
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == project.TagId);
            if(tag is null)
            {
                tag = new Tag
                {
                    ColorHex = "Info",
                    Name = project.TagId
                };
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();
            }

            var projectNew = new Project
            {
                ProjectName = project?.ProjectName,
                ProjectDescription = project.ProjectDescription,
                WorkSpaceId = project.WorkSpaceId,
                IsArchived = false,
                TagId = tag.Id
            };


            await _context.Projects.AddAsync(projectNew);
            await _context.SaveChangesAsync();

            return new Response<Project>(Status.Success, "Project created",projectNew);
        }

        return new Response<Project>(Status.Failure, "There is already a project with the same name");
    }
    #endregion
}
