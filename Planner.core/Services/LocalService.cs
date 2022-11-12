using Planner.core.Models;

namespace Planner.core.Services
{
    public class LocalService
    {
        public List<WorkSpace>? ConnectedUserWorkSpaces { get; set; }
        public User? ConnectedUser { get; set; }

        /// <summary>
        /// save in the local service for late user through the session
        /// </summary>
        /// <param name="workSpace">the workspace to save</param>
        public void KeepLoadedWorkspaceForLaterUser(WorkSpace workSpace)
        {
            if (ConnectedUserWorkSpaces is null)
            {
                ConnectedUserWorkSpaces = new List<WorkSpace>();
            }

            var isSaved = ConnectedUserWorkSpaces.Any(x => x.Id == workSpace.Id);
            if (isSaved)
            {
                var tempList = new List<WorkSpace>(ConnectedUserWorkSpaces);
                tempList.Remove(workSpace);
                ConnectedUserWorkSpaces.AddRange(tempList);
            }
            else if (!isSaved)
            {
                ConnectedUserWorkSpaces.Add(workSpace);
            }
        }

        /// <summary>
        /// Save many at once
        /// </summary>
        /// <param name="workSpaces">workspaces</param>
        public void KeepLoadedWorkspaceForLaterUser(List<WorkSpace> workSpaces)
        {
            if (ConnectedUserWorkSpaces is null)
            {
                ConnectedUserWorkSpaces = new List<WorkSpace>();
            }
            ConnectedUserWorkSpaces.Clear();//refresh data
            ConnectedUserWorkSpaces.AddRange(workSpaces);
        }

        /// <summary>
        /// save the current user data state
        /// </summary>
        /// <param name="user">user data to save</param>
        public void SaveConnectedUserState(User user) => ConnectedUser = user;
    }
}
