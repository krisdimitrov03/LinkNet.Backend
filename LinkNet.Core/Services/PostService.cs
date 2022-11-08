using LinkNet.Core.Contracts;
using LinkNet.Core.Data.Models;
using LinkNet.Infrastructure.Data.Models;
using LinkNet.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNet.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IApplicationDbRepository repo;

        public PostService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<List<PostForUserDto>> GetByUser(string userId)
        {
            try
            {
                return await repo.All<Post>()
                    .Where(p => p.UserId == userId)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Image)
                    .Select(p => new PostForUserDto
                    {
                        Id = p.Id,
                        Images = p.Images
                        .Select(i => i.Url)
                        .ToArray(),
                        User = new UserInPostDto
                        {
                            Id = p.User.Id,
                            FullName = $"{p.User.FirstName} {p.User.LastName}",
                            Image = p.User.Image.Url
                        },
                        //move to LikeService
                        Likes = p.Likes.ToList(),
                        //move to CommentService
                        Comments = p.Comments.ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<PostForUserDto>();
            }
        }
    }
}
