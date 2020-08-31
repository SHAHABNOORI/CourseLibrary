using System;
using System.Collections.Generic;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    //[Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(/*"api/authors"*/)]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(/*[FromQuery(Name="")]*/ /*string mainCategory,string searchQuery*/
            [FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            //throw new Exception("Test exception");
            var autorsFromRepo = _courseLibraryRepository.GetAuthors(/*mainCategory, searchQuery*/authorsResourceParameters);
            //var authors = autorsFromRepo.Select(author => new AuthorDto()
            //{
            //    Id = author.Id, 
            //    FullName = $"{author.FirstName} {author.LastName}", 
            //    MainCategory = author.MainCategory,
            //    Age = author.DateOfBirth.GetCurrentAge()
            //}).ToList();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(autorsFromRepo));
        }

        //[HttpGet("{authorId:guid{")]
        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            //if (!_courseLibraryRepository.AuthorExists(authorId))
            //    return NotFound();

            var autorFromRepo = _courseLibraryRepository.GetAuthor(authorId);
            return autorFromRepo == null ? (IActionResult)NotFound() : Ok(_mapper.Map<AuthorDto>(autorFromRepo));
            //return autorsFromRepo == null ? (IActionResult)NotFound() : Ok(autorsFromRepo);
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            //if (author == null)
            //    return BadRequest();

            var authorEntity = _mapper.Map<Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();
            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new {authorId = authorToReturn.Id}, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow","GET,OPTIONS,POST");
            return Ok();
        }
    }
}