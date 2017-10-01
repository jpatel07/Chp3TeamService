using System;
using StatlerWaldorfCorp.TeamService.Models;
using StatlerWaldorfCorp.TeamService.Persisistence;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace StatlerWaldorfCorp.TeamService
{
    public class MembersController : Controller
    {
        ITeamRepository repository;
        public MembersController(ITeamRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public virtual IActionResult GetMember(Guid teamID)
        {
            Team team = repository.Get(teamID);
            
            if(team == null)
            {
                return this.NotFound();
            }
            else
            {
                return this.Ok(team.Members);
            }
        }

        [HttpGet]
        [Route("/teams/{teamId}/[controller]/{memberId}")]
        public virtual IActionResult GetMember(Guid teamID, Guid memberId)
        {
            Team team = repository.Get(teamID);
            if(team == null)
            {
                return this.NotFound();
            }
            else
            {
                var q = team.Members.Where(m => m.ID == memberId);
                if(q.Count() < 1)
                {
                    return this.NotFound();
                }
                else
                {
                    return this.Ok(q.First());
                }
                
            }
        }


        [HttpPost]
        public virtual IActionResult CreateMember([FromBody]Member newMember, Guid teamID)
        {
            Team team = repository.Get(teamID);

            if(team == null)
            {
                return this.NotFound();
            }

            else 
            {
                team.Members.Add(newMember);
                var teamMember = new {TeamID = team.ID, MemberID = newMember.ID};
                return this.Created($"teams/{teamMember.TeamID}/[controller]/{teamMember.MemberID}",teamMember);
            }
        }

    }
}