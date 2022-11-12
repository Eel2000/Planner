using Microsoft.EntityFrameworkCore;
using Planner.core.Data;
using Planner.core.DTOs;
using Planner.core.DTOsm;
using Planner.core.Models;

namespace Planner.core.Services;

public class PlannerMainService
{
    private readonly ApplicationDbContext _context;

    public PlannerMainService(ApplicationDbContext context)
    {
        _context = context;
    }

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
            .Include(w => w.Projects)
            .ThenInclude(p => p.Tag)
            .ToListAsync();
        return new Response<IReadOnlyList<WorkSpace>>(Status.Success, $"User {userId} workspaces", workspaces);
    }

    public async Task<Response<string>> RemoveWorkSpaceAsync(string userId, string workspaceId)
    {
        var toRemove = await _context.WorkSpaces.FirstOrDefaultAsync(w => w.Id == workspaceId);
        if (toRemove == null)
        {
            return new Response<string>(Status.Error, "Error on deleting the workspace. Please try again later.");
        }
        _context.WorkSpaces.Remove(toRemove);
        var projects = await _context.Projects.Where(p => p.WorkSpaceId == workspaceId).ToListAsync();
        if (projects.Any())
        {
            _context.Projects.RemoveRange(projects);
            //projects.ForEach(async projects=>
            //{
            //    await _context.ToDos.Where(x=>x.)
            //})
        }
    }
}
