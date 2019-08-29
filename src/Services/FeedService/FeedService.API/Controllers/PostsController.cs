﻿using System;
using System.Linq;
using System.Security.Claims;
using FeedService.Domain.Commands.PostCommands;
using FeedService.Domain.Commands.PostCommands.Comment;
using FeedService.Domain.Repositories;
using FeedService.Infrastructure.CQRS;
using FeedService.Infrastructure.InfraServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPersonRepository _personRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;

        public PostsController(IMediatorHandler mediatorHandler,
            IPostRepository postRepository,
            IPersonRepository personRepository,
            IUserService userService)
        {
            _mediatorHandler = mediatorHandler;
            _postRepository = postRepository;
            _personRepository = personRepository;
            _userService = userService;
        }

        [HttpGet("Identity")]
        public IActionResult Identity()
        {
            var claim = (ClaimsIdentity) User.Identity;
            var nameIdentifier = claim.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value)
                .SingleOrDefault();
            var role = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            var name = claim.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            return Ok(nameIdentifier + role + name);
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_postRepository.GetAll());
        }

        [HttpGet("Feed")]
        public IActionResult GetMyFeed()
        {
            var me = _personRepository.GetByUserId(_userService.UserId);
            var feed = _postRepository.GetMyFeed(me);

            return Ok(feed);
        }

        [HttpGet("MyPosts")]
        public IActionResult GetMyPosts()
        {
            var myPosts = _postRepository.GetMyPosts(_userService.UserId);
            return Ok(myPosts);
        }

        [HttpGet("MyPostCount")]
        public IActionResult GetMyPostCount()
        {
            var me = _personRepository.GetByUserId(_userService.UserId);
            if (me == null) return Ok();

            var myPostsCount = _postRepository.GetMyPosts(me.PersonId).Count();
            return Ok(myPostsCount);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(Guid id)
        {
            var me = _personRepository.GetByUserId(_userService.UserId); 
            return Ok(_postRepository.GetByIdAndPersonId(id, me.PersonId));
        }

        [HttpPost]
        public void Post([FromBody] CreatePost cmd)
        {
            cmd.Id = Guid.NewGuid();
            _mediatorHandler.SendCommand(cmd);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var cmd = new DeletePost
            {
                Id = id,
            };
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("Comment")]
        public IActionResult AddComment([FromBody] AddComment cmd)
        {
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }

        [HttpPost("RemoveComment")]
        public IActionResult RemoveComment(RemoveComment cmd)
        {
            _mediatorHandler.SendCommand(cmd);
            return Ok();
        }
    }
}